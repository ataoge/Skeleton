using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace Ataoge.Configuration
{
    public class AssemblyProfileProvider : IProfileProvider
    {
        public AssemblyProfileProvider(IEnumerable<Assembly> assemblies)
        {
            this.Assemblies = assemblies;
        }

        public IEnumerable<Assembly> Assemblies { get; }

        public IEnumerable<Profile> GetProfiles()
        {
            return this.Assemblies.SelectMany( x => x.DefinedTypes )
				.Select( x => x.AsType() )
				.Where( x => typeof(Profile).GetTypeInfo().IsAssignableFrom( x )  && !x.GetTypeInfo().IsGenericType)
				.Select( x => ( Profile) Activator.CreateInstance( x ) );
        }
    }
}