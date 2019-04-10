using System;
using System.ComponentModel;
using System.Reflection;
using Ataoge.EventBus.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ataoge.EventBus.Utilities
{
    internal static class CommonHelper
    {

        public static bool IsController(TypeInfo typeInfo)
        {
            if (!typeInfo.IsClass)
            {
                return false;
            }

            if (typeInfo.IsAbstract)
            {
                return false;
            }

            if (!typeInfo.IsPublic)
            {
                return false;
            }

            return !typeInfo.ContainsGenericParameters
                   && typeInfo.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsComplexType(Type type)
        {
            return !CanConvertFromString(type);
        }

        private static bool CanConvertFromString(Type destinationType)
        {
            destinationType = Nullable.GetUnderlyingType(destinationType) ?? destinationType;
            return IsSimpleType(destinationType) ||
                   TypeDescriptor.GetConverter(destinationType).CanConvertFrom(typeof(string));
        }

        private static bool IsSimpleType(Type type)
        {
            return type.GetTypeInfo().IsPrimitive ||
                   type == typeof(decimal) ||
                   type == typeof(string) ||
                   type == typeof(DateTime) ||
                   type == typeof(Guid) ||
                   type == typeof(DateTimeOffset) ||
                   type == typeof(TimeSpan) ||
                   type == typeof(Uri);
        }

        private static JObject ToJObject(Exception exception)
        {
            return JObject.FromObject(new
            {
                exception.Source,
                exception.Message,
                InnerMessage = exception.InnerException?.Message
            });
        }

        private static JObject ToJObject(TracingHeaders headers)
        {
            var jobj = new JObject();
            foreach (var keyValuePair in headers)
            {
                jobj[keyValuePair.Key] = keyValuePair.Value;
            }
            return jobj;
        }

        private static string AddJsonProperty(string json, string propertyName, JObject propertyValue)
        {
            var jObj = JObject.Parse(json);

            if (jObj.TryGetValue(propertyName, out var _))
            {
                jObj[propertyName] = propertyValue;
            }
            else
            {
                jObj.Add(new JProperty(propertyName, propertyValue));
            }

            return jObj.ToString(Formatting.None);
        }

        public static string AddExceptionProperty(string json, Exception exception)
        {
            var jObject = ToJObject(exception);
            return AddJsonProperty(json, "ExceptionMessage", jObject);
        }

        public static string AddTracingHeaderProperty(string json, TracingHeaders headers)
        {
            var jObject = ToJObject(headers);
            return AddJsonProperty(json, nameof(TracingHeaders), jObject);
        }

        public static bool IsInnerIP(string ipAddress)
        {
            bool isInnerIp;
            var ipNum = GetIpNum(ipAddress);

            //Private IP：
            //category A: 10.0.0.0-10.255.255.255
            //category B: 172.16.0.0-172.31.255.255
            //category C: 192.168.0.0-192.168.255.255  

            var aBegin = GetIpNum("10.0.0.0");
            var aEnd = GetIpNum("10.255.255.255");
            var bBegin = GetIpNum("172.16.0.0");
            var bEnd = GetIpNum("172.31.255.255");
            var cBegin = GetIpNum("192.168.0.0");
            var cEnd = GetIpNum("192.168.255.255");
            isInnerIp = IsInner(ipNum, aBegin, aEnd) || IsInner(ipNum, bBegin, bEnd) || IsInner(ipNum, cBegin, cEnd);
            return isInnerIp;
        }

        private static long GetIpNum(string ipAddress)
        {
            var ip = ipAddress.Split('.');
            long a = int.Parse(ip[0]);
            long b = int.Parse(ip[1]);
            long c = int.Parse(ip[2]);
            long d = int.Parse(ip[3]);

            var ipNum = a * 256 * 256 * 256 + b * 256 * 256 + c * 256 + d;
            return ipNum;
        }

        private static bool IsInner(long userIp, long begin, long end)
        {
            return userIp >= begin && userIp <= end;
        }


    }
}