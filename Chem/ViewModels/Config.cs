﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MahApps.Metro;
using Chem.Helper;
using Chem.Model;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Management;

namespace Chem.ViewModels
{
    public class Config : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        //public ObservableCollection<Model.Theme> ThemeConfig { get; set; }
        private Tuple<AppTheme, Accent> appStyle = ThemeManager.DetectAppStyle(System.Windows.Application.Current);
        public Config()
        {
            #region Bind Button Event
            #endregion
            Console.WriteLine("Cons: Config");
            //SaveCommand = new RelayCommand(Save);
            /*
            ThemeConfig = new ObservableCollection<Model.Theme>
            {
                //new Model.Theme { ComboboxAccent = { "Red", "Blue" }, ComboboxAccentSelected = "Red" }
                new Model.Theme {  }
            };
            */
        }

        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        //

        void Save(object parameter)
        {
            Console.WriteLine("Save");
            if (parameter == null) return;
            System.Windows.MessageBox.Show(parameter.ToString());
            //ChangeAccent(parameter.ToString());
        }

        //string _comboboxAccentSelected;
        private string _ComboboxAccentSelected;
        public string ComboboxAccentSelected
        {
            get { return _ComboboxAccentSelected ?? appStyle.Item2.Name; }
            set { _ComboboxAccentSelected = value; ChangeAccent(); OnPropertyChanged(); }
            //set { _comboboxAccentSelected = value; ChangeAccent(_comboboxAccentSelected); OnPropertyChanged(); }
        }
        private string _ComboboxBaseSelected;
        public string ComboboxBaseSelected
        {
            get => _ComboboxBaseSelected ?? appStyle.Item1.Name;
            set {_ComboboxBaseSelected = value; ChangeAccent(); OnPropertyChanged(); }
            //set { _comboboxAccentSelected = value; ChangeAccent(_comboboxAccentSelected); OnPropertyChanged(); }
        }

        private void ChangeAccent() => ThemeManager.ChangeAppStyle(System.Windows.Application.Current, ThemeManager.GetAccent(ComboboxAccentSelected), ThemeManager.GetAppTheme(ComboboxBaseSelected));

        #region ComboboxAccent
        public List<string> ComboboxAccent { get; } = new List<string>(new string[] {
            "Red",
            "Green",
            "Blue",
            "Purple",
            "Orange",
            "Lime",
            "Emerald",
            "Teal",
            "Cyan",
            "Cobalt",
            "Indigo",
            "Violet",
            "Pink",
            "Magenta",
            "Crimson",
            "Amber",
            "Yellow",
            "Brown",
            "Olive",
            "Steel",
            "Mauve",
            "Taupe",
            "Sienna"
        }.OrderBy(x => x));
        #endregion
        #region ComboboxBase
        public List<string> ComboboxBase { get; } = new List<string>(new string[] {
            "BaseLight",
            "BaseDark"
        });
        #endregion

        //////////////////////////////////////////////////////////////////////////////////
        // Serial Port



        private string _SerialPortSelected;
        public string SerialPortSelected
        {
            get
            {
                return _SerialPortSelected;
            }
            set
            {
                _SerialPortSelected = value;
                OnPropertyChanged();
            }
        }
        public List<KeyValuePair<string, string>> SerialPort { get => ListSerialPort(); }

        private List<KeyValuePair<string, string>> ListSerialPort()
        {
            // https://docs.microsoft.com/en-us/windows/desktop/cimwin32prov/win32-serialport
            ManagementClass processClass = new ManagementClass("Win32_SerialPort"); // Win32_PnPEntity


            var list = new List<KeyValuePair<string, string>>();

            ManagementObjectCollection Ports = processClass.GetInstances();
            foreach (ManagementObject property in Ports)
            {
                if (property.GetPropertyValue("Name") != null)
                {
                    //if (property.GetPropertyValue("Name").ToString().Contains("COM"))
                    //{
                    var DeviceID = property.GetPropertyValue("DeviceID").ToString();
                    var Name = property.GetPropertyValue("Name").ToString();
                    Console.WriteLine(String.Format("Name: {0} - {1}", Name, DeviceID));
                    list.Add(new KeyValuePair<string, string>(DeviceID, Name));
                    //}
                }

            }
            return list;
        }
    }
}
