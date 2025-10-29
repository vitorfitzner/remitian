
namespace Remitian.Finance.Domain.TaxAccountAgg.Events
{
    public class TaxPaymentReceived : Event
    {
        public int TaxAccountId { get; init; }
        public int BankAccountOriginId { get; init; }
        public int AmountCents { get; init; }
    }
}
