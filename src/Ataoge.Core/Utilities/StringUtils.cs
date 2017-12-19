using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Ataoge.Utilities
{
    public static class StringUtils
    {

        public static string GenerateSequentialGuidString(int guidType = 0)
        {
            // We start with 16 bytes of cryptographically strong random data.
            var randomBytes = new byte[10];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            // An alternate method: use a normally-created GUID to get our initial
            // random data:
            // byte[] randomBytes = Guid.NewGuid().ToByteArray();
            // This is faster than using RNGCryptoServiceProvider, but I don't
            // recommend it because the .NET Framework makes no guarantee of the
            // randomness of GUID data, and future versions (or different
            // implementations like Mono) might use a different method.

            // Now we have the random basis for our GUID.  Next, we need to
            // create the six-byte block which will be our timestamp.

            // We start with the number of milliseconds that have elapsed since
            // DateTime.MinValue.  This will form the timestamp.  There's no use
            // being more specific than milliseconds, since DateTime.Now has
            // limited resolution.

            // Using millisecond resolution for our 48-bit timestamp gives us
            // about 5900 years before the timestamp overflows and cycles.
            // Hopefully this should be sufficient for most purposes. :)
            long timestamp = DateTime.UtcNow.Ticks / 10000L;

            // Then get the bytes
            byte[] timestampBytes = BitConverter.GetBytes(timestamp);

            // Since we're converting from an Int64, we have to reverse on
            // little-endian systems.
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(timestampBytes);
            }

            byte[] guidBytes = new byte[16];

            switch(guidType)
            {
                case 0:
                case 1:
                    // For string and byte-array version, we copy the timestamp first, followed
                    // by the random data.
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);

                    if (guidType == 0 && BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(guidBytes, 0, 4);
                        Array.Reverse(guidBytes, 4, 2);
                    }

                    break;
                case 2:
                    // For sequential-at-the-end versions, we copy the random data first,
                    // followed by the timestamp.
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);
                    break;
                default:
                    guidBytes = Guid.NewGuid().ToByteArray();
                    var counterBytes = BitConverter.GetBytes(DateTime.UtcNow.Ticks); //Interlocked.Increment(ref _counter));

                    if (!BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(counterBytes);
                    }

                    guidBytes[08] = counterBytes[1];
                    guidBytes[09] = counterBytes[0];
                    guidBytes[10] = counterBytes[7];
                    guidBytes[11] = counterBytes[6];
                    guidBytes[12] = counterBytes[5];
                    guidBytes[13] = counterBytes[4];
                    guidBytes[14] = counterBytes[3];
                    guidBytes[15] = counterBytes[2];

                    return new Guid(guidBytes).ToString("N");
            }

            return new Guid(guidBytes).ToString("N");
        }

        public static string ToCamelCase([NotNull]this string value)
        {
            return char.ToLower(value[0]) + value.Substring(1);
        }

        public static string NormalizeForKey([NotNull]this string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            return name.ToLower();
        }

        public static bool CheckString(string source, string regStr)
        {
            if (string.IsNullOrEmpty(source)) return false;
            Regex r = new Regex(regStr, RegexOptions.IgnoreCase);
            Match m = r.Match(source);
            if (m.Success) return true;
            else return false;
        }

        public static bool IsMobilePhone(string value)
        {
            return CheckString(value, REG_MOBILEPHONE);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgionString"></param>
        /// <returns></returns>
        public static string GetDeleteString(string orgionString)
        {
            if (string.IsNullOrEmpty(orgionString))
                throw new ArgumentNullException("参数值为空！");

            //if (orgionString.StartsWith(REG_DELETEMARKER))
                //return orgionString;
            Random random = new Random();
            int delta = random.Next(10000);

            return REG_DELETEMARKER + orgionString + "#" + delta.ToString();
        }

        public static string GetOrginString(string deleteString)
        {
            if (string.IsNullOrEmpty(deleteString))
                throw new ArgumentNullException("参数值为空！");


            if (deleteString.StartsWith(REG_DELETEMARKER))
            {
                int index = deleteString.IndexOf('#');
                if (index > 0)
                    return deleteString.Substring(1, index - 1);
                else
                    return deleteString.TrimStart(REG_DELETEMARKER.ToCharArray());
            }

            return deleteString;
        }

        public const string REG_EMAIL = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";  //@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
        public const string REG_EMAIL2 = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
        public const string REG_URL = @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
        public const string REG_INCLUDECHINESE = @"[\u4e00-\u9fa5]+";
        public const string REG_CHINESEONLY = @"^[\u4e00-\u9fa5]+$";
        public const string REG_PHONE = @"^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$";

        public const string REG_MOBILEPHONE = "^((\\+86)|(86))?((13[0-9])|(14[5|7])|(15([0-3]|[5-9]))|(18[0,5-9]))\\d{8}$";
        public const string REG_DATETIME = @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$";
        public const string REG_DELETEMARKER = @"!";



    }
}