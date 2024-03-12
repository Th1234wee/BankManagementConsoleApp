namespace BankClassLibrary;

public class BankTransactionException : Exception
{
    private string _message = "";
    public BankTransactionException() { }
    public BankTransactionException(string message)
    {
        _message = message;
    }
    public override string Message
    {
        //when we invoke or call this variable 
        // this will be triggered the code inside the get accessor
        // or we can say that the purpose of doing this is
        // to implement string message that overwrite from
        //string Message in Exception Class(built in exception class)
        get
        {
            string result = _message;
            if(Data.Count > 0)
            {
                var extra = Data.Keys.Cast<string>()
                    .Select(key => $"[{key} : {Data[key]}")
                    .Aggregate((a, b) => $"{a}  ,   {b}");
                if(extra != null ) 
                { 
                    result += extra;
                }
            }
            return result;   
        }
    }
}
