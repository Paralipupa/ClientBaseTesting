using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Configuration;
using ClientBaseTesting.Templates;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ClientBaseTesting.Model
{
    class Cost
    {
        private readonly string _name = null;
        private int _code;
        private int _order;
        private int _count;
        private double _summa;

        private PricelistNomenclature _pricenomenclature;
        private TypeCost _typecost;
        private Discount _disc;

        public string Name => _name;
        public int Code { get { return _code; } set { _code = value; } }
        public double Summa { get { return _summa; } set { _summa = value; } }
        public int Order { get { return _order; } set { _order = value; } }
        public int Count { get { return _count; } set { _count = value; } }

        public PricelistNomenclature PriceNomen => _pricenomenclature;
        public TypeCost TypeCost => _typecost;
        public Discount Discount => _disc;


        public Cost(int code)
        {
            Code = code;
        }

        public Cost(int code, double summa, TypeCost typecost) : this(code)
        {
            Summa = summa;
            _typecost = typecost;
        }

        public Cost(int code, double summa, TypeCost typecost = null, PricelistNomenclature prnom = null, Discount disc = null, int count = 1, int order = 0) : this(code, summa, typecost)
        {
            Count = count;
            _order = order;
            _pricenomenclature = prnom;
            _typecost = typecost;
            _disc = disc;

        }

        public static ObservableCollection<Cost> Load(int codepricenomecl, int type)
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
                            int copy = reader.IsDBNull(reader.GetOrdinal("Копия")) ? 0 : reader.GetInt32(reader.GetOrdinal("Копия"));
                            double summa = reader.IsDBNull(reader.GetOrdinal("Сумма")) ? 0 : reader.GetDouble(reader.GetOrdinal("Сумма"));

                            int codetc = reader.IsDBNull(reader.GetOrdinal("КодТипЦены")) ? 0 : reader.GetInt32(reader.GetOrdinal("КодТипЦены"));
                            int codedisc = reader.IsDBNull(reader.GetOrdinal("КодСкидка")) ? 0 : reader.GetInt32(reader.GetOrdinal("КодСкидка"));
                            int codepn = reader.IsDBNull(reader.GetOrdinal("КодПрайслистНоменклатура")) ? 0 : reader.GetInt32(reader.GetOrdinal("КодПрайслистНоменклатура"));

                            data.Add(new Cost(code, summa, new TypeCost(codetc), new PricelistNomenclature(codepn), new Discount(codedisc), count, copy));
                        }

                        return data;
                    }
                }
            }

            return null;

        }
    }
}
