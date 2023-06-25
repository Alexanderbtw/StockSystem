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
    /// Логика взаимодействия для PalletWindow.xaml
    /// </summary>
    public partial class PalletWindow : Window
    {
        private readonly List<Key> num_keys = new List<Key> { Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9, Key.D0 };

        public PalletWindow()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double xlen = double.Parse(tbXlen.Text);
                double ylen = double.Parse(tbYlen.Text);
                double zlen = double.Parse(tbZlen.Text);
                Stock.AddPallet(xlen, ylen, zlen);
            }
            catch
            {
                MessageBox.Show("Введены неверные данные", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
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

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
