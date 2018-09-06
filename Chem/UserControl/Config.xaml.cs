using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace Chem.UserControl
{
    /// <summary>
    /// Interaction logic for Config.xaml
    /// </summary>
    public partial class Config : INotifyPropertyChanged
    {
        //public string Header { get; set; }
        public string TEXT { get; set; }
        //public bool IsOpen { get; set; }

        //public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(Config), new PropertyMetadata(null));

        public Config()
        {
            InitializeComponent();
            //this.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;


    }
}
