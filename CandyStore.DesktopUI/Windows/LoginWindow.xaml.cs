using System.Windows;
using CandyStore.Entities;
using CandyStore.Repositories;
using System.Configuration;
using CandyStore.DesktopUI.Code;
using System.Threading;

namespace CandyStore.DesktopUI.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly IUserRepository _userRepository;
        public LoginWindow()
        {
            InitializeComponent();
            _userRepository = new SqlUserRepository(ConfigurationManager.ConnectionStrings["CandyStoreConnectionString"].ConnectionString);
        }

        private void loginCancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void loginOkButton_Click(object sender, RoutedEventArgs e)
        {
            // Must be deleted after testing *** //
            loginTextBox.Text = "candy_seller";
            passwordTextBox.Password = "1111";
            // ********************************* //
            string login = loginTextBox.Text;
            string password = passwordTextBox.Password;
            string encodedPassword = Encryptor.MD5Hash(password);
            User user = _userRepository.GetUserByLogin(login, encodedPassword);
            if (user == null)
            {
                MessageBox.Show(this, "Invalid user name or password", "Authentication Error", MessageBoxButton.OK);
            }
            else
            {
                CurrentUser.Initialize(user);
                new CandyStoreWindow().Show();
                Close();
            }
        }
    }
}
