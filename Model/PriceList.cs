using MySql.Data.MySqlClient;
using ClientBaseTesting.Templates;
using System.Configuration;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;

namespace ClientBaseTesting.Model
{
    internal class Pricelist : Table
    {
        private string _tablename = "Прайслист";

        private Dictionary<string, string> _fields;
        private string _name = null;
        private int _code;
        private string _datebegin;
        private string _dateend;
        private int _codepartition;
        private int _order;
        private string _description;
        private bool _isactive;

        public new event PropertyChangedEventHandler PropertyChanged;

        public new string TableName => _tablename;
        public new int Code => _code;
        public new string Name { get { return _name; } set { _name = value; OnPropertyChanged("Name"); } }
        public string DateBegin { get { return _datebegin; } set { _datebegin = value; OnPropertyChanged("DateBegin"); } }
        public string DateEnd { get { return _dateend; } set { _dateend = value; OnPropertyChanged("DateEnd"); } }
        public int Order { get { return _order; } set { _order = value; OnPropertyChanged("Order"); } }
        public int CodePartition { get { return _codepartition; } set { _codepartition = value; OnPropertyChanged("CodePartition"); } }
        public string Description { get { return _description; } set { _description = value; OnPropertyChanged("Description"); } }
        public bool IsActive { get { return _isactive; } set { _isactive = value; OnPropertyChanged("IsActive"); } }

        public ObservableCollection<PricelistNomenclature> Product;
        public ObservableCollection<PricelistGroup> Group;

        public Pricelist()
        {
            _fields = new Dictionary<string, string>
            {
               { "Code","Код"},
               { "Name","Наименование"},
               { "DateBegin","Количество"},
               { "DateEnd","Копия"},
               { "Description","Описание"},
               { "Order","Порядок"},
               { "CodeComment","КодПримечания"},
               { "CodeKateg","КодКатегория"},
               { "CodePartition","КодРаздел"}
            };
        }

        public Pricelist(int code) : this()
        {
            _code = code;
        }

        public Pricelist(int code, string name) : this(code)
        {
            _name = name;
        }

        public Pricelist(int code, string name, string d1, string d2) : this(code, name)
        {

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

        public ObservableCollection<Pricelist> Load(bool isactive = false, PropertyChangedEventHandler propertyChange = null)
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
                            Pricelist pl = new Pricelist(code, name, d1, d2);
                            pl.PropertyChanged = propertyChange;
                            data.Add(pl);
                        }
                        //data.CollectionChanged += Data_CollectionChanged;

                        return data;
                    }
                }
            }

            return null;

        }

        //private void Data_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    MessageBox.Show($"{sender.ToString()} - {e.ToString()}");
        //    //throw new NotImplementedException();
        //}


        public void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}