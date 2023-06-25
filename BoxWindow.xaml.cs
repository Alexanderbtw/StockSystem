using ModelLib;
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

namespace StockSystem
{
    /// <summary>
    /// Логика взаимодействия для BoxWindow.xaml
    /// </summary>
    public partial class BoxWindow : Window
    {
        private readonly List<Key> num_keys = new List<Key> { Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9, Key.D0 };
        private static DateTime endDate = DateTime.Now;
        private static DateTime startDate = endDate.AddDays(-100);
        private Pallet pallet;

        public BoxWindow(Pallet tp)
        {
            pallet = tp;
            InitializeComponent();
            
            dpDate.DisplayDateStart = startDate;
            dpDate.DisplayDateEnd = endDate;
            tbXlen.Text = pallet.XLen.ToString();
            tbYlen.Text = pallet.YLen.ToString();
            tbZlen.Text = pallet.ZLen.ToString();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            double xlen;
            double ylen;
            double zlen;
            double weight;
            DateTime? d = dpDate.SelectedDate;
            if (d == null)
            {
                MessageBox.Show("Отметьте дату производства", "Warning");
                return;
            }

            try
            {
                xlen = double.Parse(tbXlen.Text);
                ylen = double.Parse(tbYlen.Text);
                zlen = double.Parse(tbZlen.Text);
                weight = double.Parse(tbWeight.Text);
            }
            catch
            {
                MessageBox.Show("Введены неверные данные", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Stock.AddBox(xlen, ylen, zlen, weight, DateOnly.FromDateTime(d.Value), ref pallet);
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void tb_KeyPressed(object sender, KeyEventArgs e)
        {
            if (num_keys.Contains(e.Key))
            {
                if (((TextBox)sender).Text.Contains(',') && ((TextBox)sender).Text.Split(',')[1].Count() >= 2)
                    e.Handled = true;
                else
                    return;
            }

            else if (e.Key == Key.OemComma && !((TextBox)sender).Text.Contains(','))
                return;

            else if (e.Key == Key.Back)
                return;

            e.Handled = true;
        }
    }
}
