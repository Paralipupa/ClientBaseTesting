using MySql.Data.MySqlClient;
using System.Configuration;
using ClientBaseTesting.Templates;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;

namespace ClientBaseTesting.Model
{
    class PricelistNomenclature
    {
        public int Code { get; set; }
        public readonly string Name = null;
        public int CodePricelist { get; set; }
        public Nomenclature Nomenclature { get; set; }
        public ObservableCollection<Cost> CostSale;
        public ObservableCollection<Cost> CostService;

        public PricelistNomenclature()
        {
        }

        public PricelistNomenclature(int code)
        {
            Code= code;
        }

        public PricelistNomenclature(int code, int codepricelist) :this(code)
        {
            CodePricelist = codepricelist;
        }

        public PricelistNomenclature(int code, int codepricelist, Nomenclature nomenclature) : this(code, codepricelist)
        {
            Nomenclature = nomenclature;
        }

        public ObservableCollection<PricelistNomenclature> Load(string str)
        {
            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {

                conn.Open();

                MySqlCommand command = new MySqlCommand();

                command.Connection = conn;
                command.CommandText = str;

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        ObservableCollection<PricelistNomenclature> data = new ObservableCollection<PricelistNomenclature>();

                        while (reader.Read())
                        {
                            int code = reader.GetInt32(reader.GetOrdinal("Код"));
                            int codepricelist = reader.IsDBNull(reader.GetOrdinal("КодПрайслист")) ? 0 : reader.GetInt32(reader.GetOrdinal("КодПрайслист"));

                            int codenom = reader.IsDBNull(reader.GetOrdinal("КодНоменклатура")) ? 0 : reader.GetInt32(reader.GetOrdinal("КодНоменклатура"));
                            string namenomen = reader.IsDBNull(reader.GetOrdinal("Наименование")) ? null : reader.GetString(reader.GetOrdinal("Наименование")).Trim();

                            Nomenclature nomenclature = new Nomenclature(codenom, namenomen);

                            data.Add(new PricelistNomenclature(code, codepricelist, nomenclature));
                        }
                        data.CollectionChanged += Cost_CollectionChange;
                        return data;
                    }
                }
            }

            return null;

        }

        public ObservableCollection<PricelistNomenclature> Load(int codepricelist)
        {
            string sql = string.Format(SQLTemplates.SELECT_PRICELIST_NOMENCLATURE, codepricelist);
            return Load(sql);
        }

        public ObservableCollection<PricelistNomenclature> Load(int codepricelist, int codegroup)
        {
            string sql = string.Format(SQLTemplates.SELECT_GRUPPA_NOMENCLATURE, codepricelist, codegroup);
            return Load(sql);
        }

        private void Cost_CollectionChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add: // если добавление
                    MessageBox.Show("Добавление");
                    break;
                case NotifyCollectionChangedAction.Remove: // если удаление

                    MessageBox.Show($"удаление {sender.ToString()}");
                    break;
                case NotifyCollectionChangedAction.Replace: // если замена
                    MessageBox.Show("замена");
                    break;
            }
        }

    }
}
