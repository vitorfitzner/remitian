using Microsoft.EntityFrameworkCore;
using Remitian.Finance.Domain.AccountAgg;

namespace Remitian.Finance.Infra.Database.Repositories
{
    public class BankAccountRepository(FinanceDbContext db)
    {
        private readonly FinanceDbContext _db = db;

        public async Task<IEnumerable<BankAccount>> GetBankAccountsAsync(CancellationToken cancellationToken = default)
        {
            var accounts = await _db.BankAccounts
                .Include(a => a.Transactions)
                .ToListAsync(cancellationToken);

            return accounts;
        }

        public async Task<BankAccount> GetBankAccountAsync(int bankAccountId)
        {
            var account = await _db
                .BankAccounts
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.Id == bankAccountId);

            if (account is null)
            {
                throw new KeyNotFoundException($"Bank account with ID {bankAccountId} not found.");
            }

            return account;
        }

        public async Task UpdateBankAccountAsync(BankAccount bankAccount)
        {
            _db.BankAccounts.Update(bankAccount);
            await _db.SaveChangesAsync();
        }
    }
}
