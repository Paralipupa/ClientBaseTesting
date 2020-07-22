using System;
using MySql.Data.MySqlClient;
using System.Configuration;
using ClientBaseTesting.Templates;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.ComponentModel;
using System.Collections.Generic;

namespace ClientBaseTesting.Model
{
    class GroupNomenclature : Table
    {
        private string _tablename = "группапрайслистноменклатура";
        private int _code;
        private string _name;
        private Dictionary<string, string> _fields;
        private int _codepricelist;
        private int _codeproduct;
        private int _codegroup;

        new public event PropertyChangedEventHandler PropertyChanged;

        new public string TableName => _tablename;
        new public int Code => _code;
        new public string Name { get { return _name; } set { _name = value; } }
        new public Dictionary<string, string> Fields => _fields;
        public int CodePricelist { get { return _codepricelist; } set { _codepricelist = value;  } }
        public int CodeProduct{ get { return _codeproduct; } set { _codeproduct= value; OnPropertyChanged("CodePrroduct"); } }
        public int CodeGroup { get { return _codegroup; } set { _codegroup = value; OnPropertyChanged("CodeGroup"); } }

        public Nomenclature Nomenclature { get; }
        public ObservableCollection<Cost> CostSale { get; set; }
        public ObservableCollection<Cost> CostService { get; set; }

        public GroupNomenclature()
        {
            _fields = new Dictionary<string, string>
            {
               { "Code","Код"},
               { "Name","Наименование"},
               { "CodeProduct","КодПрайсдист"},
               { "CodeGroup","КодГруппаноменклатура"}
            };

        }

        public GroupNomenclature(int code)
        {
            _code = code;
        }

        public GroupNomenclature(int code, int codepricelist, int codegroup, int codeproduct, Nomenclature nom) : this(code)
        {
            Nomenclature = nom;
            _codepricelist = codepricelist;
            _codegroup = codegroup;
            _codeproduct = codeproduct;
        }


        public ObservableCollection<GroupNomenclature> Load(string str, PropertyChangedEventHandler propertyChange = null)
        {
            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {

                conn.Open();

                MySqlCommand command = new MySqlCommand();

                command.Connection = conn;
                command.CommandText = str;

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        ObservableCollection<GroupNomenclature> data = new ObservableCollection<GroupNomenclature>();

                        while (reader.Read())
                        {
                            int code = reader.GetInt32(reader.GetOrdinal("Код"));
                            int codepricelist = reader.IsDBNull(reader.GetOrdinal("КодПрайслист")) ? 0 : reader.GetInt32(reader.GetOrdinal("КодПрайслист"));
                            int codegroup = reader.IsDBNull(reader.GetOrdinal("КодГруппаНоменклатура")) ? 0 : reader.GetInt32(reader.GetOrdinal("КодГруппаНоменклатура"));
                            int codeproduct = reader.IsDBNull(reader.GetOrdinal("КодПрайслистНоменклатура")) ? 0 : reader.GetInt32(reader.GetOrdinal("КодПрайслистНоменклатура"));

                            int codenom = reader.IsDBNull(reader.GetOrdinal("КодНоменклатура")) ? 0 : reader.GetInt32(reader.GetOrdinal("КодНоменклатура"));
                            string namenomen = reader.IsDBNull(reader.GetOrdinal("Наименование")) ? null : reader.GetString(reader.GetOrdinal("Наименование")).Trim();

                            Nomenclature nomenclature = new Nomenclature(codenom, namenomen);
                            GroupNomenclature grnm = new GroupNomenclature(code, codepricelist, codegroup, codeproduct, nomenclature);
                            grnm.PropertyChanged += propertyChange;

                            data.Add(grnm);
                        }
                        return data;
                    }
                }
            }

            return null;

        }

        public ObservableCollection<GroupNomenclature> Load(int codepricelist, int codegroup, PropertyChangedEventHandler propertyChange = null)
        {
            string sql = string.Format(SQLTemplates.SELECT_GRUPPA_NOMENCLATURE, codepricelist, codegroup);
            return Load(sql, propertyChange);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
