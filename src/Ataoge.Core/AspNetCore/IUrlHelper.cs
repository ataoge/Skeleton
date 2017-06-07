namespace Ataoge.AspNetCore
{
    public interface IUrlHelper
    {
        bool CanUseRelativeUrl(string url);

        string GenerateNormalUrl(string url);

        string GenerateAbsoluteUrl(string url);

        string GenerateRelativeUrl(string url);

         string BrowserInfo { get;}
    }
}