using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chem.Model
{
    class JsonFormat
    {
        public ObservableCollection<Worker> Worker { get; set; }
        public String Loop { get; set; }
    }
}
