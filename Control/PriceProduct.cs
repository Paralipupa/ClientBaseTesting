using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using WpfApp1.Templates;
using System.Configuration;
using System.CodeDom;

namespace WpfApp1.Control
{
    class PriceProduct
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public PriceList PriceList { get; set; }
        public Product Product { get; set; }
        public Cost Cost { get; set; }

        public PriceProduct(int _code)
        {
            Code = _code;
        }

            public PriceProduct(int _code, string _name, PriceList _price=null, Product _product=null, Cost _cost= null)
        {
            Code = _code;
            Name = _name;
            PriceList = _price;
            Product = _product;
            Cost = _cost;
        }

        public static List<PriceProduct> Load(string str)
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
                        List<PriceProduct> data = new List<PriceProduct>();

                        while (reader.Read())
                        {
                            //int code = reader.GetInt32(reader.GetOrdinal("Код"));
                            int codePL = reader.GetInt32(reader.GetOrdinal("КодПрайслист"));
                            PriceList pricelist = new PriceList(codePL);
                            int codeProd = reader.GetInt32(reader.GetOrdinal("КодНоменклатура"));
                            string nameProd = reader.IsDBNull(reader.GetOrdinal("Наименование")) ? null : reader.GetString(reader.GetOrdinal("Наименование"));

                            Product product = new Product(codeProd, nameProd);

                            data.Add(new PriceProduct(codeProd, nameProd, pricelist, product));
                        }
                        return data;
                    }
                }
            }

            return null;

        }

        public static List<PriceProduct> LoadCost(string str)
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
                        List<PriceProduct> data = new List<PriceProduct>();

                        while (reader.Read())
                        {
                            int codePL = reader.GetInt32(reader.GetOrdinal("КодПрайслист"));
                            PriceList pricelist = new PriceList(codePL);
                            int codeProd = reader.GetInt32(reader.GetOrdinal("КодНоменклатура"));
                            Product product = new Product(codeProd);
                            int codeCost = reader.GetInt32(reader.GetOrdinal("КодЦена"));
                            double summa = reader.IsDBNull(reader.GetOrdinal("Сумма")) ? 0 : reader.GetDouble(reader.GetOrdinal("Сумма")); 
                            byte order = (byte)(reader.IsDBNull(reader.GetOrdinal("Копия")) ? 0 : reader.GetByte(reader.GetOrdinal("Копия")));
                            byte count = (byte)(reader.IsDBNull(reader.GetOrdinal("Количество")) ? 1 : reader.GetByte(reader.GetOrdinal("Количество")));



                            int codeType = reader.GetInt32(reader.GetOrdinal("КодТипЦены"));
                            string nameType = reader.GetString(reader.GetOrdinal("Наименование"));
                            TypeCost typecost = new TypeCost(codeType, nameType);

                            Cost cost = new Cost(codeCost, summa,  typecost);

                            data.Add(new PriceProduct(codeProd, "",  pricelist, product,cost));
                        }
                        return data;
                    }
                }
            }

            return null;

        }


    }
}
