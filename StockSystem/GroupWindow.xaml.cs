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
using ModelLib;

namespace StockSystem
{
    /// <summary>
    /// Логика взаимодействия для GroupWindow.xaml
    /// </summary>
    public partial class GroupWindow : Window
    {
        public GroupWindow()
        {
            InitializeComponent();
            foreach (var date in Stock.GetUniqDates())
            {
                var btn = new Button();
                btn.Content = date.ToString();
                btn.Click += btnDate_Click;
                pnlInfo.Children.Add(btn);
            }
        }

        public event EventHandler ButtonClicked;

        private void btnDate_Click(object sender, RoutedEventArgs e)
        {
            DateOnly date = DateOnly.FromDateTime(DateTime.Parse(((Button)sender).Content.ToString()));
            Stock.SelectByDate(date);
            ButtonClicked?.Invoke(this, EventArgs.Empty);
            Close();
        }
    }
}
