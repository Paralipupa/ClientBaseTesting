using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientBaseTesting.Model
{
    class TypeEdizm : Table
    {
        private int _code;
        private string _name;
        private Dictionary<string, string> _fields;

        new public int Code => _code;
        new public string Name { get { return _name; } set { _name = value; OnPropertyChanged("Name"); } }
        new public Dictionary<string, string> Fields => _fields;

        public new event PropertyChangedEventHandler PropertyChanged;

        public TypeEdizm()
        {
            _fields = new Dictionary<string, string>
            {
                { "Name","Наименование"}
            };

        }
        public TypeEdizm(int code) : this()
        {
            _code = code;
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
