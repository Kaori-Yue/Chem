using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chem.Helper
{
    static class Extension
    {
        public static bool isEvenNumber(this int a)
        {
            return a % 2 == 0 ? true : false;
        }

        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            items.ToList().ForEach(collection.Add);
        }
}
}
