using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MySql.Data.MySqlClient;
using System.Configuration;
using ClientBaseTesting.Templates;

namespace ClientBaseTesting.Model
{
    class Cost : Table
    {
        private readonly string _tablename = "Цена";

        private Dictionary<string, string> _fields;
        private readonly string _name = null;
        private int _code;

        private int _order;
        private int _count;
        private double _summa;
        private int _codeproduct;
        private int _codediscount;
        private int _codetypecost;

       new public event PropertyChangedEventHandler PropertyChanged;

        public Cost() 
        {
            _fields = new Dictionary<string, string>
            {
                { "Code","Код"},
                { "Name","Наименование"},
                { "Summa","Сумма"},
                { "Count","Количество"},
                { "Order","Копия"},
                { "CodeProduct","КодПрайслистНоменклатура"},
                { "CodeDiscount","КодТипЦены"},
                { "CodeTypeCost","КодСкидка"},
            };
        }

        public Cost(int code, int codeproduct, int codetypecost, double summa = 0) : this()
        {
            _code = code;
            _codeproduct = codeproduct;
            _codetypecost = codetypecost;
            _summa = summa;
        }

        public Cost(int code, int codeproduct, int codetypecost, double summa = 0, int count = 1, int order = 0, int disc = 0) : this(code, codeproduct, codetypecost, summa)
        {
            _count = count;
            _order = order;
            _codediscount = disc;
        }

        new public string TableName => _tablename;
        new public int Code => _code;
        new public string Name => _name;
        new public Dictionary<string, string> Fields { get => _fields; set => _fields = value; }

        public double Summa
        {
            get { return _summa; }
            set { _summa = value; OnPropertyChanged("Summa"); }
        }

        public int Order
        {
            get { return _order; }
            set { _order = value; OnPropertyChanged("Order"); }
        }

        public int Count
        {
            get { return _count; }
            set { _count = value; OnPropertyChanged("Count"); }
        }

        public int CodeDiscount
        {
            get { return _codediscount; }
            set { _codediscount = value; OnPropertyChanged("CodeDiscount"); }
        }

        public int CodeTypeCost
        {
            get { return _codetypecost; }
            set { _codetypecost= value; OnPropertyChanged("CodeTypeCost"); }
        }
         
        public ObservableCollection<Cost> Load(int codepricenomecl, int type, PropertyChangedEventHandler propertyChange = null)
        {

            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sql = string.Format(SQLTemplates.SELECT_NOMENCLATURE_COST, codepricenomecl, type);

                MySqlCommand command = new MySqlCommand();

                command.Connection = conn;
                command.CommandText = sql;

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        ObservableCollection<Cost> data = new ObservableCollection<Cost>();

                        while (reader.Read())
                        {
                            int code = reader.GetInt32(reader.GetOrdinal("Код"));
                            string name = reader.IsDBNull(reader.GetOrdinal("Наименование")) ? null : reader.GetString(reader.GetOrdinal("Наименование"));
                            int count = reader.IsDBNull(reader.GetOrdinal("Количество")) ? 0 : reader.GetInt32(reader.GetOrdinal("Количество"));
                            int order = reader.IsDBNull(reader.GetOrdinal("Копия")) ? 0 : reader.GetInt32(reader.GetOrdinal("Копия"));
                            double summa = reader.IsDBNull(reader.GetOrdinal("Сумма")) ? 0 : reader.GetDouble(reader.GetOrdinal("Сумма"));

                            int codetype = reader.IsDBNull(reader.GetOrdinal("КодТипЦены")) ? 0 : reader.GetInt32(reader.GetOrdinal("КодТипЦены"));
                            int codedisc = reader.IsDBNull(reader.GetOrdinal("КодСкидка")) ? 0 : reader.GetInt32(reader.GetOrdinal("КодСкидка"));
                            int codeproduct = reader.IsDBNull(reader.GetOrdinal("КодПрайслистНоменклатура")) ? 0 : reader.GetInt32(reader.GetOrdinal("КодПрайслистНоменклатура"));

                            Cost cost = new Cost(code, codeproduct, codetype, summa, count, order, codedisc);
                            cost.PropertyChanged += propertyChange;
                            data.Add(cost);

                        }

                        return data;
                    }
                }
            }

            return null;
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
