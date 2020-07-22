using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Configuration;
using ClientBaseTesting.Templates;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ClientBaseTesting.Model
{
    class PricelistGroup : Table
    {
        private string _tablename = "Группапрайслистноменклатура";

        private int _code;
        private string _name;
        private Dictionary<string, string> _fields;
        private int _codepricelist;
        private int _codegroupe;
        private int _codeproduct;

        new public event PropertyChangedEventHandler PropertyChanged;

        public new int Code { get { return _code; } }
        public new string Name { get { return _name; } }
        public new Dictionary<string, string> Fields { get; }
        public int CodePricelist { get { return _codepricelist; }  }
        public int CodeGroupe { get { return _codegroupe; } set { _codegroupe = value; OnPropertyChanged("CodeGroupe"); } }
        public int CodeProduct { get { return _codeproduct; } set { _codeproduct = value; OnPropertyChanged("CodeProduct"); } }

        public ObservableCollection<GroupNomenclature> Product;

        public PricelistGroup()
        {
            _fields = new Dictionary<string, string>
            {
               { "Code","Код"},
               { "Name","Наименование"},
               { "CodeProduct","КодПрайслистноменклатура"},
               { "CodeGroupe","КодГруппаноменкларура"}
            };
        }
        public PricelistGroup(int code) { _code = code; }
        public PricelistGroup(int code, string name) : this(code) { _name = name; }
        public PricelistGroup(int code, string name, int codepricelist, int codegroupe) : this(code, name) { _codepricelist = codepricelist; _codegroupe = codegroupe; }

        public ObservableCollection<PricelistGroup> Load(int pricelist, PropertyChangedEventHandler propertyChange = null)
        {

            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {

                conn.Open();

                string sql = string.Format(SQLTemplates.SELECT_PRICELIST_GRUPPA, pricelist);

                MySqlCommand command = new MySqlCommand();

                command.Connection = conn;
                command.CommandText = sql;

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        ObservableCollection<PricelistGroup> data = new ObservableCollection<PricelistGroup>();

                        while (reader.Read())
                        {
                            int code = 0; //reader.GetInt32(reader.GetOrdinal("Код"));
                            string name = reader.IsDBNull(reader.GetOrdinal("Наименование")) ? null : reader.GetString(reader.GetOrdinal("Наименование"));
                            int codepricelist = reader.IsDBNull(reader.GetOrdinal("КодПрайслист")) ? 0 : reader.GetInt32(reader.GetOrdinal("КодПрайслист"));
                            int codegroupe = reader.IsDBNull(reader.GetOrdinal("КодГруппаНоменклатура")) ? 0 : reader.GetInt32(reader.GetOrdinal("КодГруппаНоменклатура"));
                            //int codeproduct = reader.IsDBNull(reader.GetOrdinal("КодГруппаноменклатура")) ? 0 : reader.GetInt32(reader.GetOrdinal("КодГруппаноменклатура"));

                            PricelistGroup pg = new PricelistGroup(code, name, codepricelist, codegroupe);
                            pg.PropertyChanged += propertyChange;
                            data.Add(pg);
                        }

                        return data;
                    }
                }
            }

            return null;

        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
