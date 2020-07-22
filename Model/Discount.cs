using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientBaseTesting.Model
{
    class Discount : Table
    {
        private int _code;
        private string _name;
        private int _codetype;
        private double _count;
        private Dictionary<string, string> _fields;

        new public int Code => _code;
        new public string Name { get { return _name; } set { _name = value; OnPropertyChanged("Name"); } }
        new public Dictionary <string, string> Fields => _fields;
        public int CodeType { get { return _codetype; }  set { _codetype = value; OnPropertyChanged("CodeType"); } }
        public double Count { get { return _count; }  set { _count = value; OnPropertyChanged("Count"); } }


        new public event PropertyChangedEventHandler PropertyChanged;

        public Discount()
        {
            _fields = new Dictionary<string, string>
            {
                { "Name","Наименование"},
                { "CodeType","КодТипСкидки"},
                { "Count","Количество"}
            };

        }
        public Discount(int code) : this() { _code = code; }
        public Discount(int code, int codetype, int count) : this(code) { _code = code; _codetype = codetype; _count = count; }
        

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}
