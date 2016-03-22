using System.Windows;
using CandyStore.Entities;
using System.Collections.Generic;
using System;
using System.Configuration;
using CandyStore.Repositories;

namespace CandyStore.DesktopUI.Windows
{
    /// <summary>
    /// Interaction logic for AddPurchaseWindow.xaml
    /// </summary>
    public partial class AddPurchaseWindow : Window
    {
        ICandyRepository _candyRepository;

        public AddPurchaseWindow()
        {
            InitializeComponent();
            PurchaseItems = new List<PurchaseItem>();
            string connectionString = ConfigurationManager.ConnectionStrings["CandyStoreConnectionString"].ConnectionString;
            _candyRepository = new SqlCandyRepository(connectionString);
        }

        public Purchase Purchase { get; private set; }
        public bool CustomerIsNull { get; private set; }
        public List<PurchaseItem> PurchaseItems { get; private set; }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            Purchase = new Purchase();
            Purchase.PurchaseTime = DateTime.Now;

            int customerId;
            if (int.TryParse(customerIdTextBox.Text, out customerId) && customerId > 0)
            {
                Purchase.CustomerId = customerId;
                CustomerIsNull = false;
            }
            else
            {
                MessageBoxResult messageRes = MessageBox.Show("Customer Id will be null",
                                                "Warning", MessageBoxButton.OKCancel,
                                                MessageBoxImage.Warning);
                if (messageRes == MessageBoxResult.Cancel)
                {
                    return;
                }
                CustomerIsNull = true;
                //return;
            }

            DialogResult = true;
            Close();
        }

        private void addPurchaseItemButton_Click(object sender, RoutedEventArgs e)
        {
            PurchaseItem purchaseItem = new PurchaseItem();
            int candyId; 
            if (int.TryParse(candyIdTextBox.Text, out candyId) && candyId > 0)
            {
                List<int> allAvailableCandyId = new List<int>();
                foreach(var item in _candyRepository.GetAvailableCandies())
                {
                    allAvailableCandyId.Add(item.Id);
                }
                if (allAvailableCandyId.Contains(candyId))
                {
                    purchaseItem.CandyId = candyId;
                }
                else
                {
                    showErrorMessageBox("Invalid candy ID!");
                    purchaseItem = null;
                    return;
                }
            }
            else
            {
                showErrorMessageBox("Invalid candy ID!");
                purchaseItem = null;
                return;
            }

            decimal amount;
            if (decimal.TryParse(amountTextBox.Text, out amount) && amount > 0)
            {
                if (_candyRepository.CandyIsAvailable(candyId, amount))
                {
                    purchaseItem.Amount = amount;
                }
                else
                {
                    showErrorMessageBox("There are not enough candies in store!");
                    purchaseItem = null;
                    return;
                }
            }
            else
            {
                showErrorMessageBox("Invalid amount!");
                purchaseItem = null;
                return;
            }

            PurchaseItems.Add(purchaseItem);

            Candy candy = _candyRepository.GetCandy(candyId);
            string listItem = $"{candyId, 4} {candy.Name, 18} {amount, 18} ";
            purchaseItemListBox.Items.Add(listItem);
        }

        private void showErrorMessageBox(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
