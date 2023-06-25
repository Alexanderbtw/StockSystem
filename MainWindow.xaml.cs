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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ModelLib;

namespace StockSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Task.Factory.StartNew(() => Stock.RandInit());

            this.Hide();
            Stock.LoadFromJson();
            this.Show();

            lvPallets.ItemsSource = Stock.Pallets;
            Pallet.Log += PrintLogs;
        }

        private void PrintLogs(object sender, string mesText) =>
            MessageBox.Show(mesText, sender.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);

        private void btnAddPallet_Click(object sender, RoutedEventArgs e) =>
            new PalletWindow().Show();

        private void btnAddBox_Click(object sender, RoutedEventArgs e)
        {
            if (lvPallets.SelectedItem != null)
                new BoxWindow(lvPallets.SelectedItem as Pallet).Show();
            else
                MessageBox.Show("Client not selected!");
            
        }

        private void btnDelBox_Click(object sender, RoutedEventArgs e)
        {
            var pallet = lvPallets.SelectedItem as Pallet;
            pallet.RemoveBox(lvBoxes.SelectedItem as Box);
        }

        private void btnDelPallet_Click(object sender, RoutedEventArgs e) =>
            Stock.Pallets.Remove(lvPallets.SelectedItem as Pallet);

        private void btnPalletInfo_Click(object sender, RoutedEventArgs e) =>
            new FullInfoWindow(lvPallets.SelectedItem).Show();

        private void btnBoxInfo_Click(object sender, RoutedEventArgs e) =>
            new FullInfoWindow(lvBoxes.SelectedItem).Show();


        private void lvPallets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvPallets.SelectedItem != null)
            {
                btnDelPallet.Visibility = Visibility.Visible;
                btnPalletInfo.Visibility = Visibility.Visible;
                btnAddBox.Visibility = Visibility.Visible;
            }
            else
            {
                btnDelPallet.Visibility = Visibility.Collapsed;
                btnPalletInfo.Visibility = Visibility.Collapsed;
                btnAddBox.Visibility = Visibility.Collapsed;
            }
        }

        private void lvBoxes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvBoxes.SelectedItem != null)
            {
                btnBoxInfo.Visibility = Visibility.Visible;
                btnDelBox.Visibility = Visibility.Visible;
            }
            else
            {
                btnBoxInfo.Visibility = Visibility.Collapsed;
                btnDelBox.Visibility = Visibility.Collapsed;
            }
        }





        //Stupid Sorting :(
        private void SortPallet(object sender, RoutedEventArgs e)
        {
            var btn = sender as GridViewColumnHeader;
            Stock.SortPallets(btn.Content.ToString(), false);
            lvPallets.ItemsSource = Stock.Pallets;
        }

        private void SortPalletReverse(object sender, MouseButtonEventArgs e)
        {
            var btn = sender as GridViewColumnHeader;
            Stock.SortPallets(btn.Content.ToString(), true);
            lvPallets.ItemsSource = Stock.Pallets;
        }

        private void SortBoxes(object sender, RoutedEventArgs e)
        {
            if (lvPallets.SelectedItem != null)
            {
                var p = lvPallets.SelectedItem as Pallet;
                var btn = sender as GridViewColumnHeader;
                p.SortBoxes(btn.Content.ToString(), false);
            }
        }

        private void SortBoxesReverse(object sender, MouseButtonEventArgs e)
        {
            if (lvPallets.SelectedItem != null)
            {
                var p = lvPallets.SelectedItem as Pallet;
                var btn = sender as GridViewColumnHeader;
                p.SortBoxes(btn.Content.ToString(), true);
            }
        }

        private void Closed_SaveData(object sender, EventArgs e)
        {
            this.Hide();
            Task.Factory.StartNew(() => Stock.SaveToJson()).Wait();
        }

        private void btnGroupPallet_Click(object sender, RoutedEventArgs e)
        {
            var gw = new GroupWindow();
            gw.ButtonClicked += PutItemSource;
            gw.Show();
        }

        private void PutItemSource(object sender, EventArgs e)
        {
            lvPallets.ItemsSource = Stock.PalletsByDate;
        }
    }
}
