using BusinessObjects;

namespace Services.Interface
{
    public interface ISystemAccountSvc
    {
        bool Validate(string email, string password);

        public List<SystemAccount> GetAccounts();
        public List<SystemAccount> SearchAccountsByName(string accountName);
        public SystemAccount GetSystemAccountById(short id);
        public void AddSystemAccount(SystemAccount account);
        public void RemoveSystemAccount(short id);
        public void UpdateSystemAccount(SystemAccount newAccount);
        public SystemAccount GetAccountByEmail(string email);
    }
}
