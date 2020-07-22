using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace ClientBaseTesting.Model
{
    internal class Table : INotifyPropertyChanged
    {
        private string _tablename;
        private int _code;
        private string _name;
        private Dictionary<string, string> _fields;

        public event PropertyChangedEventHandler PropertyChanged;

        public Table() { }

        public string TableName { get => _tablename; set => _tablename= value; }
        public int Code { get => _code; set => _code= value; }
        public string Name { get => _name; set => _name = value; }
        public Dictionary<string,string> Fields { get => _fields; set => _fields= value; }
    }
}
