﻿using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ModbusTagManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseButtonClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myOpenFileDialog = new OpenFileDialog();
            if (myOpenFileDialog.ShowDialog() == true)
            {
                tbxFileLocation.Text = myOpenFileDialog.FileName;
            }
        }

        private void BtnOpenClick(object sender, RoutedEventArgs e)
        {
            if (!System.IO.File.Exists(tbxFileLocation.Text))
            {
                MessageBox.Show("File not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myOpenFileDialog = new OpenFileDialog();
            if (myOpenFileDialog.ShowDialog() == true)
            {
                DesignData.DesignDataReference.ReadDataFromFile(myOpenFileDialog.FileName);
            }
        }

        private void BtnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog mySaveFileDialog = new SaveFileDialog();

            if (mySaveFileDialog.ShowDialog() == true)
            {
                ModbusTagManager.FileParser.SaveFile(DesignData.DesignDataReference.GetAllData, mySaveFileDialog.FileName);
            }
            MessageBox.Show("Tags saved.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}