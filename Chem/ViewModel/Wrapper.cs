using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chem.ViewModel
{
    public class Wrapper
    {
        public Theme Theme { get; set; }


        public Wrapper()
        {
            Theme = new Theme();
        }
    }
}
