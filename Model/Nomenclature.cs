using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace ClientBaseTesting.Model
{
    class Nomenclature
    {
        private int _code;
        private string _name;
        private string _shortname;
        private int _codetypeedizm;
        private string _comment;

        public int Code { get { return _code; } set { _code = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public string ShortName { get { return _shortname; } set { _shortname = value; } }
        public int CodeTypeEdizm { get { return _codetypeedizm; } set { _codetypeedizm = value; } }
        public string Comment { get { return _comment; } set { _comment = value; } }


        public Nomenclature() { }

        public Nomenclature(int code) { _code = code; }

        public Nomenclature(int code, string name, string sname = null, int tcode = 0, string comment = null) : this(code)
        {
            _name = name;
            _shortname = sname;
            _codetypeedizm = tcode;
            _comment = comment;
        }

        //public string Name()
        //{
        //    return _name;
        //}

        //public void Name(string name)
        //{
        //    _name = name;
        //}

        public static List<Nomenclature> Load(string str)
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
                            string comment = reader.IsDBNull(reader.GetOrdinal("Описание")) ? null : reader.GetString(reader.GetOrdinal("Описание"));
                            int codetype = reader.IsDBNull(reader.GetOrdinal("КодТипЕдиницы")) ? 0 : reader.GetInt32(reader.GetOrdinal("КодТипЕдиницы"));
                            int codekat = reader.IsDBNull(reader.GetOrdinal("КодКатегория")) ? 0 : reader.GetInt32(reader.GetOrdinal("КодКатегория"));
                            int codeprimkat = reader.IsDBNull(reader.GetOrdinal("КодПримечания")) ? 0 : reader.GetInt32(reader.GetOrdinal("КодПримечания"));

                            data.Add(new Nomenclature(code, name, krname, codetype, comment));
                        }
                        return data;
                    }
                }
            }

            return null;

        }

    }
}
