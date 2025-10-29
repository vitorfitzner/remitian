using Remitian.Finance.Domain.AccountAgg;
namespace Remitian.Finance.Infra.Database.Repositories
{
    public interface IBankAccountRepository
    {
        Task<BankAccount> GetBankAccountAsync(int bankAccountId);
        Task<IEnumerable<BankAccount>> GetBankAccountsAsync(CancellationToken cancellationToken = default);
        Task UpdateBankAccountAsync(BankAccount bankAccount);
    }
}