using Remitian.Finance.Domain.AccountAgg;
using Remitian.Finance.Domain.TaxAccountAgg;
using Remitian.Finance.Domain.TaxAccountAgg.Events;

namespace Remitian.Finance.Domain.Tests
{
    public class TaxAccountTests
    {
        [Fact]
        public void ReceiveTransfer_Adds_Transaction_And_Event()
        {
            var tax = new TaxAccount { Id = 5, Name = "Gov" };
            int originId = 3;
            tax.ReceiveTransfer(originId, 25_00);

            Assert.Equal(25_00, tax.BalanceCents);
            Assert.Single(tax.Transactions);
            Assert.Single(tax.Events);

            var tx = tax.Transactions[0];
            Assert.Equal(TransactionType.TaxPayment, tx.TransactionType);
            Assert.Equal(25_00, tx.AmountCents);
            Assert.Equal(originId, tx.AccountOriginId);
            Assert.Equal(tax.Id, tx.AccountDestinationId);

            var evt = (TaxPaymentReceived)tax.Events[0];
            Assert.Equal("TaxPaymentReceived", evt.Name);
            Assert.Equal(25_00, evt.AmountCents);
            Assert.Equal(originId, evt.BankAccountOriginId);
            Assert.Equal(tax.Id, evt.TaxAccountId);
        }

        [Fact]
        public void ReceiveTransfer_NonPositive_Throws()
        {
            var tax = new TaxAccount { Id = 5, Name = "Gov" };

            Assert.Throws<ArgumentException>(() => tax.ReceiveTransfer(3, 0));
            Assert.Throws<ArgumentException>(() => tax.ReceiveTransfer(3, -10));

            Assert.Empty(tax.Transactions);
            Assert.Empty(tax.Events);
        }
    }
}