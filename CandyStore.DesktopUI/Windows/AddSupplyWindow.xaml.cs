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

namespace CandyStore.DesktopUI.Windows
{
    /// <summary>
    /// Interaction logic for AddSupplyWindow.xaml
    /// </summary>
    public partial class AddSupplyWindow : Window
    {
        public AddSupplyWindow()
        {
            InitializeComponent();
        }
        public decimal? SupplyQty { get; set; }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {

            decimal supply;
            if (decimal.TryParse(supplyTextBox.Text, out supply) && supply >= 0 && supply < 100)
            {
                SupplyQty = supply;
            }
            else
            {
                MessageBox.Show("Invalid supply quontity!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DialogResult = true;
            Close();
        }
    }
}
