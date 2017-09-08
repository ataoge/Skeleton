namespace Ataoge.AspNetCore
{
    public interface IUrlHelper : IUserAgentAccessor
    {
        bool CanUseRelativeUrl(string url);

        string GenerateNormalUrl(string url);

        string GenerateAbsoluteUrl(string url);

        string GenerateRelativeUrl(string url);

    }
}