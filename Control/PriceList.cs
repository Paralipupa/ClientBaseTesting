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
    class PriceList
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string DateBegin { get; set; }
        public string DateEnd { get; set; }

        public PriceList(int code)
        {
            Code = code;
        }

        public PriceList(int code, string name, string d1, string d2)
        {
            this.Code = code;
            this.Name = name;
            try
            {
                this.DateBegin = d1;
                this.DateEnd = d2;
            }
            catch
            {
                this.DateBegin = null;
                this.DateEnd = null;
            }
        }

        public static List<PriceList> Load(string str)
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
                        List<PriceList> data = new List<PriceList>();

                        while (reader.Read())
                        {
                            int code = reader.GetInt32(reader.GetOrdinal("Код"));
                            string name = reader.IsDBNull(reader.GetOrdinal("Наименование")) ? null : reader.GetString(reader.GetOrdinal("Наименование"));
                            string d1 = reader.IsDBNull(reader.GetOrdinal("Дата начала")) ? null : reader.GetString(reader.GetOrdinal("Дата начала"));
                            string d2 = reader.IsDBNull(reader.GetOrdinal("Дата окончания")) ? null : reader.GetString(reader.GetOrdinal("Дата окончания"));

                            data.Add(new PriceList(code, name, d1, d2));
                        }
                        return data;
                    }
                }
            }

            return null;            

        }
    }
}
