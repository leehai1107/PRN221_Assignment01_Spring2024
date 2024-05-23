using BusinessObjects;
using Microsoft.Extensions.Configuration;
using Repositories.Implement;
using Repositories.Interface;
using Services.Interface;

namespace Services.Implement
{
    public class SystemAccountSvc : ISystemAccountSvc
    {
        private readonly ISystemAccountRepo _systemAccountRepo;
        private string adminEmail;

        private void GetAdminAccount()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            IConfigurationSection section = config.GetSection("AdminAccount");

            adminEmail = section["Email"];
        }

        private bool SignUpWithAdminAccount(string email)
        {
            if (email == adminEmail)
            {
                return true;
            }
            return false;
        }

        public SystemAccountSvc()
        {
            _systemAccountRepo = new SytemAccountRepo();
        }

        public void AddSystemAccount(SystemAccount account)
        {
            GetAdminAccount();
            if (SignUpWithAdminAccount(account.AccountEmail))
            {
                throw new Exception("Email already exits!");
            }


            SystemAccount existAccount = _systemAccountRepo.GetAccountByEmail(account.AccountEmail);
            if (existAccount != null)
            {
                throw new Exception("Email already exits!");
            }
            else
            {
                _systemAccountRepo.AddSystemAccount(account);
            }
        }
        public List<SystemAccount> SearchAccountsByName(string accountName)
        {
            return _systemAccountRepo.SearchAccountsByName(accountName);
        }

        public SystemAccount GetAccountByEmail(string email)
        {
            return _systemAccountRepo.GetAccountByEmail(email);
        }

        public List<SystemAccount> GetAccounts()
        {
            return _systemAccountRepo.GetAccounts();
        }

        public SystemAccount GetSystemAccountById(short id)
        {
            return _systemAccountRepo.GetSystemAccountById(id);
        }

        public void RemoveSystemAccount(short id)
        {
            _systemAccountRepo.RemoveSystemAccount(id);
        }

        public void UpdateSystemAccount(SystemAccount newAccount)
        {
            _systemAccountRepo.UpdateSystemAccount(newAccount);
        }

        public bool Validate(string email, string password)
        {
            try
            {
                var account = _systemAccountRepo.GetAccountByEmail(email);
                if (account == null)
                {
                    return false;
                }
                else if (account.AccountPassword != password)
                {
                    return false;
                }
                else if (account.AccountRole == 2)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("You have no permision to this system!", ex);
            }
        }
    }
}
