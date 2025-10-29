using Microsoft.AspNetCore.SignalR;
using Remitian.Finance.Api.Hubs;
using Remitian.Finance.Infra.Database.Repositories;

namespace Remitian.Finance.Api.Resources.BankAccountResource
{
    /// <summary>
    /// Provides services for managing bank accounts
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="hubContext"></param>
    public class BankAccountService(
        BankAccountRepository repository,
        IHubContext<NotificationsHub> hubContext)
    {

        #region dependencies
        private readonly BankAccountRepository _repository = repository;
        private readonly IHubContext<NotificationsHub> _hub = hubContext;
        #endregion

        public async Task<IEnumerable<BankAccountDto>> GetBankAccountsAsync(BankAccountFilter filter, CancellationToken cancellationToken = default)
        {
            var bankAccounts = await _repository.GetBankAccountsAsync(cancellationToken);

            return bankAccounts.Select(account => new BankAccountDto
            {
                Name = account.Name,
                BalanceCents = account.BalanceCents
            });
        }

        public async Task Deposit(int bankAccountId, int amountCents)
        {
            var bankAccount = await _repository.GetBankAccountAsync(bankAccountId);

            bankAccount.Deposit(amountCents);

            await _repository.UpdateBankAccountAsync(bankAccount);
        }

        public async Task TransferTo(int bankAccountId, int taxAccountId, int amountCents)
        {
            var bankAccount = await _repository.GetBankAccountAsync(bankAccountId);
            var taxAccount = await _repository.GetTaxAccountAsync(taxAccountId);

            bankAccount.TransferTo(taxAccount, amountCents);

            await _repository.UpdateBankAccountAsync(bankAccount);
            await _hub.Clients.All.SendAsync("ReceiveMessage", taxAccount.Events);
        }

        public class BankAccountDto
        {
            public required string Name { get; set; }
            public int BalanceCents { get; set; }
        }
    }
}
