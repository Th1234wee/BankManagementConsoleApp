namespace BankClassLibrary;

public class Transaction
{
    public Transaction() { }
    public TransactionType Type { get; init; } = TransactionType.None;
    public double Amount { get; init; } = 0;
    public DateTime Date { get; init; } = DateTime.Now;
    public string Note { get; init; } = "";
    public double GetActionAmount()
    {
        int Factor = Type switch
        {
            TransactionType.Create
            or TransactionType.Deposit
            or TransactionType.Reset => 1,
            TransactionType.Withdraw
            or TransactionType.Close => -1,
            _ => 0
        };
        return Amount * Factor;
    }
}