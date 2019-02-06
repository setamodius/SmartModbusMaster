namespace Kr.Communication.SmartModbusMaster.Modbus
{
    using global::Modbus.Device;
    using System;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using System.Timers;
    using TagManagement.Types;

    internal class ModbusMaster : IDisposable
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private DateTime dtDisconnect = new DateTime();
        private DateTime dtNow = new DateTime();
        private bool isreading = false;
        private ModbusIpMaster master;
        private Device myDevice;
        private Timer myTimer = new Timer();

        private bool networkIsOk = false;
        private bool oldNetworkIsOk = false;
        private TcpClient tcpClient;

        public ModbusMaster(Device device)
        {
            if (device == null)
            {
                logger.Error("device is NULL");
                return;
            }
            myDevice = device;
            IpAddress = myDevice.Ip;
            ModbusPort = myDevice.Port;
            logger.Debug("ModbusMaster is creating - {0}", device.Name);
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
            logger.Debug("ModbusMaster is disposing - {0}", myDevice?.Name);
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
            logger.Debug("ModbusMaster is starting - {0}", myDevice?.Name);
            myTimer.Start();
        }

        public void WriteBoolTagValue(BoolTag tag)
        {
            foreach (var item in tag.GetAddresses())
            {
                master.WriteSingleCoilAsync((ushort)(item - 1), tag.GetWriteValue());
            }
        }

        public void WriteFloatTagValue(FloatTag tag)
        {
            int addressindex = 0;
            foreach (var item in tag.GetAddresses())
            {
                master.WriteSingleRegisterAsync((ushort)(item - 1), tag.GetWriteValue()[addressindex]);
                addressindex++;
                if (addressindex == 2)
                {
                    break;
                }
            }
        }

        public void WriteUshortTagValue(UshortTag tag)
        {
            try
            {
                master.WriteSingleRegister((ushort)(tag.GetAddresses()[0] - 1), tag.GetWriteValue());
            }
            catch
            {
                WriteUshortTagValue(tag);
            }
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
                        logger.Warn("Not connecting to server - {0}", IpAddress);
                        return false;
                    }
                    // create Modbus TCP Master by the tcpclient
                    master = ModbusIpMaster.CreateIp(tcpClient);
                    master.Transport.Retries = 0; //don't have to do retries
                    //master.Transport.ReadTimeout = 1500;
                    logger.Debug("Connected to server - {0}", IpAddress);
                    isreading = false;
                    return tcpClient.Connected;
                }
                catch (Exception ex)
                {
                    logger.Warn(ex);
                    return false;
                }
            }
            return false;
        }

        private void MyTimer_Tick(object sender, EventArgs e)
        {
            //WriteToConsole(this.GetHashCode().ToString());
            try
            {
                if (oldNetworkIsOk != networkIsOk)
                {
                    oldNetworkIsOk = networkIsOk;
                    ConnectionStateChanged?.Invoke(this, networkIsOk);
                }

                if (networkIsOk && !isreading)
                {
                    ReadValues();
                }
                else
                {
                    dtNow = DateTime.Now;
                    if ((dtNow - dtDisconnect) > TimeSpan.FromSeconds(10))
                    {
                        logger.Debug("Trying to connect - {0}", IpAddress);
                        networkIsOk = Connect();
                        if (!networkIsOk)
                        {
                            logger.Warn("Not connected - {0}", IpAddress);
                            dtDisconnect = DateTime.Now;
                        }
                    }
                    else
                    {
                        logger.Trace("Waiting for reconnect - {0}", IpAddress);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Source.Equals("System"))
                {
                    networkIsOk = false;
                    dtDisconnect = DateTime.Now;
                }
                logger.Warn(ex);
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
            //WriteToConsole(myWatch.ElapsedMilliseconds.ToString() + " ms");
        }
    }
}