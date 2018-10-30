using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Chem.Model
{
	public class Worker : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		// 1 = LEFT | 2 = RIGHT
		private string _Pump;
		public string Pump {
			get { return _Pump; }
			set { _Pump = value; OnPropertyChanged(); }
		}

		private string _Value;
		public string Value {
			get { return _Value; }
			set { _Value = value; OnPropertyChanged(); }
		}

		private string _Volume;
		public string Volume {
			get { return _Volume; }
			set { _Volume = value; OnPropertyChanged(); }
		}

		private string _Speed;
		public string Speed {
			get { return _Speed; }
			set { _Speed = value; OnPropertyChanged(); }
		}

		private string _Wait;
		public string Wait {
			get { return _Wait; }
			set { _Wait = value; OnPropertyChanged(); }
		}

	}
}
