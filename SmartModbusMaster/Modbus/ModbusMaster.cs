﻿namespace Kr.Communication.SmartModbusMaster.Modbus
{
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using System.Timers;
    using global::Modbus.Device;
    using Kr.Communication.SmartModbusMaster.Diagnostic;
    using TagManagement.Types;

    internal class ModbusMaster : IDisposable
    {
        private readonly ICoreLogger logger;
        private readonly Device myDevice;
        private readonly Timer myTimer = new Timer();
        private readonly Queue<BoolTag> boolWriteList = new Queue<BoolTag>();
        private DateTime dtDisconnect = new DateTime();
        private DateTime dtNow = new DateTime();
        private readonly Queue<FloatTag> floatWriteList = new Queue<FloatTag>();
        private bool isreading = false;
        private bool iswriting = false;
        private ModbusIpMaster master;
        private bool networkIsOk = false;
        private bool? oldNetworkIsOk;
        private TcpClient tcpClient;
        private readonly Queue<UshortTag> ushortWriteList = new Queue<UshortTag>();

        public ModbusMaster(Device device, ICoreLogger coreLogger)
        {
            logger = coreLogger ?? throw new ArgumentNullException(nameof(coreLogger));

            if (device == null)
            {
                logger.Fatal(null, "Device is NULL");
                return;
            }
            myDevice = device;
            IpAddress = myDevice.Ip;
            ModbusPort = myDevice.Port;
            logger.Info($"ModbusMaster is creating - {device.Name}");
            myTimer.Interval = myDevice.RefreshRate;
            myTimer.Elapsed += MyTimer_Tick;
        }

        ~ModbusMaster()
        {
        }

        public delegate void ConnectionStateChangedEventhandler(ModbusMaster sender, bool status);

        public event ConnectionStateChangedEventhandler ConnectionStateChanged;

        private enum InternetConnectionState : int
        {
            INTERNET_CONNECTION_MODEM = 0x1,
            INTERNET_CONNECTION_LAN = 0x2,
            INTERNET_CONNECTION_PROXY = 0x4,
            INTERNET_RAS_INSTALLED = 0x10,
            INTERNET_CONNECTION_OFFLINE = 0x20,
            INTERNET_CONNECTION_CONFIGURED = 0x40
        }

        public string IpAddress { get; private set; }

        public int ModbusPort { get; private set; }

        public void Dispose()
        {
            logger.Info($"ModbusMaster is disposing - {myDevice?.Name}");
            if (myTimer != null)
            {
                myTimer.Stop();
                myTimer.Dispose();
            }
            if (master != null)
            {
                master.Dispose();
            }
            if (tcpClient != null)
            {
                tcpClient.Close();
            }
        }

        public void Start()
        {
            logger.Info($"ModbusMaster is starting - {myDevice?.Name}");
            myTimer.Start();
        }

        public void WriteBoolTagValue(BoolTag tag)
        {
            boolWriteList.Enqueue(tag);
        }

        public void WriteFloatTagValue(FloatTag tag)
        {
            floatWriteList.Enqueue(tag);
        }

        public void WriteUshortTagValue(UshortTag tag)
        {
            ushortWriteList.Enqueue(tag);
        }

        [DllImport("WININET", CharSet = CharSet.Auto)]
        private static extern bool InternetGetConnectedState(ref InternetConnectionState lpdwFlags, int
        dwReserved);

        private bool CheckInternet()
        {
            InternetConnectionState flag = InternetConnectionState.INTERNET_CONNECTION_LAN;
            return InternetGetConnectedState(ref flag, 0);            
        }

        private bool Connect()
        {
            if (master != null)
                master.Dispose();
            if (tcpClient != null)
                tcpClient.Close();
            if (CheckInternet())
            {
                try
                {
                    tcpClient = new TcpClient();
                    IAsyncResult asyncResult = tcpClient.BeginConnect(IpAddress, ModbusPort, null, null);
                    asyncResult.AsyncWaitHandle.WaitOne(3000, true); //wait for 3 sec
                    if (!asyncResult.IsCompleted)
                    {
                        tcpClient.Close();
                        logger.Warning(null, $"Not connecting to server - {IpAddress}");
                        return false;
                    }
                    // create Modbus TCP Master by the tcpclient
                    master = ModbusIpMaster.CreateIp(tcpClient);
                    master.Transport.Retries = 0; //don't have to do retries
                    //master.Transport.ReadTimeout = 1500;
                    logger.Info($"Connected to server - {IpAddress}");
                    isreading = false;
                    return tcpClient.Connected;
                }
                catch (Exception ex)
                {
                    logger.Warning(ex, "Connection error");
                    return false;
                }
            }
            return false;
        }

        private void InnerWriteBoolTagValue(BoolTag tag)
        {
            foreach (var item in tag.GetAddresses())
            {
                master.WriteSingleCoil(myDevice.Id, (ushort)(item - 1), tag.GetWriteValue());
            }
        }

        private void InnerWriteFloatTagValue(FloatTag tag)
        {
            int addressindex = 0;
            foreach (var item in tag.GetAddresses())
            {
                master.WriteSingleRegister(myDevice.Id, (ushort)(item - 1), tag.GetWriteValue()[addressindex]);
                addressindex++;
                if (addressindex == 2)
                {
                    break;
                }
            }
        }

        private void InnerWriteUshortTagValue(UshortTag tag)
        {
            master.WriteSingleRegister(myDevice.Id, (ushort)(tag.GetAddresses()[0] - 1), tag.GetWriteValue());
        }

        private void MyTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (networkIsOk && !isreading && !iswriting)
                {
                    if (WriteValues() > 0)
                    {
                        return;
                    }
                }

                if (networkIsOk && !isreading && !iswriting)
                {
                    ReadValues();
                }
                else
                {
                    dtNow = DateTime.Now;
                    if ((dtNow - dtDisconnect) > TimeSpan.FromSeconds(10))
                    {
                        logger.Trace($"Trying to connect - {IpAddress}");
                        networkIsOk = Connect();
                        if (!networkIsOk)
                        {
                            logger.Warning(null, $"Not connected - {IpAddress}");
                            dtDisconnect = DateTime.Now;
                        }
                    }
                    else
                    {
                        logger.Trace($"Waiting for reconnect - {IpAddress}");
                    }
                }
                if (oldNetworkIsOk != networkIsOk)
                {
                    oldNetworkIsOk = networkIsOk;
                    ConnectionStateChanged?.Invoke(this, networkIsOk);
                }
            }
            catch (Exception ex)
            {
                if (ex.Source.Equals("System"))
                {
                    networkIsOk = false;
                    dtDisconnect = DateTime.Now;
                }
                logger.Warning(ex, "Read error");
            }
        }

        private void ReadValues()
        {
            System.Diagnostics.Stopwatch myWatch = new System.Diagnostics.Stopwatch();
            myWatch.Start();

            isreading = true;
            if (myDevice != null
                && myDevice.Collection != null
                && myDevice.Collection.CoilStatuses.Maps != null)
            {
                foreach (var item in myDevice.Collection.CoilStatuses.Maps)
                {
                    bool[] values = master.ReadCoils(myDevice.Id, (ushort)(item.StartAddress - 1), item.Range);
                    myDevice.Collection.CoilStatuses.RefreshValues(item, values);
                }
            }
            if (myDevice != null
                && myDevice.Collection != null
                && myDevice.Collection.InputStatuses.Maps != null)
            {
                foreach (var item in myDevice.Collection.InputStatuses.Maps)
                {
                    bool[] values = master.ReadInputs(myDevice.Id, (ushort)(item.StartAddress - 1), item.Range);
                    myDevice.Collection.InputStatuses.RefreshValues(item, values);
                }
            }
            if (myDevice != null
                && myDevice.Collection != null
                && myDevice.Collection.HoldingRegisters.Maps != null)
            {
                foreach (var item in myDevice.Collection.HoldingRegisters.Maps)
                {
                    ushort[] values = master.ReadHoldingRegisters(myDevice.Id, (ushort)(item.StartAddress - 1), item.Range);
                    myDevice.Collection.HoldingRegisters.RefreshValues(item, values);
                }
            }
            if (myDevice != null
                && myDevice.Collection != null
                && myDevice.Collection.InputRegisters.Maps != null)
            {
                foreach (var item in myDevice.Collection.InputRegisters.Maps)
                {
                    ushort[] values = master.ReadInputRegisters(myDevice.Id, (ushort)(item.StartAddress - 1), item.Range);
                    myDevice.Collection.InputRegisters.RefreshValues(item, values);
                }
            }
            isreading = false;
            myWatch.Stop();
        }

        private int WriteValues()
        {
            int writecount = 0;
            iswriting = true;
            while (boolWriteList.Count > 0)
            {
                var booltag = boolWriteList.Dequeue();
                InnerWriteBoolTagValue(booltag);
                writecount++;
            }

            while (floatWriteList.Count > 0)
            {
                var floattag = floatWriteList.Dequeue();
                InnerWriteFloatTagValue(floattag);
                writecount++;
            }

            while (ushortWriteList.Count > 0)
            {
                var ushorttag = ushortWriteList.Dequeue();
                InnerWriteUshortTagValue(ushorttag);
                writecount++;
            }
            iswriting = false;
            return writecount;
        }
    }
}