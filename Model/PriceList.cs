﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using ClientBaseTesting.Templates;
using System.Configuration;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ClientBaseTesting.Model
{
    class PricelistCollection { }

    class Pricelist
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string DateBegin { get; set; }
        public string DateEnd { get; set; }
        public ObservableCollection<PricelistNomenclature> Product;
        public ObservableCollection<PricelistGroup> Group;

        public Pricelist() { }

        public Pricelist(int code)
        {
            Code = code;
        }

        public Pricelist(int code, string name, string d1, string d2) : this(code)
        {
            Name = name;
            try
            {
                DateBegin = d1;
                DateEnd = d2;
            }
            catch
            {
                DateBegin = null;
                DateEnd = null;
            }
        }

        public ObservableCollection<Pricelist> Load(bool isactive = false)
        {

            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {

                conn.Open();

                string sql = string.Format(SQLTemplates.SELECT_COMMON_WHERE_ORDER, "Прайслист", isactive ? "Активный" : "True", "`Дата начала` DESC");

                MySqlCommand command = new MySqlCommand();

                command.Connection = conn;
                command.CommandText = sql;

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        ObservableCollection<Pricelist> data = new ObservableCollection<Pricelist>();

                        while (reader.Read())
                        {
                            int code = reader.GetInt32(reader.GetOrdinal("Код"));
                            string name = reader.IsDBNull(reader.GetOrdinal("Наименование")) ? null : reader.GetString(reader.GetOrdinal("Наименование"));
                            string d1 = reader.IsDBNull(reader.GetOrdinal("Дата начала")) ? null : reader.GetString(reader.GetOrdinal("Дата начала"));
                            string d2 = reader.IsDBNull(reader.GetOrdinal("Дата окончания")) ? null : reader.GetString(reader.GetOrdinal("Дата окончания"));

                            data.Add(new Pricelist(code, name, d1, d2));
                        }

                        return data;
                    }
                }
            }

            return null;

        }


    }

}