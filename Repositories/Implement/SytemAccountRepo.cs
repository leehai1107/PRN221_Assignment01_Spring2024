using BusinessObjects;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Repositories.Interface;

namespace Repositories.Implement
{
    public class SytemAccountRepo : ISystemAccountRepo
    {
        public void AddSystemAccount(SystemAccount account)
        {
            using (FunewsManagementDbContext _context = new())
            {
                try
                {
                    _context.SystemAccounts.Add(account);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception("Can not add this account", ex);

                }
            }
        }

        public SystemAccount GetAccountByEmail(string email)
        {
            using (FunewsManagementDbContext _context = new())
                try
                {
                    return _context.SystemAccounts.FirstOrDefault(x => x.AccountEmail == email);
                }
                catch (Exception ex)
                {
                    throw new Exception("Can not get account", ex);
                }
        }

        public List<SystemAccount> GetAccounts()
        {
            using (FunewsManagementDbContext _context = new())
                return _context.SystemAccounts.Include(x => x.NewsArticles).ToList();
        }

        public SystemAccount GetSystemAccountById(short id)
        {
            using (FunewsManagementDbContext _context = new())
                return _context.SystemAccounts.Find(id);
        }

        public void RemoveSystemAccount(short id)
        {
            using (FunewsManagementDbContext _context = new())
            {
                var accountToRemove = _context.SystemAccounts.Find(id);
                if (accountToRemove != null)
                {
                    try
                    {
                        _context.SystemAccounts.Remove(accountToRemove);
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Can not delete this account", ex);

                    }
                }
                else
                {
                    throw new ArgumentException("Account not found", nameof(id));
                }
            }
        }

        public void UpdateSystemAccount(SystemAccount newAccount)
        {
            using (FunewsManagementDbContext _context = new())
            {
                var existAccount = _context.SystemAccounts.Find(newAccount.AccountId);
                if (existAccount != null)
                {
                    try
                    {
                        existAccount.AccountName = newAccount.AccountName;
                        existAccount.AccountEmail = newAccount.AccountEmail;
                        existAccount.AccountRole = newAccount.AccountRole;
                        existAccount.AccountPassword = newAccount.AccountPassword;

                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Can not update this account", ex);

                    }
                }
                else { throw new ArgumentException("Account not found", newAccount.AccountId.ToString()); }
            }
        }
        public List<SystemAccount> SearchAccountsByName(string accountName)
        {
            using (FunewsManagementDbContext _context = new())
            {
                // Query the database for categories matching the provided name
                return _context.SystemAccounts
                               .Where(a => a.AccountName.Contains(accountName))
                               .ToList();
            }
        }
    }
}
