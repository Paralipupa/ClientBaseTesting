using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Configuration;
using MySql.Data;
using MySql.Data.MySqlClient;
using WpfApp1.Templates;
using WpfApp1.Control;

namespace WpfApp1.View
{
    /// <summary>
    /// Логика взаимодействия для Pricelist.xaml
    /// </summary>
    public partial class PricelistView : Window
    {

        public PricelistView()
        {
            
            InitializeComponent();
        }


        private void Button_Click_OpenDB(object sender, RoutedEventArgs e)
        {
            OpenDbPricelist();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            OpenDbPricelist();
            
        }

        private void OpenDbPricelist()
        {
            {

                string sql = string.Format(SQLTemplates.SELECT_COMMON_ORDER, "Прайслист","`Дата начала` DESC");
                List<PriceList> data = PriceList.Load(sql);
                dgPricelist.ItemsSource = data;


                (dgPricelist.Columns[1] as DataGridTextColumn).Binding.StringFormat = "dd.mm.yyyy";
                (dgPricelist.Columns[2] as DataGridTextColumn).Binding.StringFormat = "dd.mm.yyyy";
            }
        }

        private void OpenDbPriceProduct(int codePrice)
        {
            {

                dgCostSale.ItemsSource = null;
                dgCostService.ItemsSource = null;

                string sql = string.Format(SQLTemplates.SELECT_PRICELIST_PRODUCT, codePrice);
                List<PriceProduct> data = PriceProduct.Load(sql);
                dgPriceProduct.ItemsSource = data;
            }
        }

        private void OpenDbPriceProductCost(int codePrice, int codeProduct)
        {
            {

                string sql = string.Format(SQLTemplates.SELECT_PRICELIST_PRODUCT_COST, codePrice, codeProduct, 1);
                List<PriceProduct> data = PriceProduct.LoadCost(sql);
                dgCostSale.ItemsSource = data;

                sql = string.Format(SQLTemplates.SELECT_PRICELIST_PRODUCT_COST, codePrice, codeProduct, 2);
                data= PriceProduct.LoadCost(sql);
                dgCostService.ItemsSource = data;

            }
        }


        private void dgPricelist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PriceList pl = dgPricelist.SelectedItem as PriceList;
            if (pl != null)   OpenDbPriceProduct(pl.Code);
        }

        private void dgPriceProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PriceProduct pl = dgPriceProduct.SelectedItem as PriceProduct;
            if (pl != null) OpenDbPriceProductCost(pl.PriceList.Code,pl.Product.Code);

        }
    }

}
