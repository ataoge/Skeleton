using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

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

        public int[] GetIntArrayParam(string name)
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

        public string GetStringParam(string name, string defaultValue = null)
        {
             if (_httpContextAccessor.HttpContext.Request.Query.ContainsKey(name))
                return _httpContextAccessor.HttpContext.Request.Query[name];
            return defaultValue;
        }

        public string[] GetStringArrayParam(string name)
        {
            if (_httpContextAccessor.HttpContext.Request.Form.ContainsKey(name))
                return _httpContextAccessor.HttpContext.Request.Form[name].ToArray();
            return null;
        }


        protected StringValues GetOriginForm(string name)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            return request.Form[name];
        }

        protected StringValues GetOriginQuery(string name)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            return request.Query[name];
        }

        protected IFormFile GetOriginFile(string name)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            return request.Form.Files[name];
        }

        public string GetFormFileName(string name)
        {
            var formFile = GetOriginFile(name);
            return formFile.FileName;
        }

        public string GetFormFileContentType(string name)
        {
            var formFile = GetOriginFile(name);
            return formFile.ContentType;
        }

        public byte[] GetFormFileContent(string name)
        {
            
            var formFile = GetOriginFile(name);
            var bytes = new byte[formFile.Length];
            try
            {
                formFile.OpenReadStream().Read(bytes, 0, (int)formFile.Length);
                return bytes;
            }
            catch
            {
                return null;
            }
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

        protected string GetBaseUrl(bool withPath = false)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            if (withPath)
                return string.Format("{0}://{1}{2}{3}", request.Scheme, request.Host, request.PathBase.Value, request.Path);
            return string.Format("{0}://{1}{2}", request.Scheme, request.Host, request.PathBase.Value);
        }

        public bool IsAjaxRequest()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            return "XMLHttpRequest".Equals(request.Headers["X-Requested-With"]);
        }
       

        protected virtual string GetBrowserInfo()
        {
            var httpContext = _httpContextAccessor.HttpContext ?? _httpContext;
            return httpContext?.Request?.Headers?["User-Agent"];
        }
    }
}