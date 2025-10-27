using Remitian.Finance.Domain.AccountAgg;

namespace Remitian.Finance.Domain.Tests
{
    public class AccountTests
    {
        [Fact]
        public void TransferTo_Moves_Funds_Between_Accounts()
        {
            // Arrange
            var sourceAccount = new BankAccount { Name = "Vitor" };
            var depositAmount = 30_00;

            sourceAccount.Deposit(depositAmount);

            var targetAccount = new BankAccount { Name = "PO" };
            int transferAmount = 10_00;

            // Act
            sourceAccount.TransferTo(targetAccount, transferAmount);

            // Assert
            Assert.Equal(20_00, sourceAccount.BalanceCents);
            Assert.Equal(10_00, targetAccount.BalanceCents);
        }
    }
}
