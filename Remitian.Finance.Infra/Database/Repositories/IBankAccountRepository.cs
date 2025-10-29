using Remitian.Finance.Domain.AccountAgg;
using Remitian.Finance.Domain.TaxAccountAgg;

namespace Remitian.Finance.Infra.Database.Repositories
{
    public interface IBankAccountRepository
    {
        Task<BankAccount> GetBankAccountAsync(int bankAccountId);
        Task<IEnumerable<BankAccount>> GetBankAccountsAsync(CancellationToken cancellationToken = default);
        Task UpdateBankAccountAsync(BankAccount bankAccount);
    }
}