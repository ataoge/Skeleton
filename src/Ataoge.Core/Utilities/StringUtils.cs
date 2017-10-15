using System;
using System.Security.Cryptography;
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
    }
}