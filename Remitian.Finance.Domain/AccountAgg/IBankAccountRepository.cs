using Remitian.Finance.Domain.TaxAccountAgg;

namespace Remitian.Finance.Domain.AccountAgg
{
    public interface IBankAccountRepository
    {
        Task<BankAccount> GetBankAccountAsync(int bankAccountId);
        Task<IEnumerable<BankAccount>> GetBankAccountsAsync(CancellationToken cancellationToken = default);
        Task<TaxAccount> GetTaxAccountAsync(int taxAccountId);
        Task UpdateBankAccountAsync(BankAccount bankAccount);
    }
}