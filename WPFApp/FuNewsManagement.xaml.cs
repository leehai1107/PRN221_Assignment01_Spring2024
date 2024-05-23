using BusinessObjects;
using System.Windows;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for FuNewsManagement.xaml
    /// </summary>
    public partial class FuNewsManagement : Window
    {
        public SystemAccount Account { get; set; }
        public bool IsAdmin { get; set; }
        public FuNewsManagement()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e) => Close();

        private void btnNewsArticle_Click(object sender, RoutedEventArgs e)
        {
            Hide();

            NewsArticleManagement window = new();
            window.Closed += (s, args) => Show(); // Show login form when management form is closed

            window.Show();
        }

        private void btnAccount_Click(object sender, RoutedEventArgs e)
        {
            if (IsAdmin)
            {
                Hide();

                SystemAccountManagement window = new();
                window.Closed += (s, args) => Show(); // Show login form when management form is closed

                window.Show();
            }
            else
            {
                Hide();

                SystemAccountDetails window = new SystemAccountDetails
                {
                    systemAccountDetail = Account,
                    InsertOrUpdate = true,
                    EditPassword = true,
                };
                window.Closed += (s, args) => Show(); // Show login form when management form is closed

                window.Show();
            }
        }

        private void btnCategory_Click(object sender, RoutedEventArgs e)
        {
            Hide();

            CategoryManagement window = new();
            window.Closed += (s, args) => Show(); // Show login form when management form is closed

            window.Show();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsAdmin)
            {
                lbUsername.Content = Account.AccountName;
                btnReport.Visibility = Visibility.Collapsed;
            }
            else
            {
                lbUsername.Content = "Admin";
                btnHistory.Visibility = Visibility.Collapsed;
            }
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {

            Hide();

            NewsArticleManagement window = new NewsArticleManagement
            {
                IsHistory = true,
                AccountId = Account.AccountId
            };
            window.Closed += (s, args) => Show(); // Show login form when management form is closed

            window.Show();
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            Hide();

            ReportWindow window = new();
            window.Closed += (s, args) => Show(); // Show login form when management form is closed

            window.Show();
        }
    }
}
