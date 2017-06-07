using System.Collections.Generic;
using AutoMapper;

namespace Ataoge.Configuration
{
    public interface IProfileProvider
    {
        IEnumerable<Profile> GetProfiles();
    }
}