using System.Windows;
using CandyStore.DesktopUI.Windows;

namespace CandyStore.DesktopUI.Windows
{
    /// <summary>
    /// Interaction logic for PromotionWindow.xaml
    /// </summary>
    public partial class PromotionWindow : Window
    {
        public PromotionWindow()
        {
            InitializeComponent();
        }

        public decimal? Promotion { get; set; }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            decimal promo;
            if (decimal.TryParse(promotionTextBox.Text, out promo) && promo >= 0 && promo < 100)
            {
                Promotion = promo;
            }
            else
            {
                MessageBox.Show("Invalid promotion!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DialogResult = true;
            Close();
        }
    }
}
