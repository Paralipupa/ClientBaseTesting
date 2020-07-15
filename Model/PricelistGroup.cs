using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Configuration;
using ClientBaseTesting.Templates;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ClientBaseTesting.Model
{
    class PricelistGroup
    {

        public int Code { get; set; }
        public string Name { get; set; }
        public int CodePricelist { get; set; }
        public ObservableCollection<PricelistNomenclature> Product;


        public PricelistGroup() { }
        public PricelistGroup(int code) { Code = code; }
        public PricelistGroup(int code, string name) : this(code) { Name = name; }
        public PricelistGroup(int code, string name, int codepricelist) : this(code, name) { CodePricelist = codepricelist; }

        public ObservableCollection<PricelistGroup> Load(int pricelist)
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
                            int code = reader.GetInt32(reader.GetOrdinal("Код"));
                            string name = reader.IsDBNull(reader.GetOrdinal("Наименование")) ? null : reader.GetString(reader.GetOrdinal("Наименование"));
          
                            data.Add(new PricelistGroup(code,name,pricelist));
                        }

                        return data;
                    }
                }
            }

            return null;

        }

    }
}
