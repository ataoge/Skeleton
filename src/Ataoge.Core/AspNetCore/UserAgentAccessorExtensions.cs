using System.Linq;

namespace Ataoge.AspNetCore
{
    public static class UserAgentAccessorExtentions
    {
        public static bool IsAndroid(this IUserAgentAccessor sc)
        {
            if (string.IsNullOrEmpty(sc.BrowserInfo))
                return false;

            if (sc.BrowserInfo.Contains("Android"))
                return true;
            return false;
        }

        public static bool IsiPhone(this IUserAgentAccessor sc)
        {
            if (string.IsNullOrEmpty(sc.BrowserInfo))
                return false;
                
            if (sc.BrowserInfo.Contains("iPhone"))
                return true;
            return false;
        }

        public static bool IsiPad(this IUserAgentAccessor sc)
        {
            if (string.IsNullOrEmpty(sc.BrowserInfo))
                return false;

            if (sc.BrowserInfo.Contains("iPad"))
                return true;
            return false;
        }

        public static bool IsIOS(this IUserAgentAccessor sc)
        {
            if (string.IsNullOrEmpty(sc.BrowserInfo))
                return false;
            return IsiPhone(sc) || IsiPad(sc);
        }

        
    }
}