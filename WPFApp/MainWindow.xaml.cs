using Microsoft.Extensions.Configuration;
using Services.Implement;
using Services.Interface;
using System.Windows;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ISystemAccountSvc _accountSvc;
        private string adminEmail;
        private string adminPassword;

        public MainWindow()
        {
            _accountSvc = new SystemAccountSvc();
            InitializeComponent();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool LoginWithAdminAccount(string email, string password)
        {
            if (email == adminEmail && password == adminPassword)
            {
                return true;
            }
            return false;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (AllowLogin())
            {

                if (LoginWithAdminAccount(txtEmail.Text.Trim(), txtPassword.Password.Trim()))
                {
                    Hide();

                    FuNewsManagement fuNews = new FuNewsManagement
                    {
                        IsAdmin = true,
                    };
                    fuNews.Closed += (s, args) => Show(); // Show login form when management form is closed

                    fuNews.Show();
                }
                else
                {
                    var validate = _accountSvc.Validate(txtEmail.Text.Trim(), txtPassword.Password.Trim());
                    if (validate)
                    {
                        Hide();

                        FuNewsManagement fuNews = new FuNewsManagement
                        {
                            Account = _accountSvc.GetAccountByEmail(txtEmail.Text.Trim()),
                            IsAdmin = false
                        };
                        fuNews.Closed += (s, args) => Show(); // Show login form when management form is closed

                        fuNews.Show();

                    }
                    else
                    {
                        MessageBox.Show("You have no permision to this system!", "Login", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

            }

        }

        private bool AllowLogin()
        {
            if (txtEmail.Text.Trim() == "")
            {
                MessageBox.Show("Please enter your email!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (txtPassword.Password.Trim() == "")
            {
                MessageBox.Show("Please enter your password!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            IConfigurationSection section = config.GetSection("AdminAccount");

            adminEmail = section["Email"];
            adminPassword = section["Password"];

        }
    }
}