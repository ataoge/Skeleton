using System;
using Ataoge.AspNetCore;
using Ataoge.Configuration;
using Ataoge.Data;
using AutoMapper;
using Microsoft.Extensions.Options;

namespace Ataoge.AutoMapper
{
    public class FrontendUrlValueResolver : IValueResolver<IHasUrl, IHasUrl, string>
    {
        public FrontendUrlValueResolver(IUrlHelper urlHelper)//, IOptions<SettingsOptions> optionsAccessor)
        {
            this._urlHelper = urlHelper;
            //this._options = optionsAccessor.Value;
        }


        //private readonly SettingsOptions _options;

        private readonly IUrlHelper _urlHelper;

        
        public string Resolve(IHasUrl source, IHasUrl destination, string destMember, ResolutionContext context)
        {
            if (_urlHelper.IsAndroid() || _urlHelper.IsIOS())
                return _urlHelper.GenerateAbsoluteUrl(source.Url);
            return _urlHelper.GenerateNormalUrl(source.Url);
        }
    }


    public class FrontendUrlForPlatValueResolver : IValueResolver<IHasUrlForPlat, IHasUrl, string>
    {
        public FrontendUrlForPlatValueResolver(IUrlHelper urlHelper, IOptions<SettingsOptions> optionsAccessor)
        {
            this._urlHelper = urlHelper;
            this._options = optionsAccessor.Value;
        }

        private readonly SettingsOptions _options;

        private readonly IUrlHelper _urlHelper;

         public string Resolve(IHasUrlForPlat source, IHasUrl destination, string destMember, ResolutionContext context)
        {
            if (_urlHelper.IsAndroid() || _urlHelper.IsIOS())
            {
                if ((source.PlatSupport & 1) == 1) //Android
                {
                    return _options.AndroidBaseScheme + _urlHelper.GenerateNormalUrl(source.Url);
                }
                if ((source.PlatSupport & 2) == 2) //IOS
                {
                     return _options.IosBaseScheme + _urlHelper.GenerateNormalUrl(source.Url);
                }

                return _urlHelper.GenerateAbsoluteUrl(source.Url);
            }

            return _urlHelper.GenerateNormalUrl(source.Url);
        }
    }

    public class BackendUrlValueResolver : IValueResolver<IHasUrl, IHasUrl, string>
    {
        public BackendUrlValueResolver(IUrlHelper urlHelper)
        {
            this._urlHelper = urlHelper;
        }

        private readonly IUrlHelper _urlHelper;

        
        public string Resolve(IHasUrl source, IHasUrl destination, string destMember, ResolutionContext context)
        {
            return _urlHelper.GenerateRelativeUrl(source.Url);
        }
    }

    public class AtaogeUrlMembarValueResolver : IMemberValueResolver<IHasUrl, IHasUrl, string, string>
    {
        public AtaogeUrlMembarValueResolver(IUrlHelper urlHelper, IOptions<SettingsOptions> optionsAccessor)
        {
            this._urlHelper = urlHelper;
            this._options = optionsAccessor.Value;
        }

        private readonly SettingsOptions _options;
        private readonly IUrlHelper _urlHelper;
        public string Resolve(IHasUrl source, IHasUrl destination, string sourceMember, string destMember, ResolutionContext context)
        {
            if (context.Items.ContainsKey("flag"))
            {
                if (context.Items["flag"].ToString() == "todb")
                    return _urlHelper.GenerateRelativeUrl(sourceMember);
            } 

            if (_urlHelper.IsAndroid() || _urlHelper.IsIOS())
                return _urlHelper.GenerateAbsoluteUrl(source.Url);
            return _urlHelper.GenerateNormalUrl(source.Url);
        }
    }
}