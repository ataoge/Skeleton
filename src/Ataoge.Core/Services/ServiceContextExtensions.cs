using System.Linq;

namespace Ataoge.Services
{
    public static class ServiceContextExtensions
    {
        public static bool IsMobileDevice(this IServiceContext sc)
        {
            var agent = sc.BrowserInfo;
            if (agent != null && _mobileKeywords.Any(keyword => agent.Contains(keyword)))
                return true;
            return false;
        }

        public static bool IsAndroid(this IServiceContext sc)
        {
            if (sc.BrowserInfo.Contains("Android"))
                return true;
            return false;
        }

        public static bool IsiPhone(this IServiceContext sc)
        {
            if (sc.BrowserInfo.Contains("iPhone"))
                return true;
            return false;
        }

        public static bool IsiPad(this IServiceContext sc)
        {
            if (sc.BrowserInfo.Contains("iPad"))
                return true;
            return false;
        }

        public static bool IsIOS(this IServiceContext sc)
        {
            return IsiPhone(sc) || IsiPad(sc);
        }

        #region yaml    
        private readonly static string[] _tabletKeywords = {
            "tablet",
            "ipad",
            "playbook",
            "hp-tablet",
            "kindle"
        };

        private readonly static string[] _crawlerKeywords = {
            "bot",
            "slurp",
            "spider"
        };

        private readonly static string[] _mobileKeywords = {
            "blackberry",
            "webos",
            "ipod",
            "lge vx",
            "midp",
            "maemo",
            "mmp",
            "mobile",
            "netfront",
            "hiptop",
            "nintendo DS",
            "novarra",
            "openweb",
            "opera mobi",
            "opera mini",
            "phone",
            "smartphone",
            "symbian",
            "up.browser",
            "up.link"
 
        };
        #endregion
    }
}