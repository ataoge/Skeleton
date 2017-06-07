namespace Ataoge.AspNetCore
{
    public static class UrlHelperExtension
    {
        public static bool IsAndroid(this IUrlHelper sc)
        {
            if (sc.BrowserInfo.Contains("Android"))
                return true;
            return false;
        }

        public static bool IsiPhone(this IUrlHelper sc)
        {
            if (sc.BrowserInfo.Contains("iPhone"))
                return true;
            return false;
        }

        public static bool IsiPad(this IUrlHelper sc)
        {
            if (sc.BrowserInfo.Contains("iPad"))
                return true;
            return false;
        }

        public static bool IsIOS(this IUrlHelper sc)
        {
            return IsiPhone(sc) || IsiPad(sc);
        }
    }
}