﻿using Kr.Communication.SmartModbusMaster.Modbus;
using ModbusTagManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ModbusTagManager.ModelView
{
    public enum LiveDataViewModelStatuses
    {
        Idle,
        Ready,
        Running
    }
    public class LiveDataViewModel: ObservableObject
    {
        private ObservableCollection<TagResult> _currentTags;
        private int _deviceCount;
        private LiveDataViewModelStatuses _status;
        private ModbusDevices myDevices;
        private Dictionary<string, TagResult> tagDictionary = new Dictionary<string, TagResult>();
        public LiveDataViewModel()
        {
            Status = LiveDataViewModelStatuses.Idle;
            FileOpenCommand = new RelayCommand(openFile, checkFile);
            StartCommand = new RelayCommand(startDevices, checkStatus);
            WriteCommand = new RelayCommand(writeTag, canWrite);
            CurrentTags = new ObservableCollection<TagResult>();
        }

        private bool canWrite(object obj)
        {
            TagResult tag = obj as TagResult;
            if (Status != LiveDataViewModelStatuses.Running)
            {
                return false;
            }
            if (tag == null)
            {
                return false;
            }
            if (tag.TagType == typeof(bool))
            {
                bool boolresult = false;
                return bool.TryParse(tag.WriteValue, out boolresult);
            }
            if (tag.TagType == typeof(float))
            {
                float floatresult = float.MinValue;
                return float.TryParse(tag.WriteValue, out floatresult);
            }
            if (tag.TagType == typeof(ushort))
            {
                ushort ushortresult = ushort.MinValue;
                return ushort.TryParse(tag.WriteValue, out ushortresult);
            }
            return false;
        }

        private void writeTag(object obj)
        {
            TagResult tag = obj as TagResult;
            myDevices.WriteTag(tag.Name, Convert.ChangeType(tag.WriteValue, tag.TagType));
        }

        public ObservableCollection<TagResult> CurrentTags
        {
            get { return _currentTags; }
            set { SetProperty(ref _currentTags, value); }
        }

        public int DeviceCount
        {
            get { return _deviceCount; }
            private set { SetProperty(ref _deviceCount, value); }
        }

        public ICommand FileOpenCommand { get; set; }

        public ICommand StartCommand { get; set; }

        public ICommand WriteCommand { get; set; }

        public LiveDataViewModelStatuses Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        private bool checkFile(object obj)
        {            
            if (obj == null || string.IsNullOrWhiteSpace(obj.ToString()))
            {
                return false;
            }
            return true;
        }

        private bool checkStatus(object obj)
        {
            if (Status == LiveDataViewModelStatuses.Ready)
            {
                return true;
            }
            return false;
        }
        private void create()
        {
            CurrentTags.Clear();
            tagDictionary.Clear();
            foreach (var device in myDevices.Values)
            {
                var readTags = from item in device.Collection.GetAllTags() where item.TagDirection == Kr.Communication.SmartModbusMaster.TagManagement.Types.Direction.Read select item;
                foreach (var tag in device.Collection.GetAllTags())
                {
                    if (tagDictionary.ContainsKey(tag.Name))
                    {
                        continue;
                    }
                    TagResult aTag = new TagResult(tag);
                    tagDictionary.Add(tag.Name, aTag);
                    tag.TagStatusChanged += Tag_TagStatusChanged;
                    CurrentTags.Add(aTag);
                }
            }
            DeviceCount = myDevices.Count;
            Status = LiveDataViewModelStatuses.Ready;
        }

        private void openFile(object obj)
        {
            if (!File.Exists(obj.ToString()))
            {
                throw new FileNotFoundException();
            }
            if (myDevices != null)
            {
                myDevices.KillAllDevices();
            }
            myDevices = Creator.FromFile(obj.ToString());
            if (myDevices == ModbusDevices.Empty)
            {
                System.Windows.MessageBox.Show("File not found or inaccessible","File Error",System.Windows.MessageBoxButton.OK,System.Windows.MessageBoxImage.Error);
            }
            create();
        }

        private void startDevices(object obj)
        {
            myDevices.StartDevices();
            Status = LiveDataViewModelStatuses.Running;
        }
        private void Tag_TagStatusChanged(Kr.Communication.SmartModbusMaster.TagManagement.Tag sender, object value, bool quality)
        {
            if (tagDictionary.ContainsKey(sender.Name))
            {
                tagDictionary[sender.Name].SetTag(sender);
            }
        }
    }
}
