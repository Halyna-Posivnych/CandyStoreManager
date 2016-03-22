using System.Windows;
using CandyStore.Entities;
using System;

namespace CandyStore.DesktopUI.Windows
{
    /// <summary>
    /// Interaction logic for AddCandyWindow.xaml
    /// </summary>
    public partial class AddCandyWindow : Window
    {
        public AddCandyWindow()
        {
            InitializeComponent();
        }
        
        public Candy Candy { get; private set; }

        private void addCancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void addOkButton_Click(object sender, RoutedEventArgs e)
        {
            Candy = new Candy();

            Candy.Name = nameTextBox.Text;
            if (string.IsNullOrWhiteSpace(Candy.Name))
            {
                showErrorMessageBox("Invalid name!");
                Candy = null;
                return;
            }

            Candy.Manufacturer = manufactureTextBox.Text;
            if (string.IsNullOrWhiteSpace(Candy.Manufacturer))
            {
                showErrorMessageBox("Invalid manufacturer!");
                Candy = null;
                return;
            }

            decimal price;
            if (decimal.TryParse(priceTextBox.Text, out price) && price > 0)
            {
                Candy.Price = price;
            }
            else
            {
                showErrorMessageBox("Invalid price!");
                Candy = null;
                return;
            }

            decimal supply;
            if (decimal.TryParse(supplyQtyTextBox.Text, out supply) && supply >= 0)
            {
                Candy.SupplyQty = supply;
            }
            else
            {
                showErrorMessageBox("Invalid supply quantities!");
                Candy = null;
                return;
            }

            DialogResult = true;
            Close();
        }

        private void showErrorMessageBox(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
