using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using WpfApp1.Templates;
using System.Configuration;

namespace WpfApp1.Control
{
    class Product
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int CodeTypeEdizm { get; set; }
        public string Comment { get; set; }

        public Product(int code)
        {
            Code = code;
        }

        public Product(int code, string name, string sname = null, int tcode = 0, string comment = null)
        {
            Code = code;
            Name = name;
            ShortName = sname;
            CodeTypeEdizm = tcode;
            Comment = comment;
        }


        public static List<Product> Load(string str)
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
                        List<Product> data = new List<Product>();

                        while (reader.Read())
                        {
                            int code = reader.GetInt32(reader.GetOrdinal("Код"));
                            string name = reader.IsDBNull(reader.GetOrdinal("Наименование")) ? null : reader.GetString(reader.GetOrdinal("Наименование"));
                            string krname = reader.IsDBNull(reader.GetOrdinal("Краткое наименование")) ? null : reader.GetString(reader.GetOrdinal("Краткое наименование"));
                            string comment = reader.IsDBNull(reader.GetOrdinal("Описание")) ? null : reader.GetString(reader.GetOrdinal("Описание"));
                            int codeType = reader.GetInt32(reader.GetOrdinal("КодТипЕдиницы"));
                            int codeKat = reader.GetInt32(reader.GetOrdinal("КодКатегория"));
                            int codePrimKat = reader.GetInt32(reader.GetOrdinal("КодПримечания"));

                            data.Add(new Product(code, name, krname, codeType, comment  ));
                        }
                        return data;
                    }
                }
            }

            return null;

        }

    }
}
