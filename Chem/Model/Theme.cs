using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Chem.Model
{
    public class Theme : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        /*
        // Combobox Accent
        
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
        });
        */
        

        /*
        private List<string> _ComboboxAccent = new List<string>();
        public List<string> ComboboxAccent
        {
            get { return _ComboboxAccent; }
            //set { if (_ComboboxAccent != value) _ComboboxAccent = value; OnPropertyChanged(); }
        }
        */



        string _comboboxAccentSelected;
        public string ComboboxAccentSelected
        {
            get { return _comboboxAccentSelected; }
            set { _comboboxAccentSelected = value; Console.WriteLine(ComboboxAccentSelected); OnPropertyChanged(); }
        }
    }
}
