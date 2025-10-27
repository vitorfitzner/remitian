namespace Remitian.Finance.Domain.AccountAgg
{
    public class Transaction
    {
        #region constructors

        internal Transaction()
        {
            Date = DateTime.UtcNow;
        }

        internal Transaction(TransactionType type, int amountCents) : this()
        {
            TransactionType = type;
            AmountCents = amountCents;
        }

        #endregion

        public int Id { get; private set; }
        public int AmountCents { get; private set; }
        public TransactionType TransactionType { get; private set; }
        public DateTime Date { get; private set; }
    }

    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        Transfer
    }
}
