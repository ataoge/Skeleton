using System;
using Ataoge.Data;
using Microsoft.Extensions.Logging;

namespace Ataoge.Services
{
    public abstract class ServiceContextBase : IServiceContext
    {
        protected ServiceContextBase()
        {
            
        }

        public virtual IPageInfo PageInfo => new PageInfo();

        public abstract ILogger Logger {get;}

        public abstract string BrowserInfo  {get;}
   

        public abstract string ClientIpAddress {get;}

        public virtual int[] GetIntArrayForm(string name)
        {
            return null;
        }

        public virtual int[] GetIntArrayParam(string name)
        {
            return null;
        }

        public virtual int GetIntForm(string name, int defaultValue = 0)
        {
             return defaultValue;
        }

        public virtual int GetIntParam(string name, int defaultValue = 0)
        {
            return defaultValue;
        }

        public virtual string[] GetStringArrayForm(string name)
        {
           return null;
        }

        public virtual string[] GetStringArrayParam(string name)
        {
             return null;
        }

        public virtual string GetStringForm(string name, string defaultValue = null)
        {
            return defaultValue;
        }

        public virtual string GetStringParam(string name, string defaultValue = null)
        {
            return defaultValue;
        }

        public virtual string GetUserId()
        {
            return null;
        }
    }
}