﻿using System;
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
    class PricelistNomenclature : Table
    {
        private string _tablename= "прайслист-номенклатура";
        private int _code;
        private string _name;
        private Dictionary<string, string> _fields;
        private int _codepricelist;
        private int _codenomenclature;

        new public event PropertyChangedEventHandler PropertyChanged;

        new public string TableName => _tablename;
        new public int Code => _code;
        new public string Name { get { return _name; } set { _name = value; } }
        new public Dictionary<string, string> Fields => _fields;
        public int CodePricelist { get { return _codepricelist; } set { _codepricelist = value; OnPropertyChanged("CodePricelist"); } }
        public int CodeNomenclature { get { return _codenomenclature; } set { _codenomenclature = value; OnPropertyChanged("CodeNomenclature"); } }


        public Nomenclature Nomenclature { get; set;  }
        public ObservableCollection<Cost> CostSale { get; set; }
        public ObservableCollection<Cost> CostService { get; set; }

        public PricelistNomenclature()
        {
            _fields = new Dictionary<string, string>
            {
               { "Code","Код"},
               { "Name","Наименование"},
               { "CodePricelist","КодПрайсдист"},
               { "CodeNomenclature","КодНоменклатура"}
            };

        }

        public PricelistNomenclature(int code)
        {
            _code = code;
        }

        public PricelistNomenclature(int code, int codepricelist) : this(code)
        {
            _codepricelist = codepricelist;
        }

        public PricelistNomenclature(int code, int codepricelist, int codenomenclature, Nomenclature nomenclature) : this(code, codepricelist)
        {
            _codenomenclature = codenomenclature;
            Nomenclature = nomenclature;
        }

        public ObservableCollection<PricelistNomenclature> Load(string str, PropertyChangedEventHandler propertyChange = null)
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
                        ObservableCollection<PricelistNomenclature> data = new ObservableCollection<PricelistNomenclature>();

                        while (reader.Read())
                        {
                            int code = reader.GetInt32(reader.GetOrdinal("Код"));
                            int codepricelist = reader.IsDBNull(reader.GetOrdinal("КодПрайслист")) ? 0 : reader.GetInt32(reader.GetOrdinal("КодПрайслист"));

                            int codenom = reader.IsDBNull(reader.GetOrdinal("КодНоменклатура")) ? 0 : reader.GetInt32(reader.GetOrdinal("КодНоменклатура"));
                            string namenomen = reader.IsDBNull(reader.GetOrdinal("Наименование")) ? null : reader.GetString(reader.GetOrdinal("Наименование")).Trim();

                            Nomenclature nomenclature = new Nomenclature(codenom, namenomen);
                            PricelistNomenclature plnm = new PricelistNomenclature(code, codepricelist, codenom, nomenclature);
                            plnm.PropertyChanged += propertyChange;

                            data.Add(plnm);
                        }
                        return data;
                    }
                }
            }

            return null;

        }

        public ObservableCollection<PricelistNomenclature> Load(int codepricelist, PropertyChangedEventHandler propertyChange = null)
        {
            string sql = string.Format(SQLTemplates.SELECT_PRICELIST_NOMENCLATURE, codepricelist);
            return Load(sql, propertyChange);
        }

        public ObservableCollection<PricelistNomenclature> Load(int codepricelist, int codegroup, PropertyChangedEventHandler propertyChange = null)
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
