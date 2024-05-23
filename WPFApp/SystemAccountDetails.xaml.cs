using BusinessObjects;
using Services.Interface;
using System.Text.RegularExpressions;
using System.Windows;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for SystemAccountDetails.xaml
    /// </summary>
    public partial class SystemAccountDetails : Window
    {
        public SystemAccountDetails()
        {
            InitializeComponent();
        }

        public ISystemAccountSvc _systemAccountSvc { get; set; }
        public bool InsertOrUpdate { get; set; }
        public bool EditPassword { get; set; }
        public SystemAccount systemAccountDetail { get; set; }

        private void btnCancel_Click(object sender, RoutedEventArgs e) => Close();

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SystemAccount systemAccount = new SystemAccount
                {
                    AccountId = Int16.Parse(txtAccountId.Text),
                    AccountName = txtAccountName.Text,
                    AccountPassword = txtAccountPass.Text,
                    AccountRole = Int32.Parse(txtAccountRole.Text)
                };

                // Validate email format
                if (!IsValidEmail(txtAccountEmail.Text))
                {
                    MessageBox.Show("Please enter a valid email address.", "Invalid Email", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; // Stop further execution
                }
                else
                {
                    systemAccount.AccountEmail = txtAccountEmail.Text;
                }

                if (InsertOrUpdate)
                {
                    _systemAccountSvc.UpdateSystemAccount(systemAccount);
                    MessageBox.Show("System Account " + systemAccount.AccountName + " updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _systemAccountSvc.AddSystemAccount(systemAccount);
                    MessageBox.Show("System Account " + systemAccount.AccountName + " added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, InsertOrUpdate == false ? "Add New Tag" : "Update A Tag");
            }
        }

        // Function to validate email format using regular expression
        private bool IsValidEmail(string email)
        {
            // Regex pattern for basic email format validation
            string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            return Regex.IsMatch(email, pattern);
        }


        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            txtAccountId.IsEnabled = !InsertOrUpdate;
            txtAccountPass.IsEnabled = !EditPassword;
            if (InsertOrUpdate)
            {
                txtAccountId.Text = systemAccountDetail.AccountId.ToString();
                txtAccountEmail.Text = systemAccountDetail.AccountEmail;
                txtAccountName.Text = systemAccountDetail.AccountName;
                txtAccountPass.Text = systemAccountDetail.AccountPassword;
                txtAccountRole.Text = systemAccountDetail.AccountRole.ToString();
            }
        }
    }
}
