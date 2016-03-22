using System.Collections.Generic;
using System.Windows;
using CandyStore.Repositories;
using CandyStore.Entities;
using System.Configuration;
using CandyStore.DesktopUI.Code;

namespace CandyStore.DesktopUI.Windows
{
    /// <summary>
    /// Interaction logic for CandyStoreWindow.xaml
    /// </summary>
    public partial class CandyStoreWindow : Window
    {
        ICandyRepository _candyRepository;
        ICustomerRepository _customerRepository;
        IPurchaseItemRepository _purchaseItemRepository;
        IPurchaseRepository _purchaseRepository;

        public CandyStoreWindow()
        {
            
            string connectionString = ConfigurationManager.ConnectionStrings["CandyStoreConnectionString"].ConnectionString;
            _candyRepository = new SqlCandyRepository(connectionString);
            _customerRepository = new SqlCustomerRepository(connectionString);
            _purchaseItemRepository = new SqlPurchaseItemRepository(connectionString);
            _purchaseRepository = new SqlPurchaseRepository(connectionString);

            InitializeComponent();

            loginTextBlock.Text = CurrentUser.Login;

            fillCandiesListBox();
            fillPurchaseListBox();
        }

        #region Candy mehods

        private void fillCandiesListBox()
        {
            IEnumerable<Candy> candies = _candyRepository.GetAvailableCandies();
            candyListBox.ItemsSource = candies;
        }


        private void addCandyButton_Click(object sender, RoutedEventArgs e)
        {
            AddCandyWindow addCandyWindow = new AddCandyWindow();
            if (addCandyWindow.ShowDialog() ?? false)
            {
                Candy candy = addCandyWindow.Candy;
                if (candy != null)
                {
                    _candyRepository.AddNewCandy(candy.Name, candy.Manufacturer, candy.Price, candy.SupplyQty);
                    fillCandiesListBox();
                }
            }
        }

        private void deleteCandyButton_Click(object sender, RoutedEventArgs e)
        {
            Candy candy = (Candy)candyListBox.SelectedItem;
            if (candy == null)
            {
                MessageBox.Show("Please select an item to delete from the list.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Are you sure you want to delete candy {candy.Name}?",
                "Delete", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _candyRepository.DeleteCandy(candy.Id);
                fillCandiesListBox();
            }
        }

        private void promotionCandyButton_Click(object sender, RoutedEventArgs e)
        {
            Candy candy = (Candy)candyListBox.SelectedItem;
            if (candy == null)
            {
                MessageBox.Show("Please select an item to promote from the list.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            PromotionWindow promoWindow = new PromotionWindow();
            promoWindow.ShowDialog();
            if (promoWindow.Promotion == null)
            {
                return;
            }

            _candyRepository.ChangePromotion(candy.Id, promoWindow.Promotion.Value);
            fillCandiesListBox();
        }

        private void supplyCandyButton_Click(object sender, RoutedEventArgs e)
        {
            Candy candy = (Candy)candyListBox.SelectedItem;
            if (candy == null)
            {
                MessageBox.Show("Please select an item which supply mast be replanash.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            AddSupplyWindow supplyWindow = new AddSupplyWindow();
            supplyWindow.ShowDialog();
            if (supplyWindow.SupplyQty == null)
            {
                return;
            }

            _candyRepository.ReplenishCandySupply(candy.Id, supplyWindow.SupplyQty.Value);
            fillCandiesListBox();
        }
        #endregion

        #region Purchase methods

        private void fillPurchaseListBox()
        {
            IEnumerable<Purchase> purchases = _purchaseRepository.GetAllPurchases();
            purchaseListBox.ItemsSource = purchases;
        }

        private void addPurchaseButton_Click(object sender, RoutedEventArgs e)
        {
            AddPurchaseWindow addPurchaseWindow = new AddPurchaseWindow();
            if (addPurchaseWindow.ShowDialog() ?? false)
            {
                Purchase purchase = addPurchaseWindow.Purchase;
                bool customerIsNull = addPurchaseWindow.CustomerIsNull;
                if (purchase != null)
                {
                    Purchase addedPurchase = null;
                    if (customerIsNull)
                    {
                        addedPurchase = _purchaseRepository.AddPurchase(null, purchase.PurchaseTime, CurrentUser.Id);
                    }
                    else
                    {
                        addedPurchase = _purchaseRepository.AddPurchase(purchase.CustomerId, purchase.PurchaseTime, CurrentUser.Id);
                    }

                    foreach (var item in addPurchaseWindow.PurchaseItems)
                    {
                        try
                        {

                            if (customerIsNull)
                            {
                                _purchaseItemRepository.AddPurchaseItem(null, addedPurchase.Id, item.CandyId, item.Amount);
                            }
                            else
                            {
                                _purchaseItemRepository.AddPurchaseItem(purchase.CustomerId, addedPurchase.Id, item.CandyId, item.Amount);
                            }
                        }
                        catch
                        {
                            Candy candy = _candyRepository.GetCandy(item.CandyId);
                            MessageBox.Show($"We are out of {candy.Name}. So it won`t be included in purchase!", "Warning", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        }
                    }

                    if (!customerIsNull)
                    {
                        decimal newDiscount = _purchaseRepository.ConfirmPurchase(addedPurchase.Id, purchase.CustomerId);
                        if (newDiscount > 0)
                        {
                            MessageBox.Show($"Customer`s discount rate is {newDiscount.ToString("##.##")}%", "Discount rate", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        }
                    }
                    fillPurchaseListBox();
                }
            }
        }

        #endregion

    }
}
