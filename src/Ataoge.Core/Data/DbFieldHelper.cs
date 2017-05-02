namespace Ataoge.Data
{
    internal static class DbFieldHelper
    {
        public static bool IsFlag(this SafFieldType safFieldType, int flag)
        {
             return (flag & (int)safFieldType) == (int)safFieldType;
        }

       
    }
}