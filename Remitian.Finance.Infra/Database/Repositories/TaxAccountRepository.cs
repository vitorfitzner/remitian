using Microsoft.EntityFrameworkCore;
using Remitian.Finance.Domain.TaxAccountAgg;

namespace Remitian.Finance.Infra.Database.Repositories
{
    public class TaxAccountRepository(FinanceDbContext db) : ITaxAccountRepository
    {
        private readonly FinanceDbContext _db = db;
        
        public async Task<TaxAccount> GetTaxAccountByIdAsync(int taxAccountId)
        {
            var account = await _db
                .TaxAccounts
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.Id == taxAccountId);

            if (account is null)
            {
                throw new KeyNotFoundException($"Tax account with ID {taxAccountId} not found.");
            }

            return account;
        }

        public async Task Save(TaxAccount taxAccount)
        {
            _db.TaxAccounts.Update(taxAccount);
            await _db.SaveChangesAsync();
        }
    }
}
