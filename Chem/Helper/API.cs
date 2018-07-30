﻿using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chem.Helper
{
    class API
    {
        public API()
        {
            port.WriteLine("/1N1R\r");
        }

        private Temp tmp = new Temp();

        private SerialPort port;
        public API(SerialPort port)
        {
            this.port = port;
        }
        //string SetZero()
        //{
        //    return "";
        //}
        public void SetZero()
        {
            //port.WriteLine("/1IA24000R\r");
            port.WriteLine("/1ZR\r");
        }
        public void SetSyring(string volume, string speed, string wait)
        {
            port.WriteLine("/1IV" + speed + "A" + volume + "R\r");

            if (String.IsNullOrEmpty(tmp.volume))
            {
                Console.WriteLine("null");
                int deley = (((int.Parse(volume) / 9600) * 1200 / int.Parse(speed)) * 1000) + 5000 + int.Parse(wait) * 1000;
                Console.WriteLine("delay : " + deley);
                Thread.Sleep(deley);

            } else
            {
                Console.WriteLine("null: " + tmp.volume);
                int deley = ((Math.Abs(((int.Parse(volume) / 9600) * 1200) - ((int.Parse(tmp.volume) / 9600) * 1200)) / int.Parse(speed)) * 1000) + 5000 + int.Parse(wait) * 1000;
                Console.WriteLine("delay : " + deley);
                Thread.Sleep(deley);
            }
            tmp.volume = volume;
           
             // <<< CHANGE TIMER

            //port.WriteLine("/2I5R");
            //port.WriteLine("/1OP6000R");
        }

        //public void Valve(string portn, string syring, string speed)
        //{
        //    port.WriteLine("/2I" + portn + "R/r");
        //    // delay 3000
        //    port.WriteLine("/1OV" + speed + "P" + syring + "R/r");
        //}

        private void Release(string volume, string speed, string portn)
        {
            //port.WriteLine("/2O"+ portn +"R\r");
            port.WriteLine("/1OV" + speed + "A" + volume + "R\r");
            // etc
        }

        public void ChangeValve_Release(string portn, string volume, string speed, string wait)
        {
            port.WriteLine("/2O" + portn + "R\r");
            Thread.Sleep(3000);
            Release(volume, speed, portn);
            int deley = (((int.Parse(volume) / 9600) * 1200 / int.Parse(speed)) * 1000) + 5000 + int.Parse(wait) * 1000;
            Thread.Sleep(deley);
        }

        // release l
    }
    class Temp
    {
        private String _volume;
        public String volume
        {
            get { return _volume; }
            set { _volume = value; }
        }
    }
}
