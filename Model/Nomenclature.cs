using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MySql.Data.MySqlClient;
using System.Configuration;
using ClientBaseTesting.Templates;
using System.Collections.ObjectModel;
using System.Collections.Specialized;


namespace ClientBaseTesting.Model
{
    class Nomenclature : Table
    {
        private string _tablename = "Номенклатура";
        private Dictionary<string, string> _fields;

        private int _code;
        private string _name;
        private string _shortname;
        private int _codetypeedizm;
        private int _codekateg;
        private int _codecomment;
        private int _order;
        private string _description;

        new public event PropertyChangedEventHandler PropertyChanged;

        new public string TableName => _tablename;
        new public int Code => _code;
        new public string Name { get { return _name; } set { _name = value; OnPropertyChanged("Name"); } }
        new public Dictionary<string, string> Fields { get { return _fields; } set { _fields = value; } }
        public string ShortName { get { return _shortname; } set { _shortname = value; OnPropertyChanged("ShortName"); } }
        public int CodeTypeEdizm { get { return _codetypeedizm; } set { _codetypeedizm = value; OnPropertyChanged("CodeTypeEdizm"); } }
        public int CodeKateg { get { return _codekateg; } set { _codekateg = value; OnPropertyChanged("CodeKateg"); } }
        public int CodeComment { get { return _codecomment; } set { _codecomment = value; OnPropertyChanged("CodeComment"); } }
        public int Order { get { return _order; } set { _order = value; OnPropertyChanged("Order"); } }
        public string Description { get { return _description; } set { _description = value; OnPropertyChanged("Description"); } }


        public Nomenclature()
        {
            _fields = new Dictionary<string, string>
            {
               { "Code","Код"},
               { "Name","Наименование"},
               { "ShortName","Количество"},
               { "CodeTypeEdizm","Копия"},
               { "Order","Порядок"},
               { "CodeComment","КодПримечания"},
               { "CodeDescription","Описание"},
               { "CodeKateg","КодКатегория"}
            };
        }

        public Nomenclature(int code) : this() { _code = code; }
        public Nomenclature(int code, string name) : this(code) { _name = name; }

        public Nomenclature(int code, string name, string sname = null, int tcode = 0, int kcode = 0, int ccode = 0, int order = 0, string description = null) : this(code, name)
        {
            _shortname = sname;
            _codetypeedizm = tcode;
            _codekateg = kcode;
            _codecomment = ccode;
            _description = description;
            _order = order;
        }



        public static List<Nomenclature> Load(string str, PropertyChangedEventHandler propertyChange = null)
        {

            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {

                conn.Open();

                string sql = string.Format(str);

                MySqlCommand command = new MySqlCommand();

                command.Connection = conn;
                command.CommandText = sql;

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        List<Nomenclature> data = new List<Nomenclature>();

                        while (reader.Read())
                        {
                            int code = reader.GetInt32(reader.GetOrdinal("Код"));
                            string name = reader.IsDBNull(reader.GetOrdinal("Наименование")) ? null : reader.GetString(reader.GetOrdinal("Наименование"));
                            string krname = reader.IsDBNull(reader.GetOrdinal("Краткое наименование")) ? null : reader.GetString(reader.GetOrdinal("Краткое наименование"));
                            string description = reader.IsDBNull(reader.GetOrdinal("Описание")) ? null : reader.GetString(reader.GetOrdinal("Описание"));
                            int codetype = reader.IsDBNull(reader.GetOrdinal("КодТипЕдиницы")) ? 0 : reader.GetInt32(reader.GetOrdinal("КодТипЕдиницы"));
                            int codekat = reader.IsDBNull(reader.GetOrdinal("КодКатегория")) ? 0 : reader.GetInt32(reader.GetOrdinal("КодКатегория"));
                            int codecomment = reader.IsDBNull(reader.GetOrdinal("КодПримечания")) ? 0 : reader.GetInt32(reader.GetOrdinal("КодПримечания"));
                            int order = reader.IsDBNull(reader.GetOrdinal("Порядок")) ? 0 : reader.GetInt32(reader.GetOrdinal("Порядок"));

                            Nomenclature nom = new Nomenclature(code, name, krname, codetype, codekat, codecomment, order, description);
                            nom.PropertyChanged += propertyChange;
                            data.Add(nom);
                        }
                        return data;
                    }
                }
            }

            return null;

        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
