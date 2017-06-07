using System;
using Microsoft.AspNetCore.Http;

namespace Ataoge.Services
{
    public class BaseQueryParamsAccessor : IQueryParamsAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;

        protected BaseQueryParamsAccessor(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._httpContext = httpContextAccessor.HttpContext;
        }

        protected HttpContext HttpContext => _httpContext;

        public int GetIntParam(string name, int defaultValue = 0)
        {
            if (_httpContextAccessor.HttpContext.Request.Query.ContainsKey(name))
            {
                int returnValue;
                if (int.TryParse(_httpContextAccessor.HttpContext.Request.Query[name], out returnValue))
                    return returnValue;
            }
            return defaultValue;
        }

        public string GetStringParam(string name, string defaultValue = null)
        {
             if (_httpContextAccessor.HttpContext.Request.Query.ContainsKey(name))
                return _httpContextAccessor.HttpContext.Request.Query[name];
            return defaultValue;
        }

        public string GetClientIpAddress()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext ?? _httpContext;
                return httpContext?.Connection?.RemoteIpAddress?.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //return null;
        }

        protected virtual string PageIndexParamName  => "pageIndex";

        protected virtual string PageSizeParamName  => "pageSize";
        

        public int GetPageIndex(int defaultValue = 0)
        {
            return GetIntParam(PageIndexParamName, defaultValue);
        }

        public int GetPageSize(int defaultValue = 10)
        {
            return GetIntParam(PageSizeParamName, defaultValue);
        }

        protected string GetBaseUrl()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            return string.Format("{0}://{1}{2}", request.Scheme, request.Host, request.PathBase.Value);
            
        }

        protected virtual string GetBrowserInfo()
        {
            var httpContext = _httpContextAccessor.HttpContext ?? _httpContext;
            return httpContext?.Request?.Headers?["User-Agent"];
        }
    }
}