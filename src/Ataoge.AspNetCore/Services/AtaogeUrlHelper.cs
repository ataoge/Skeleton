using System;
using Microsoft.AspNetCore.Http;

namespace Ataoge.AspNetCore
{
    public class AtaogeUrlHelper : IUrlHelper
    {
        public AtaogeUrlHelper(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public bool CanUseRelativeUrl(string url)
        {
            return url.StartsWith(this.BaseUrl);
        }

        public string GenerateAbsoluteUrl(string url)
        {
            if (IsRelativeLocalUrl(url))
            {
                return BaseUrl + url.TrimStart('~');
            }
            return url;
        }

        public string GenerateNormalUrl(string url)
        {
            if (IsRelativeLocalUrl(url))
            {
                if (string.IsNullOrEmpty(GetPathBase()))
                    return url;
                else
                    return GetPathBase() + url.TrimStart('~');
    
            }
            return url;
        }

        public string BrowserInfo => GetBrowserInfo();
        
        
        protected virtual string GetBrowserInfo()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            return httpContext?.Request?.Headers?["User-Agent"];
        }

        public string GenerateRelativeUrl(string url)
        {
            if (url.StartsWith(BaseUrl))
                return url.Substring(BaseUrl.Length);
            return url;
        }

        bool IsLocalUrl(string url)
        {
            return IsRelativeLocalUrl(url) || url.StartsWith(this.BaseUrl);
        }

        static bool IsRelativeLocalUrl(string url)
        {
    
            //This code is copied from System.Web.WebPages.RequestExtensions class.

            if (string.IsNullOrEmpty(url))  
                return false;
            if ((int)url[0] == 47 && (url.Length == 1 || (int)url[1] != 47 && (int)url[1] != 92))
                return true;
            if (url.Length > 1 && (int)url[0] == 126)
                return (int)url[1] == 47;
            return false;
   
        }

        private string _baseUrl;
        public string BaseUrl
        {
            get 
            {
                if (string.IsNullOrEmpty(_baseUrl))
                    _baseUrl = GeBaseUrl();
                return _baseUrl;
            }
        } 

        private string GeBaseUrl(bool withPath = false)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            if (withPath)
                return string.Format("{0}://{1}{2}{3}", request.Scheme, request.Host, request.PathBase.Value, request.Path);
            return string.Format("{0}://{1}{2}", request.Scheme, request.Host, request.PathBase.Value);
            
        }

        private string GetPathBase()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            return request.PathBase.Value;
        }
    }
}