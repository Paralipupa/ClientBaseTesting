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
using ClientBaseTesting.Templates;
using ClientBaseTesting.Model;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ClientBaseTesting.View
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
            ViewPriceActiveOnChecked();
            
        }

        private void ViewPriceActiveOnChecked()
        {
            this.DataContext = null;
            OpenDbPricelist();
        }

        private void ViewTypeCostOnChecked()
        {
            double h = 1;
            bool ischecked = (bool)chkSelectTypeCost.IsChecked;
            if (ischecked)
            {
                if (dgCostService.ItemsSource == null) h = 0.1;
                else h = 1;
            }
            else h = 0;
            grdRowDefinCostService.Height = new GridLength(h, GridUnitType.Star);
            grdRowDefinCostSale.Height = new GridLength(1, GridUnitType.Star);
        }

        private void ViewGroupOnCheked()
        {
            double h = 1;
            bool ischecked = (bool)chkSelectGroup.IsChecked;
            if (ischecked)
            {
                if (dgPricelistGroup.ItemsSource == null) h = 0.1;
                else h = 2;
            }
            else h = 0;
            grdRowDefinGroup.Height = new GridLength(h, GridUnitType.Star);
            grdRowDefinProd.Height = new GridLength(1, GridUnitType.Star);
        }


        //список Прайслистов
        private void OpenDbPricelist()
        {

            if (this.DataContext == null)
            {
                Pricelist pl = new Pricelist();
                ObservableCollection<Pricelist> data = pl.Load((bool)chkSelectPriceActive.IsChecked);
                this.DataContext = data;

            }

            dgPricelist.ItemsSource = this.DataContext as ObservableCollection<Pricelist>;

            dgPricelist.SelectedItem = dgPricelist.Items[0];
            dgPricelist.ScrollIntoView(dgPricelist.Items[0]);


            //(dgPricelist.Columns[1] as DataGridTextColumn).Binding.StringFormat = "dd.mm.yyyy";
            //(dgPricelist.Columns[2] as DataGridTextColumn).Binding.StringFormat = "dd.mm.yyyy";
        }


        //список номенклатуры и групп  при выборе Прайслиста
        private void OpenDbPricelist_GroupAndProduct()
        {
            dgCostSale.ItemsSource = null;
            dgCostService.ItemsSource = null;

            Pricelist pl = dgPricelist.SelectedItem as Pricelist;
            if (pl != null)
            {
                if (pl.Product == null)
                {
                    PricelistNomenclature pd = new PricelistNomenclature();
                    pl.Product = pd.Load(pl.Code);
                    PricelistGroup gr = new PricelistGroup();
                    pl.Group = gr.Load(pl.Code);

                    dgPricelistNomenclature.ItemsSource = pl.Product;
                    dgPricelistGroup.ItemsSource = pl.Group;
                }
                dgPricelistNomenclature.ItemsSource = pl.Product;
                dgPricelistGroup.ItemsSource = pl.Group;
            }
            ViewGroupOnCheked();
        }

        //список номенклатуры при выборе группы
        private void OpenDbPricelistGroup_Product()
        {
            PricelistGroup gr = dgPricelistGroup.SelectedItem as PricelistGroup;
            if (gr != null)
            {
                PricelistNomenclature pd = new PricelistNomenclature();
                gr.Product = pd.Load(gr.CodePricelist, gr.Code);
                dgPricelistNomenclature.ItemsSource = gr.Product;
            }
        }

        //цены на продукцию при выборе номенклатуры из прайса
        private void OpenDbProduct_Cost()
        {
            PricelistNomenclature pd = dgPricelistNomenclature.SelectedItem as PricelistNomenclature;
            if (pd != null)
            {
                if (pd.CostSale == null) pd.CostSale = Cost.Load(pd.Code, 1);
                dgCostSale.ItemsSource = pd.CostSale;

                if (pd.CostService == null) pd.CostService = Cost.Load(pd.Code, 2);
                dgCostService.ItemsSource = pd.CostService;
            }
            ViewTypeCostOnChecked();
        }

        //*************************
        private void chkSelectGroup_Click(object sender, RoutedEventArgs e) { ViewGroupOnCheked(); }

        private void chkSelectPriceActive_Click(object sender, RoutedEventArgs e) { ViewTypeCostOnChecked(); }

        private void chkSelectTypeCost_Click(object sender, RoutedEventArgs e) { ViewTypeCostOnChecked(); }

        private void dgPricelistNomenclature_SelectionChanged(object sender, SelectionChangedEventArgs e) { OpenDbProduct_Cost(); }

        private void dgPricelist_SelectionChanged(object sender, SelectionChangedEventArgs e) { OpenDbPricelist_GroupAndProduct(); }

        private void dgPricelistGroup_SelectionChanged(object sender, SelectionChangedEventArgs e) { OpenDbPricelistGroup_Product(); }
        //*************************


        private void btnTest1_Click(object sender, RoutedEventArgs e)
        {
            PricelistNomenclature pricenom = dgPricelistNomenclature.SelectedItem as PricelistNomenclature;

            if (pricenom != null)
            {
                if (pricenom.CostSale != null && pricenom.CostSale.Count != 0)
                {
                    try
                    {
                        pricenom.CostSale.RemoveAt(0);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            }
        }

        private void btnPriceDel_Click(object sender, RoutedEventArgs e)
        {
            Pricelist d = dgPricelist.SelectedItem as Pricelist;
        }

        private void btnNomenclatureDel_Click(object sender, RoutedEventArgs e)
        {
            PricelistNomenclature d = dgPricelistNomenclature.SelectedItem as PricelistNomenclature;
            d?.CostSale?.RemoveAt(0);
        }

        private void btnCostDel_Click(object sender, RoutedEventArgs e)
        {
            Cost d = dgCostSale.SelectedItem as Cost;
        }
    }
}


