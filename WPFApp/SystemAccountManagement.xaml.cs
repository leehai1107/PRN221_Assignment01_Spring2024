using BusinessObjects;
using Services.Implement;
using Services.Interface;
using System.Windows;
using System.Windows.Controls;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for SystemAccountManagement.xaml
    /// </summary>
    public partial class SystemAccountManagement : Window
    {
        private ISystemAccountSvc _systemAccountSvc;
        public SystemAccountManagement()
        {
            _systemAccountSvc = new SystemAccountSvc();
            InitializeComponent();
            LoadAccounts();
        }

        private void LoadAccounts()
        {
            List<SystemAccount> systemAccounts = _systemAccountSvc.GetAccounts();
            if (systemAccounts.Count > 0)
            {
                btnDelete.IsEnabled = true;
            }
            else
            {
                btnDelete.IsEnabled = false;
            }

            lvAccounts.ItemsSource = _systemAccountSvc.GetAccounts();
        }


        private void lvAccountsMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SystemAccount item = (sender as ListView).SelectedItem as SystemAccount;
            if (item != null)
            {
                txtAccountId.Text = item.AccountId.ToString();
                txtAccountName.Text = item.AccountName;
                txtAccountEmail.Text = item.AccountEmail;
                txtAccountPass.Text = item.AccountPassword;
                txtAccountRole.Text = item.AccountRole.ToString();
            }
        }


        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var id = txtAccountId.Text.Trim();
            if (id.Length > 0)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this account?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _systemAccountSvc.RemoveSystemAccount(Convert.ToInt16(id));
                    LoadAccounts();
                }

            }
            else
            {
                MessageBox.Show("Please input valid data!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            SystemAccountDetails systemAccountDetails = new SystemAccountDetails
            {
                Title = "Add System Account",
                InsertOrUpdate = false,
                _systemAccountSvc = _systemAccountSvc,
            };

            systemAccountDetails.Show();
            systemAccountDetails.Closed += (s, args) => LoadAccounts();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e) => Close();

        private void lvAccounts_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SystemAccountDetails systemAccountDetails = new SystemAccountDetails
            {
                Title = "Update System Account",
                InsertOrUpdate = true,
                _systemAccountSvc = _systemAccountSvc,
                systemAccountDetail = GetSystemAccountDetails(),
            };

            systemAccountDetails.Show();
            systemAccountDetails.Closed += (s, args) => LoadAccounts();
        }

        private void lvAccounts_Loaded(object sender, RoutedEventArgs e)
        {
            btnDelete.IsEnabled = false;
            LoadAccounts();
        }

        private SystemAccount GetSystemAccountDetails()
        {
            SystemAccount systemAccountDetail = null;
            try
            {
                systemAccountDetail = new SystemAccount
                {
                    AccountId = Int16.Parse(txtAccountId.Text),
                    AccountEmail = txtAccountEmail.Text,
                    AccountName = txtAccountName.Text,
                    AccountPassword = txtAccountPass.Text,
                    AccountRole = Int32.Parse(txtAccountRole.Text),
                };

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Get System Account");

            }
            return systemAccountDetail;
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            LoadAccounts();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            List<SystemAccount> systemAccounts = _systemAccountSvc.SearchAccountsByName(txtSearch.Text);
            if (systemAccounts.Count > 0)
            {
                btnDelete.IsEnabled = true;
            }
            else
            {
                btnDelete.IsEnabled = false;
            }

            lvAccounts.ItemsSource = _systemAccountSvc.GetAccounts();
        }
    }
}
