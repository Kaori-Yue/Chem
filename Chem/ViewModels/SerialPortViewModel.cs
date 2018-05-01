using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Chem.Helper;

namespace Chem.ViewModels
{
    public class SerialPortViewModel : INotifyPropertyChanged
    {
        private SerialPort port;
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
            RunCommand = new RelayCommand(Run);
            UpCommand = new RelayCommand(Up);
            DownCommand = new RelayCommand(Down);
            #endregion
            Console.WriteLine("Cons: SerialPortViewModel");

            Worker = new ObservableCollection<Model.Worker> {
                new Model.Worker { Pump = "Con", Value = "12", Volume = "as", Speed = "asass", Wait = "assaaa" }
            };

            //Worker = new List<Model.Worker>();

            port = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
            port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            try
            {
                port.Open();
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
                port.WriteLine(args.Trim().ToString());
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
        #endregion


        #region RelayCommand
        public RelayCommand SetZeroCommand { get; set; }
        public RelayCommand AddQueueCommand { get; set; }
        public RelayCommand RunCommand { get; set; }
        public RelayCommand UpCommand { get; set; }
        public RelayCommand DownCommand { get; set; }
        //public RelayCommand ChangeAccentCommand { get; set; }
        #endregion

        #region button
        #region SetZero
        private void SetZero()
        {

        }
        #endregion
        #region AddQueue
        //private void AddQueue(object parameter) => Console.WriteLine("Delay: " + Wait + " | Speed: " + Speed);
        private void AddQueue(object parameter)
        {
            Worker.Add(new Model.Worker { Pump = "Pump", Value = Value, Volume = Volume, Speed = Speed, Wait = Wait });
        }

        #endregion
        #region Run
        private void Run(object parameter)
        {
            int cycle = Int32.Parse(Cycle);
            Console.WriteLine(cycle);
            foreach(Model.Worker worker in Worker)
            {
                Console.WriteLine(worker.Speed);
            }
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
        #endregion

    }
}
