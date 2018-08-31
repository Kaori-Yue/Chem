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
            #endregion
            Console.WriteLine("Cons: SerialPortViewModel");

            Worker = new ObservableCollection<Model.Worker> {
                //new Model.Worker { Pump = 1, Value = "12", Volume = "as", Speed = "asass", Wait = "assaaa" }
            };

            //Worker = new List<Model.Worker>();
            try
            {
                port = new SerialPort("COM5", 9600, Parity.None, 8, StopBits.One);
                port.Open();
                port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
                api = new API(port);
            } catch (Exception e)
            {
                Console.WriteLine("Can't Connect SerialPort: " + e);
            }
        }

        private void SerialPortProgram(string args)
        {
            Console.WriteLine("Incoming Data:");
            if (!(port.IsOpen))
            {
                // Attach a method to be called when there
                // is data waiting in the port's buffer

                // Begin communications
                port.Open();
                port.Write("/1ZR\r");

                // Enter an application loop to keep this thread alive
                //Application.Run();
            }
            else
            {
                Console.WriteLine("port open: " + args);
                //port.Write("/1ZR\r");
                //MessageBox.Show(args);
                //port.Write("/1ZR\r");
                //port.WriteLine("/1ZIP" + args + "R\r");
                byte[] asciiBytes = Encoding.ASCII.GetBytes(args);
                string s2 = Encoding.ASCII.GetString(asciiBytes);
                Console.WriteLine("port open2: " + s2);
                
                port.Write(s2);
            }

        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
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

        private string _Wait;
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
            Worker.Add(new Model.Worker { Pump = Pump, Value = Value, Volume = Volume, Speed = Speed, Wait = Wait });
            Pump = String.Empty;
            Value = String.Empty;
            Volume = String.Empty;
            Speed = String.Empty;
            Wait = String.Empty;
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
            Thread.Sleep(2000);
            for (int i = 0; i < cycle; i++)
            {
                for (int j = 0; j < Worker.Count; j++)
                {
                    Model.Worker worker = Worker[j];
                    string oldVolume = "null";
                    if (j == 0)
                    {
                        if (worker.Pump == "L")
                        {
                            Console.WriteLine(string.Concat((int)(float.Parse(worker.Volume) * 9600)));
                            api.SetSyring(string.Concat((int)(float.Parse(worker.Volume) * 9600)), worker.Speed, worker.Wait, true , "null");
                        }
                        else
                        {
                            api.ChangeValve_Release(worker.Value, string.Concat((int)(float.Parse(worker.Volume) * 9600)), worker.Speed, worker.Wait);
                            //api.SetSyring(string.Concat((int)(float.Parse(worker.Volume) * 9600)), worker.Speed);
                        }
                    } else
                    {
                        if (worker.Pump == "L")
                        {
                            Console.WriteLine(string.Concat((int)(float.Parse(worker.Volume) * 9600)));
                            api.SetSyring(string.Concat((int)(float.Parse(worker.Volume) * 9600)), worker.Speed, worker.Wait, false, oldVolume);
                        }
                        else
                        {
                            api.ChangeValve_Release(worker.Value, string.Concat((int)(float.Parse(worker.Volume) * 9600)), worker.Speed, worker.Wait);
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
                Filter = "JSON Files (*.csv)|*.JSON|All Files (*.*)|*.*",
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
                /*
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    writer.Formatting = Formatting.Indented;
                    writer.WriteStartObject();
                    writer.WritePropertyName("data");
                    writer.WriteStartArray();

                    //Worker.ToList().foreach (var item in collection)
                    //{

                    //}
                    Worker.ToList().ForEach(worker =>
                    {
                        writer.WriteStartObject();

                        writer.WritePropertyName("Pump");
                        writer.WriteValue(worker.Pump);

                        writer.WritePropertyName("Value");
                        writer.WriteValue(worker.Value);

                        writer.WritePropertyName("Volume");
                        writer.WriteValue(worker.Volume);

                        writer.WritePropertyName("Speed");
                        writer.WriteValue(worker.Speed);

                        writer.WritePropertyName("Wait");
                        writer.WriteValue(worker.Wait);

                        writer.WriteEndObject();
                    });

                    writer.WriteEndArray();

                    writer.WritePropertyName("Loop");
                    writer.WriteValue(Cycle);

                    writer.WriteEndObject();

                }
                //Console.WriteLine(sb.ToString());
                System.IO.File.WriteAllText(save.FileName, sb.ToString());
                */
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
                Filter = "JSON Files (*.JSON)|*.JSON|All Files (*.*)|*.*",
                InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory
            };
            if (open.ShowDialog() == true)
            {
                Model.JsonFormat jsonFormat = JsonConvert.DeserializeObject<Model.JsonFormat>(System.IO.File.ReadAllText(open.FileName));
                //Console.WriteLine(jsonFormat.Loop);
                Worker.AddRange(jsonFormat.Worker);
                Cycle = jsonFormat.Loop;
                /*
                try
                {
                    using (StreamReader reader = new StreamReader(open.FileName))
                    {
                        ObservableCollection<Model.Worker> _worker = new ObservableCollection<Model.Worker>();
                        while (!reader.EndOfStream)
                        {
                            string[] property = reader.ReadLine().Split(',');
                            _worker.Add(new Model.Worker { Pump = property[0], Value = property[1], Volume = property[2], Speed = property[3], Wait = property[4] });
                            if (String.IsNullOrEmpty(property[5]))
                                Console.WriteLine("loop: " + property[5]);
                            //Worker.Add(new Model.Worker { Pump = property[0], Value = property[1], Volume = property[2], Speed = property[3], Wait = property[4] });
                        }
                        Worker.Clear();
                        Worker.AddRange(_worker);
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    Console.WriteLine("IndexOutOfRangeException");
                } 
                catch (Exception e)
                {
                    Console.WriteLine("Read File Fail: " + e);
                }
                */
            }
        }
        #endregion
        #endregion

    }
}
