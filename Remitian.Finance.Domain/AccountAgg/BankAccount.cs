namespace Remitian.Finance.Domain.AccountAgg
{
    public class BankAccount
    {
        public int Id { get; set; }
        public required string Name  { get; set; }
        public List<Transaction> Transactions { get; private set; } = [];

        #region balance

        public int BalanceCents => Transactions.Sum(x => x.AmountCents);

        public void Deposit(int amountCents)
        {
            if (amountCents <= 0)
            {
                throw new ArgumentException("Deposit amount must be positive", nameof(amountCents));
            }

            var transaction = new Transaction(TransactionType.Deposit, amountCents);

            Transactions.Add(transaction);
        }

        public void Withdraw(int amountCents)
        {
            if (amountCents <= 0)
            { 
                throw new ArgumentException("Withdrawal amount must be positive.");
            }

            if (amountCents > BalanceCents)
            { 
                throw new InvalidOperationException("Insufficient funds.");
            }

            var transaction = new Transaction(TransactionType.Withdrawal, amountCents * -1);

            Transactions.Add(transaction);
        }

        public void TransferTo(BankAccount targetAccount, int amountCents)
        {
            Withdraw(amountCents);

            targetAccount.Deposit(amountCents);
        }

        #endregion
    }
}
