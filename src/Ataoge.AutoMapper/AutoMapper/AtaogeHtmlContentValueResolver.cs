using System;
using Ataoge.AspNetCore;
using Ataoge.Data;
using AutoMapper;

namespace Ataoge.AutoMapper
{
    public class HtmlContentValueResolver : IValueResolver<IHtmlContent, IHtmlContent, string>
    {
        public HtmlContentValueResolver(IUrlHelper urlHelper)
        {
            this._urlHelper = urlHelper;
        }

        private readonly IUrlHelper _urlHelper;

        public virtual string Resolve(IHtmlContent source, IHtmlContent destination, string destMember, ResolutionContext context)
        {
   
            var content = source.Content.Replace("src=\"~/", "src=\"" + _urlHelper.GenerateAbsoluteUrl("~/"));
            content = content.Replace("src=\"/", "src=\"" + _urlHelper.GenerateAbsoluteUrl("/"));
            content = content.Replace("href=\"~/", "href=\"" + _urlHelper.GenerateAbsoluteUrl("~/"));
            return content.Replace("href=\"/", "href=\"" + _urlHelper.GenerateAbsoluteUrl("/"));;
            
        }
    }
}
