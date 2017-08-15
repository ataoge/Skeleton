using System;
using System.Linq;
using Ataoge.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Ataoge.Services
{
    public class AspNetCoreServiceContext : ServiceContextBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly ILoggerFactory _loggerFactory;

        private readonly ILogger _logger;

        public AspNetCoreServiceContext(IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._httpContext = httpContextAccessor.HttpContext;
            this._loggerFactory = loggerFactory;
            this._logger = loggerFactory.CreateLogger<AspNetCoreServiceContext>();

        }

        public override IPageInfo PageInfo 
        {
            get 
            {
                int index = GetIntParam("pageindex",0);
                int size = GetIntParam("pagesize",20);
                return new PageInfo(index, size);
            }
        }

     

        public override string BrowserInfo => GetBrowserInfo();

        public override string ClientIpAddress => GetClientIpAddress();

        public override ILogger Logger => this._logger;

        public override int[] GetIntArrayForm(string name)
        {
            if (_httpContextAccessor.HttpContext.Request.Form.ContainsKey(name))
            {
                return _httpContextAccessor.HttpContext.Request.Form[name].Select(v => {
                    int returnValue;
                    if (int.TryParse(v, out returnValue))
                        return returnValue;
                    return 0;
                }).ToArray();
               
            } 
            return null;
        }

        public override int[] GetIntArrayParam(string name)
        {
            if (_httpContextAccessor.HttpContext.Request.Query.ContainsKey(name))
            {
                return _httpContextAccessor.HttpContext.Request.Query[name].Select(v => {
                    int returnValue;
                    if (int.TryParse(v, out returnValue))
                        return returnValue;
                    return 0;
                }).ToArray();
               
            }  
            return null;
        }

        public override int GetIntForm(string name, int defaultValue = 0)
        {
            if (_httpContextAccessor.HttpContext.Request.Form.ContainsKey(name))
            {
                int returnValue;
                if (int.TryParse(_httpContextAccessor.HttpContext.Request.Form[name], out returnValue))
                    return returnValue;
            }
            return defaultValue;

        }

        public override int GetIntParam(string name, int defaultValue = 0)
        {
            if (_httpContextAccessor.HttpContext.Request.Query.ContainsKey(name))
            {
                int returnValue;
                if (int.TryParse(_httpContextAccessor.HttpContext.Request.Query[name], out returnValue))
                    return returnValue;
            }
            return defaultValue;
        }

        public override string[] GetStringArrayForm(string name)
        {
            if (_httpContextAccessor.HttpContext.Request.Form.ContainsKey(name))
                return _httpContextAccessor.HttpContext.Request.Form[name].ToArray();
            return null;
        }

        public override string[] GetStringArrayParam(string name)
        {
            if (_httpContextAccessor.HttpContext.Request.Query.ContainsKey(name))
                return _httpContextAccessor.HttpContext.Request.Query[name].ToArray();
            return null;
        }

        public override string GetStringForm(string name, string defaultValue = null)
        {
             if (_httpContextAccessor.HttpContext.Request.Form.ContainsKey(name))
                return _httpContextAccessor.HttpContext.Request.Form[name];
            return defaultValue;
        }

        public override string GetStringParam(string name, string defaultValue = null)
        {
            if (_httpContextAccessor.HttpContext.Request.Query.ContainsKey(name))
                return _httpContextAccessor.HttpContext.Request.Query[name];
            return defaultValue;
        }

        protected virtual string GetBrowserInfo()
        {
            var httpContext = _httpContextAccessor.HttpContext ?? _httpContext;
            return httpContext?.Request?.Headers?["User-Agent"];
        }

        protected virtual string GetClientIpAddress()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext ?? _httpContext;
                return httpContext?.Connection?.RemoteIpAddress?.ToString();
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex.ToString());
            }

            return null;
        }

        
    }
}