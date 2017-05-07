namespace Ataoge.Data
{
    public interface IConcurrencyStamp
    {
        string ConcurrencyStamp
        {
            get;
            set;
        }
    }
}