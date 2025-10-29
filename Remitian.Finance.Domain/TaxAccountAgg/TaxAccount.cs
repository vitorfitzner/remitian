using Remitian.Finance.Domain.AccountAgg;
using Remitian.Finance.Domain.TaxAccountAgg.Events;

namespace Remitian.Finance.Domain.TaxAccountAgg
{

    public class TaxAccount
    {
        public int Id { get; set; }
        public required string Name  { get; set; }
        public int BalanceCents => Transactions.Sum(x => x.AmountCents);
        public List<Transaction> Transactions { get; private set; } = [];
        public List<Event> Events { get; private set; } = [];

        public void ReceiveTransfer(int bankAccountOriginId, int amountCents)
        {
            if (amountCents <= 0)
            {
                throw new ArgumentException("Payment amount must be positive", nameof(amountCents));
            }

            var transaction = new Transaction(bankAccountOriginId, accountDestinationId: Id, TransactionType.TaxPayment, amountCents);

            Transactions.Add(transaction);

            Events.Add(new TaxPaymentReceived()
            {
                AmountCents = amountCents,
                BankAccountOriginId = bankAccountOriginId,
                TaxAccountId = Id,
                Date = DateTime.UtcNow,
                Name = "TaxPaymentReceived",
            });
        }
    }
}
