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
using System.ComponentModel;
using dlgForm = System.Windows.Forms;
using ClientBaseTesting.Properties;
using tt = ClientBaseTesting.Model;

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
            //Создаем компонент, регистрирующий изменения данных
            List<RegisterChange> reg = new List<RegisterChange>();
            this.DataContext = reg;

            ViewPriceActiveOnChecked();

        }

        private void ViewPriceActiveOnChecked()
        {
            grdPrice.DataContext = null;
            OpenDbPricelist();
        }

        private void ViewTypeCostOnChecked()
        {
            double h;
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
            double h;
            bool ischecked = (bool)chkSelectGroup.IsChecked;
            if (ischecked)
            {
                //if (dgPricelistGroup.ItemsSource == null) h = 0.1;
                //else h = 2;
                h = 1;
            }
            else h = 0;
            grdRowDefinGroup.Height = new GridLength(h, GridUnitType.Star);
            grdRowDefinProd.Height = new GridLength(1, GridUnitType.Star);
        }


        //список Прайслистов
        private void OpenDbPricelist()
        {

            if (grdPrice.DataContext == null)
            {
                Pricelist pl = new Pricelist();
                ObservableCollection<Pricelist> data = pl.Load((bool)chkSelectPriceActive.IsChecked);
                if (data != null) data.CollectionChanged += Data_CollectionChanged;
                grdPrice.DataContext = data;
            }

            dgPricelist.ItemsSource = grdPrice.DataContext as ObservableCollection<Pricelist>;
            if (dgPricelist.Items.Count != 0)
            {
                dgPricelist.SelectedItem = dgPricelist.Items[0];
                dgPricelist.ScrollIntoView(dgPricelist.Items[0]);
            }
        }


        //список номенклатуры и групп  при выборе Прайслиста
        private void OpenDbPricelist_GroupAndProduct()
        {
            dgCostSale.ItemsSource = null;
            dgCostService.ItemsSource = null;

            var pl = dgPricelist.SelectedItem as Pricelist;
            if (pl != null)
            {
                if (pl.Product == null)
                {
                    PricelistNomenclature pd = new PricelistNomenclature();
                    pl.Product = pd.Load(pl.Code);
                    if (pl.Product != null) pl.Product.CollectionChanged += Data_CollectionChanged;

                    PricelistGroup gr = new PricelistGroup();
                    pl.Group = gr.Load(pl.Code);
                    if (pl.Group != null) pl.Group.CollectionChanged += Data_CollectionChanged;
                    dgPricelistNomenclature.ItemsSource = pl.Product;
                    dgPricelistGroup.ItemsSource = pl.Group;
                }
                dgPricelistNomenclature.ItemsSource = pl.Product;
                dgPricelistGroup.ItemsSource = pl.Group;
                if (dgPricelistNomenclature.Items.Count != 0)
                {
                    dgPricelistNomenclature.SelectedItem = dgPricelistNomenclature.Items[0];
                    dgPricelistNomenclature.ScrollIntoView(dgPricelistNomenclature.Items[0]);
                }

                dgPricelistNomenclature.Visibility = Visibility.Visible;
                dgGroupNomenclature.Visibility = Visibility.Hidden;

                Mess("Прайслист " + pl.Code);
            }
            ViewGroupOnCheked();
        }

        //список номенклатуры при выборе группы
        private void OpenDbPricelistGroup_Product()
        {
            PricelistGroup gr = dgPricelistGroup.SelectedItem as PricelistGroup;
            if (gr != null)
            {
                GroupNomenclature gd = new GroupNomenclature();
                gr.Product = gd.Load(gr.CodePricelist, gr.CodeGroupe);
                if (gr.Product != null)
                {
                    gr.Product.CollectionChanged += Data_CollectionChanged;
                    dgGroupNomenclature.ItemsSource = gr.Product;

                    dgPricelistNomenclature.Visibility = Visibility.Hidden;
                    dgGroupNomenclature.Visibility = Visibility.Visible;

                    Mess("Группа " + gr.Code);
                }


            }
        }

        //цены на продукцию при выборе номенклатуры из прайса
        private void OpenDbProduct_Cost()
        {
            if (dgPricelistNomenclature.Visibility == Visibility.Visible)
            {
                var pd = dgPricelistNomenclature.SelectedItem as PricelistNomenclature;
                if (pd != null)
                {
                    if (pd.CostSale == null)
                    {
                        Cost cost = new Cost();
                        ObservableCollection<Cost> collcost = cost.Load(pd.Code, 1, MyOnPropertyChange);
                        if (collcost != null) collcost.CollectionChanged += Data_CollectionChanged;
                        pd.CostSale = collcost;
                    }
                    dgCostSale.ItemsSource = pd.CostSale as ObservableCollection<Cost>;


                    if (pd.CostService == null)
                    {
                        Cost cost = new Cost();
                        ObservableCollection<Cost> collcost = cost.Load(pd.Code, 2, MyOnPropertyChange);
                        if (collcost != null) collcost.CollectionChanged += Data_CollectionChanged;
                        pd.CostService = collcost;

                    }
                    dgCostService.ItemsSource = pd.CostService;
                    Mess("Продукт " + pd.Code);
                }
            }
            else if (dgGroupNomenclature.Visibility == Visibility.Visible)
            {
                var pd = dgGroupNomenclature.SelectedItem as GroupNomenclature;
                if (pd != null)
                {
                    if (pd.CostSale == null)
                    {
                        Cost cost = new Cost();
                        ObservableCollection<Cost> collcost = cost.Load(pd.CodeProduct, 1, MyOnPropertyChange);
                        if (collcost != null) collcost.CollectionChanged += Data_CollectionChanged;
                        pd.CostSale = collcost;
                    }
                    dgCostSale.ItemsSource = pd.CostSale as ObservableCollection<Cost>;


                    if (pd.CostService == null)
                    {
                        Cost cost = new Cost();
                        ObservableCollection<Cost> collcost = cost.Load(pd.CodeProduct, 2, MyOnPropertyChange);
                        if (collcost != null) collcost.CollectionChanged += Data_CollectionChanged;
                        pd.CostService = collcost;

                    }
                    dgCostService.ItemsSource = pd.CostService;
                    Mess("Продукт " + pd.Code);
                }

            }

            ViewTypeCostOnChecked();
        }

        private void OpenDbCost(object sender)
        {
            Cost cost = dgCostSale.SelectedItem as Cost;
            if (cost != null) { Mess("код покупки " + cost.Code.ToString()); }

            Cost cost2 = dgCostService.SelectedItem as Cost;
            if (cost2 != null) { Mess("код обслуживания " + cost2.Code.ToString()); }

        }

        //*************************
        private void chkSelectGroup_Click(object sender, RoutedEventArgs e) { ViewGroupOnCheked(); }

        private void chkSelectPriceActive_Click(object sender, RoutedEventArgs e) { ViewPriceActiveOnChecked(); }

        private void chkSelectTypeCost_Click(object sender, RoutedEventArgs e) { ViewTypeCostOnChecked(); }

        private void dgPricelistNomenclature_SelectionChanged(object sender, SelectionChangedEventArgs e) { OpenDbProduct_Cost(); }

        private void DgPricelist_SelectionChanged(object sender, SelectionChangedEventArgs e) { OpenDbPricelist_GroupAndProduct(); }

        private void dgPricelist_GotFocus(object sender, RoutedEventArgs e) { OpenDbPricelist_GroupAndProduct(); }

        private void DgPricelistGroup_SelectionChanged(object sender, SelectionChangedEventArgs e) { OpenDbPricelistGroup_Product(); }

        private void DgCostSale_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OpenDbCost(dgCostSale);
        }

        private void DgCostService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OpenDbCost(dgCostService);
        }

        private void winPricelist_Closed(object sender, EventArgs e)
        {
            List<RegisterChange> rg = this.DataContext as List<RegisterChange>;
            if ((rg != null) && (rg.Count != 0)) Save(true);
            Settings.Default.Save();

        }

        private void dgPricelist_UnloadingRow(object sender, DataGridRowEventArgs e) { }


        //*************************

        private void btnPriceDel_Click(object sender, RoutedEventArgs e)
        {
            var d = dgPricelist.ItemsSource as ObservableCollection<Pricelist>;
            if (d != null) d.Remove((Pricelist)dgPricelist.SelectedItem);
        }

        private void btnNomenclatureDel_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<PricelistNomenclature> d = dgPricelistNomenclature.ItemsSource as ObservableCollection<PricelistNomenclature>;
            if (d != null) d.Remove((PricelistNomenclature)dgPricelistNomenclature.SelectedItem);
        }

        private void btnCostDel_Click(object sender, RoutedEventArgs e)
        {
            var d = dgCostService.ItemsSource as ObservableCollection<Cost>;
            if (d != null)
            {
                d.Remove((Cost)dgCostService.SelectedItem);
            }
        }

        private void btnCostDel_Click_1(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Cost> d = dgCostSale.ItemsSource as ObservableCollection<Cost>;
            if (d != null)
            {
                d.Remove((Cost)dgCostSale.SelectedItem);
            }


        }
        //*************************

        private void Data_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RegisterChange reg = new RegisterChange(sender, null, e);
            (this.DataContext as List<RegisterChange>)?.Add(reg);

        }

        private void MyOnPropertyChange(object sender, PropertyChangedEventArgs e)
        {
            RegisterChange reg = new RegisterChange(sender, e, null);
            (this.DataContext as List<RegisterChange>)?.Add(reg);
        }

        private void Mess(string s, bool b = false)
        {
            if (b) lblMessage.Content += s;
            else lblMessage.Content = s;
        }


        private void Save(bool isWarning = false)
        {
            List<RegisterChange> rg = this.DataContext as List<RegisterChange>;
            if (rg != null)
            {
                if (rg.Count != 0)
                {
                    bool issave = true;
                    if (isWarning)
                    {
                        string message = $" зафиксированы изменения {rg.Count}  Сохранить ?";
                        string caption = "Внимание";
                        dlgForm.MessageBoxButtons buttons = dlgForm.MessageBoxButtons.YesNo;
                        dlgForm.DialogResult result = dlgForm.MessageBox.Show(message, caption, buttons);
                        if (result != dlgForm.DialogResult.Yes)
                        {
                            issave = false;
                        }
                    }
                    if (issave)
                        if (RegisterChange.Save(rg)) rg.Clear();
                }
            }

        }

        private void butButtonSave_Click(object sender, RoutedEventArgs e) { Save(); }




    }
}


