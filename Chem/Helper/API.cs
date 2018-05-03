using System;
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
            port.WriteLine("/1IA3000R\r");
            //port.WriteLine("/1ZR\r");
        }
        public void SetSyring(string volume, string speed)
        {
            port.WriteLine("/1IV" + speed + "A" + volume + "R\r");
            Thread.Sleep(10000);

            //port.WriteLine("/2I5R");
            //port.WriteLine("/1OP6000R");
        }

        //public void Valve(string portn, string syring, string speed)
        //{
        //    port.WriteLine("/2I" + portn + "R/r");
        //    // delay 3000
        //    port.WriteLine("/1OV" + speed + "P" + syring + "R/r");
        //}

        private void Release(string volume, string speed)
        {
            port.WriteLine("/1OV" + speed + "A" + volume + "R\r");
            // etc
        }

        public void ChangeValve_Release(string portn, string volume, string speed)
        {
            port.WriteLine("/2I" + portn + "R\r");
            Thread.Sleep(3000);
            Release(volume, speed);
            Thread.Sleep(7000);
        }

        // release l
    }
}
