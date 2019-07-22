using System;
using System.Collections.Generic;

namespace Ataoge.Utilities
{
    public static class LockObjectsManager
    {
        private static Dictionary<string, object> lockObjectsNames = new Dictionary<string, object>();
        public static object GetLockObject(string name)
        {
            object lockObject = null;
            if (lockObjectsNames.ContainsKey(name))
            {
                lockObject = lockObjectsNames[name];
            }
            else
            {
                lock (lockObjectsNames)
                {
                    if (lockObjectsNames.ContainsKey(name))
                    {
                        lockObject = lockObjectsNames[name];
                    }
                    else
                    {
                        lockObjectsNames[name] = lockObject = new Object();
                    }
                }

            }
            return lockObject;
        } 

        public static void RemoveLockObject(string name)
        {
            if (lockObjectsNames.ContainsKey(name))
            {
                lock (lockObjectsNames)
                {
                    lockObjectsNames.Remove(name);
                }
            }
        }
    }
}