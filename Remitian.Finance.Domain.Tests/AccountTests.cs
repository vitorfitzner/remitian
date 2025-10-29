using Remitian.Finance.Domain.AccountAgg;
using Remitian.Finance.Domain.TaxAccountAgg;
using Remitian.Finance.Domain.TaxAccountAgg.Events;

namespace Remitian.Finance.Domain.Tests
{
    public class AccountTests
    {
        [Fact]
        public void Deposit_Increases_Balance_And_Registers_Transaction()
        {
            var account = new BankAccount { Name = "A", Id = 1 };

            account.Deposit(50_00);

            Assert.Equal(50_00, account.BalanceCents);
            Assert.Single(account.Transactions);
            var tx = account.Transactions[0];
            Assert.Equal(TransactionType.Deposit, tx.TransactionType);
            Assert.Equal(50_00, tx.AmountCents);
        }

        [Fact]
        public void Deposit_NonPositive_Throws()
        {
            var account = new BankAccount { Name = "A" };

            var exZeroAmount = Assert.Throws<ArgumentException>(() => account.Deposit(0));
            var exNegativeAmount = Assert.Throws<ArgumentException>(() => account.Deposit(-10));
            Assert.Equal("Deposit amount must be positive. (Parameter 'amountCents')", exZeroAmount.Message);
            Assert.Equal("Deposit amount must be positive. (Parameter 'amountCents')", exNegativeAmount.Message);
        }

        [Fact]
        public void Withdraw_Decreases_Balance_And_Registers_Negative_Transaction()
        {
            var account = new BankAccount { Name = "A" };
            account.Deposit(80_00);

            account.Withdraw(30_00);

            Assert.Equal(50_00, account.BalanceCents);
            Assert.Equal(2, account.Transactions.Count);
            var withdrawal = account.Transactions.Last();
            Assert.Equal(TransactionType.Withdrawal, withdrawal.TransactionType);
            Assert.Equal(-30_00, withdrawal.AmountCents);
        }

        [Fact]
        public void Withdraw_More_Than_Balance_Throws()
        {
            var account = new BankAccount { Name = "A" };
            account.Deposit(20_00);

            var ex = Assert.Throws<InvalidOperationException>(() => account.Withdraw(30_00));

            Assert.Equal("Insufficient funds for withdrawal.", ex.Message);
        }

        [Fact]
        public void Withdraw_NonPositive_Throws()
        {
            var account = new BankAccount { Name = "A" };
            account.Deposit(20_00);

            var exZeroAmount = Assert.Throws<ArgumentException>(() => account.Withdraw(0));
            var exNegativeAmount = Assert.Throws<ArgumentException>(() => account.Withdraw(-5));

            Assert.Equal("Withdrawal amount must be positive. (Parameter 'amountCents')", exZeroAmount.Message);
            Assert.Equal("Withdrawal amount must be positive. (Parameter 'amountCents')", exNegativeAmount.Message);
        }

        [Fact]
        public void TransferTo_Moves_Funds_Between_Accounts()
        {
            var sourceAccount = new BankAccount { Name = "Vitor", Id = 1 };
            sourceAccount.Deposit(30_00);

            var targetAccount = new BankAccount { Name = "PO", Id = 2 };
            int transferAmount = 10_00;

            sourceAccount.TransferTo(targetAccount, transferAmount);

            Assert.Equal(20_00, sourceAccount.BalanceCents);
            Assert.Equal(10_00, targetAccount.BalanceCents);

            Assert.Equal(2, sourceAccount.Transactions.Count);
            Assert.Single(targetAccount.Transactions);

            var withdrawal = sourceAccount.Transactions.Last();
            Assert.Equal(TransactionType.Withdrawal, withdrawal.TransactionType);
            Assert.Equal(-10_00, withdrawal.AmountCents);

            var deposit = targetAccount.Transactions.Last();
            Assert.Equal(TransactionType.Deposit, deposit.TransactionType);
            Assert.Equal(10_00, deposit.AmountCents);
        }

        [Fact]
        public void TransferTo_InsufficientFunds_Throws_And_DoesNot_Modify_Target()
        {
            var source = new BankAccount { Name = "Source" };
            source.Deposit(5_00);
            var target = new BankAccount { Name = "Target" };

            var ex = Assert.Throws<InvalidOperationException>(() => source.TransferTo(target, 10_00));

            Assert.Equal(5_00, source.BalanceCents);
            Assert.Equal(0, target.BalanceCents);
            Assert.Single(source.Transactions);
            Assert.Empty(target.Transactions);
            Assert.Equal("Insufficient funds for transfer.", ex.Message);
        }

        [Fact]
        public void TransferTo_TaxAccount_Creates_TaxPayment_Event_And_Transaction()
        {
            var bank = new BankAccount { Name = "Main", Id = 10 };
            bank.Deposit(100_00);

            var tax = new TaxAccount { Name = "IRS", Id = 99 };

            bank.TransferTo(tax, 40_00);

            // Bank
            Assert.Equal(60_00, bank.BalanceCents);
            Assert.Equal(2, bank.Transactions.Count);
            Assert.Equal(-40_00, bank.Transactions.Last().AmountCents);
            Assert.Equal(TransactionType.Withdrawal, bank.Transactions.Last().TransactionType);

            // Tax Account
            Assert.Equal(40_00, tax.BalanceCents);
            Assert.Single(tax.Transactions);
            var taxTx = tax.Transactions[0];
            Assert.Equal(TransactionType.TaxPayment, taxTx.TransactionType);
            Assert.Equal(40_00, taxTx.AmountCents);
            Assert.Equal(bank.Id, taxTx.AccountOriginId);
            Assert.Equal(tax.Id, taxTx.AccountDestinationId);

            // Event
            Assert.Single(tax.Events);
            var evt = (TaxPaymentReceived)tax.Events[0];
            Assert.Equal("TaxPaymentReceived", evt.Name);
            Assert.Equal(40_00, evt.AmountCents);
            Assert.Equal(bank.Id, evt.BankAccountOriginId);
            Assert.Equal(tax.Id, evt.TaxAccountId);
        }

        [Fact]
        public void TransferTo_TaxAccount_InsufficientFunds_Throws_No_Event()
        {
            var bank = new BankAccount { Name = "Main", Id = 10 };
            bank.Deposit(10_00);

            var tax = new TaxAccount { Name = "IRS", Id = 99 };

            var ex = Assert.Throws<InvalidOperationException>(() => bank.TransferTo(tax, 20_00));

            Assert.Equal(10_00, bank.BalanceCents);
            Assert.Empty(tax.Transactions);
            Assert.Empty(tax.Events);
            Assert.Equal("Insufficient funds for transfer.", ex.Message);
        }
    }
}
