using BusinessObjects;

namespace Repositories.Interface
{
    public interface ISystemAccountRepo
    {
        public SystemAccount GetAccountByEmail(string email);
        public List<SystemAccount> GetAccounts();
        public SystemAccount GetSystemAccountById(short id);
        public void AddSystemAccount(SystemAccount account);
        public void RemoveSystemAccount(short id);
        public void UpdateSystemAccount(SystemAccount newAccount);
        public List<SystemAccount> SearchAccountsByName(string accountName);
    }
}
