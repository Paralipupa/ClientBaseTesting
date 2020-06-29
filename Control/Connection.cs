using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace WpfApp1.Control
{
    class Connection : IDisposable
    {
        public MySqlConnection connect;
        
        public Connection()
        {
            if ((connect = MyConnect()) == null)
            {
                this.Dispose();
            }
        }

        private MySqlConnection MyConnect()
        {
            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            try
            {
                connect = new MySqlConnection(connStr);
                connect.Open();
                return connect;
            }
            catch
            {
                connect.Dispose();
                return null;
            }
        }

        public void Dispose()
        {
            if (connect != null)
            {
                connect.Dispose();
            }
        }

    }
}
