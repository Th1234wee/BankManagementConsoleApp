
namespace BankClassLibrary;

public class BankAccount
{
    //initialize starting number's account
    public const long StartingNumber = 900000;
    public static int Count = 0;

    protected string _number = "";
    protected double _balance = 0;
    protected List<Transaction> _transaction = new List<Transaction>();
    protected bool isClosed = false;

    public string Number => _number;
    public string Holder {  get; set; }
    public double Balance => _balance;
    public bool IsClosed => isClosed;
    public IEnumerable<Transaction> Transactions => _transaction.AsEnumerable();

    public static BankAccount Create(string holder , double balance)
    {
        BankTransactionException? exception = null;
        string msg = "Invalid Argument";
        if(string.IsNullOrEmpty(holder.Trim()))
        {
            exception ??= new BankTransactionException(msg);
            exception.Data["Holder"] = "(empty)";
        }
        if(balance < 0)
        {
            exception ??= new BankTransactionException(msg);
            exception.Data["Initial Balance"] = balance;

        }
        if(exception != null)
        {
            throw exception;                
        }
        return new BankAccount(holder.Trim(), balance);
    }
    public void ApplyTransaction(Transaction transaction)
    {
        _balance += transaction.GetActionAmount();
        _transaction.Add(transaction);
    }
    protected BankAccount(string holder, double initBalance)
    {
        _number = (BankAccount.StartingNumber + BankAccount.Count + 1).ToString();
        Holder = holder;
        _balance = 0;
        ApplyTransaction(new Transaction()
        {
            Type = TransactionType.Create,
            Amount = initBalance,
            Date = DateTime.Now,
            Note = "Creating New Account",
        });
        BankAccount.Count++;
    }
    protected void EnsureNotCLosed()
    {
        if (isClosed)
            throw new Exception($"The Account , {_number} has been closed");
    }
    public void Reset(string note)
    {
        EnsureNotCLosed();
        _transaction.Clear();
        ApplyTransaction(new Transaction()
        {
            Type = TransactionType.Reset,
            Amount = 0,
            Date = DateTime.Now,
            Note = note
        });
    }
    public void Close(string note)
    {
        EnsureNotCLosed();
        ApplyTransaction(new Transaction()
        {
            Type = TransactionType.Close,
            Amount = _balance,
            Date = DateTime.Now,
            Note = note
        });
        isClosed = true;
    }
    public void Deposit(double amount, string note)
    {
        EnsureNotCLosed();
        if(amount <= 0)
        {
            var exception = new BankTransactionException("Amount is not positve");
            exception.Data["amount"] = amount;
            throw exception;
        }
        ApplyTransaction(new Transaction()
        {
            Type = TransactionType.Deposit,
            Amount = amount,
            Date = DateTime.Now,
            Note = note
        });
    }
    public void WithDraw(double amount, string note)
    {
        EnsureNotCLosed();
        if(amount <= 0)
        {
            var exception = new BankTransactionException("The amount is not positive");
            exception.Data["amount"] = amount;
            throw exception;
        }
        if(_balance - amount < 0)
        {
            var exception = new BankTransactionException("Amount is overdrawed");
            exception.Data["amount"] = amount;
            throw exception;
        }
        ApplyTransaction(new Transaction()
        {
            Type= TransactionType.Withdraw,
            Amount = amount,
            Date = DateTime.Now,
            Note = note
        });
    }
}
