using ModelLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для FullInfoWindow.xaml
    /// </summary>
    public partial class FullInfoWindow : Window
    {
        public FullInfoWindow(object obj)
        {
            InitializeComponent();
            var props = obj.GetType().GetProperties().Where(p => p.GetIndexParameters().Length == 0 && p.Name != "Boxes");
            foreach (var prop in props)
            {
                var tb = new TextBlock();
                tb.Text = $"{prop.Name}: {prop.GetValue(obj)}";
                pnlInfo.Children.Add(tb);
            }
        }
    }
}
