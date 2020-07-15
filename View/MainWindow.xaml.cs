using System;
using System.Windows;
using System.Windows.Controls;
using ClientBaseTesting.View;


namespace ClientBaseTesting
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            // Добавляем обработчик для всех кнопок на гриде
        }
        private void Button_Click_Pricelist(object sender, RoutedEventArgs e)
        {
            PricelistView pricelist = new PricelistView();
            pricelist.ShowDialog();

        }
    }
}