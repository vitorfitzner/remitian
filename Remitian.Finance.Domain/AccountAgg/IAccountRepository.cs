
namespace Remitian.Finance.Domain.AccountAgg
{
    public interface IAccountRepository
    {
        BankAccount GetAccountById(Guid sourceAccountId);
        BankAccount GetAccounts();
        void Save(BankAccount account);
    }
}
