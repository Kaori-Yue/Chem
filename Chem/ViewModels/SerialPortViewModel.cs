using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Chem.Helper;
using Newtonsoft.Json;

namespace Chem.ViewModels
{
    public class SerialPortViewModel : INotifyPropertyChanged
    {
        private SerialPort port;
        API api;
        public ObservableCollection<Model.Worker> Worker { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SerialPortViewModel()
        {
            #region Bind Button Event
            AddQueueCommand = new RelayCommand(AddQueue);
            SetZeroCommand = new RelayCommand(SetZero);
            RunCommand = new RelayCommand(Run);
            UpCommand = new RelayCommand(Up);
            DownCommand = new RelayCommand(Down);
            SaveCommand = new RelayCommand(Save);
            LoadCommand = new RelayCommand(Load);
            ConnectCommand = new RelayCommand(Connect);
            #endregion
            Console.WriteLine("Cons: SerialPortViewModel");

            Worker = new ObservableCollection<Model.Worker> {
                //new Model.Worker { Pump = 1, Value = "12", Volume = "as", Speed = "asass", Wait = "assaaa" }
            };

            //Worker = new List<Model.Worker>();
            //SerialPort("");

        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {


            //int intBuffer;
            //intBuffer = port.BytesToRead;
            //byte[] byteBuffer = new byte[intBuffer];
            //port.Read(byteBuffer, 0, intBuffer); //<--- YOU ARE READING (AND DISCARDING) DATA HERE
            //Console.WriteLine(System.Text.Encoding.UTF8.GetString(byteBuffer));









            // Show all the incoming data in the port's buffer
            Console.WriteLine(port.ReadExisting());
        }

        #region property
        private string _Value;
        public string Value
        {
            get { return _Value; }
            set { _Value = value; OnPropertyChanged(); }
        }

        private string _Volume;
        public string Volume
        {
            get { return _Volume; }
            set { _Volume = value; OnPropertyChanged(); }
        }

        private string _Speed;
        public string Speed
        {
            get { return _Speed; }
            set { _Speed = value; OnPropertyChanged(); }
        }

        private string _Wait = "0";
        public string Wait
        {
            get { return _Wait; }
            set { _Wait = value; OnPropertyChanged(); }
        }

        private string _Cycle = "1";
        public string Cycle
        {
            get { return _Cycle; }
            set { _Cycle = value; OnPropertyChanged(); }
        }

        private string _Pump;
        public string Pump
        {
            get { return _Pump; }
            set { _Pump = value; OnPropertyChanged(); }
        }

        #endregion


        #region RelayCommand
        public RelayCommand SetZeroCommand { get; set; }
        public RelayCommand AddQueueCommand { get; set; }
        public RelayCommand RunCommand { get; set; }
        public RelayCommand UpCommand { get; set; }
        public RelayCommand DownCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand LoadCommand { get; set; }
        public RelayCommand ConnectCommand { get; set; }
        //public RelayCommand ChangeAccentCommand { get; set; }
        #endregion

        #region button
        #region SetZero
        private void SetZero(object parameter)
        {
            api.SetZero();
        }
        #endregion
        #region AddQueue
        //private void AddQueue(object parameter) => Console.WriteLine("Delay: " + Wait + " | Speed: " + Speed);
        private void AddQueue(object parameter)
        {
            string check = checkValue();
            if (!check.Equals("PASSED"))
            {
                MessageBox.Show(check);
                return;
            }

            Worker.Add(new Model.Worker { Pump = Pump, Value = Value, Volume = Volume, Speed = Speed, Wait = Wait });
            Pump = String.Empty;
            Value = String.Empty;
            Volume = String.Empty;
            Speed = String.Empty;
            Wait = String.Empty;
        }

        private string checkValue()
        {
            if (string.IsNullOrWhiteSpace(Pump))
                return "Pump";

            if (Pump == "R")
                if (string.IsNullOrWhiteSpace(Value))
                    return "if PUMP R  VALUE can't null";

            if (string.IsNullOrWhiteSpace(Volume))
                return "Volume";

            if (string.IsNullOrWhiteSpace(Speed))
                return "Speed";

            if (string.IsNullOrWhiteSpace(Wait))
                return "Wait";

            return "PASSED";
        }

        #endregion
        #region Run
        private void Run(object parameter)
        {
            Thread thread = new Thread(new ThreadStart(RunMe));
            thread.Start();
            //thread.
        }

        private void RunMe() {
            int cycle = Int32.Parse(Cycle);
            //int water = 0;
            Console.WriteLine(cycle);
            //Thread.Sleep(2000);
            for (int i = 0; i < cycle; i++)
            {
                string oldVolume = null; // default value
                for (int j = 0; j < Worker.Count; j++)
                {
                    Model.Worker worker = Worker[j];
                    if (j == 0)
                    {
                        if (worker.Pump == "L")
                        {
                            Console.WriteLine("delay VM" + string.Concat((int)(float.Parse(worker.Volume) * 9600)));
                            api.SetSyring(string.Concat((int)(float.Parse(worker.Volume) * 9600)), worker.Speed, worker.Wait);
                        }
                        else
                        {
                            api.ChangeValve_Release(worker.Value, string.Concat((int)(float.Parse(worker.Volume) * 9600)), worker.Speed, worker.Wait, oldVolume);
                            //api.SetSyring(string.Concat((int)(float.Parse(worker.Volume) * 9600)), worker.Speed);
                        }
                    } else
                    {
                        if (worker.Pump == "L")
                        {
                            Console.WriteLine(string.Concat((int)(float.Parse(worker.Volume) * 9600)));
                            api.SetSyring(string.Concat((int)(float.Parse(worker.Volume) * 9600)), worker.Speed, worker.Wait, oldVolume);
                        }
                        else
                        {
                            api.ChangeValve_Release(worker.Value, string.Concat((int)(float.Parse(worker.Volume) * 9600)), worker.Speed, worker.Wait, oldVolume);
                            //api.SetSyring(string.Concat((int)(float.Parse(worker.Volume) * 9600)), worker.Speed);
                        }
                    }

                    oldVolume = worker.Volume;
                }

            }
            //int cycle = Int32.Parse(Cycle);
            //int water = 0;
            //Console.WriteLine(cycle);
            //for (int i = 0; i < cycle; i++)
            //{
            //    Console.WriteLine("Cycle: " + (i + 1));
            //    int tmp_count = 1;

            //    foreach (Model.Worker worker in Worker)
            //    {
            //        Console.WriteLine("Worker: " + tmp_count++);
            //        Console.WriteLine("PUMP: " + worker.Pump);
            //        if (worker.Pump.Equals("L"))
            //        {
            //            api.SetSyring(worker.Volume, worker.Speed);
            //            water = Int32.Parse(worker.Volume);
            //        }
            //        else
            //        {
            //            // right
            //            api.ChangeValve_Release(worker.Value, worker.Volume, worker.Speed);

            //        }
            //        Thread.Sleep(100);
            //    }
            //}
        }

        #endregion
        #region Up
        private void Up(object parameter)
        {
            int index = Int32.Parse(parameter.ToString());
            if (index <= 0 || index >= Worker.Count)
                return;
            Worker.Move(index, index - 1);
            /*
            Model.Worker temp = Worker[index];
            Worker.RemoveAt(index);
            Worker.Insert(index - 1, temp);
            */
        }
        #endregion
        #region Down
        private void Down(object parameter)
        {
            int index = Int32.Parse(parameter.ToString());
            if (index < 0 || index >= Worker.Count - 1)
                return;
            Worker.Move(index, index + 1);
            /*
            Model.Worker temp = Worker[index];
            Worker.RemoveAt(index);
            Worker.Insert(index + 1, temp);
            */
        }
        #endregion
        #region Save
        private void Save(object parameter)
        {
            Microsoft.Win32.SaveFileDialog save = new Microsoft.Win32.SaveFileDialog
            {
                Title = "Save a file",
                DefaultExt = ".json",
                Filter = "JSON (*.json)|*.JSON|All Files (*.*)|*.*",    
            };
            if (save.ShowDialog() == true)
            {
                Model.JsonFormat jsonFormat = new Model.JsonFormat
                {
                    Worker = Worker,
                    Loop = Cycle
                };
                String json = JsonConvert.SerializeObject(jsonFormat, Formatting.Indented);
                System.IO.File.WriteAllText(save.FileName, json.ToString());
                Console.WriteLine(json);
            }
        }
        #endregion
        #region Load
        private void Load(object parameter)
        {

            /*
            using (var searcher = new ManagementObjectSearcher ("SELECT * FROM WIN32_SerialPort"))
            {
                string[] portnames = SerialPort.GetPortNames();
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();
                var tList = (from n in portnames
                             join p in ports on n equals p["DeviceID"].ToString()
                             select n + " - " + p["Caption"]).ToList();

                tList.ForEach(Console.WriteLine);
            }


            */

            /*
            // https://docs.microsoft.com/en-us/windows/desktop/cimwin32prov/win32-serialport
            ManagementClass processClass = new ManagementClass("Win32_SerialPort"); // Win32_PnPEntity


            ManagementObjectCollection Ports = processClass.GetInstances();
            string device = "No recognized";
            foreach (ManagementObject property in Ports)
            {
                if (property.GetPropertyValue("Name") != null)
                    //if (property.GetPropertyValue("Name").ToString().Contains("COM"))
                    //{
                    Console.WriteLine(property.GetPropertyValue("DeviceID"));
                Console.WriteLine(property.GetPropertyValue("Name").ToString());
                device = property.GetPropertyValue("Name").ToString();
                    //}
            }
            */




            //string[] ports = SerialPort.GetPortNames();
            //foreach (string port in ports)
            //{
            //    Console.WriteLine(port);
            //}

            //
            Microsoft.Win32.OpenFileDialog open = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Select a file",
                DefaultExt = ".json",
                Filter = "JSON (*.JSON)|*.JSON|All Files (*.*)|*.*",
                InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory
            };
            if (open.ShowDialog() == true)
            {
                Model.JsonFormat jsonFormat = JsonConvert.DeserializeObject<Model.JsonFormat>(System.IO.File.ReadAllText(open.FileName));
                //Console.WriteLine(jsonFormat.Loop);
                Worker.AddRange(jsonFormat.Worker);
                Cycle = jsonFormat.Loop;
            }
        }
        #endregion
        #region Connect
        private void Connect(object parameters)
        {
            Console.WriteLine(parameters);
            if (!string.IsNullOrWhiteSpace((string)parameters))
                SerialPort(parameters.ToString());
        }
        #endregion
        #endregion

        private string _SerialPortText;
        public string SerialPortText
        {
            get
            {
                return _SerialPortText;
            }
            set
            {
                _SerialPortText = value;
                OnPropertyChanged();
            }
        }

        private void SerialPort(string comPort)
        {
            if (string.IsNullOrEmpty(comPort))
                return;

            try
            {
                if (port.IsOpen)
                {
                    port.Close();
                    SerialPortText = "Unconnected";
                }

            }
            catch(NullReferenceException)
            {
                Console.WriteLine("null ref");
                SerialPortText = "Unconnected";
            }
            // http://www.sparxeng.com/blog/software/must-use-net-system-io-ports-serialport

            try
            {
                
                port = new SerialPort(comPort, 9600, Parity.None, 8, StopBits.One);
                port.WriteTimeout = 2000;
                //port.Encoding = Encoding.GetEncoding(1252);
                port.Open();
                port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
                Console.WriteLine("Connect SerialPort Success");
                api = new API(port);
                SerialPortText = "Connect Success";
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't Connect SerialPort: " + e);
                SerialPortText = "Unconnected";
            }
        }
    }
}
