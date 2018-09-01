using System.Reflection;
using Ataoge.GisCore.Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Ataoge.GisCore.Features
{
    public class FeatuerGeometryResolver : CamelCasePropertyNamesContractResolver
    {

        public readonly static FeatuerGeometryResolver Instance = new FeatuerGeometryResolver();
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (member.Name == "Geometry" && typeof(IHasGeometry).IsAssignableFrom(member.DeclaringType))
            {
                property.ShouldSerialize = instance => false;
               
            }
            else if(member.Name == "Id" &&  typeof(IHasGeometryWithId).IsAssignableFrom(member.DeclaringType))
            {
                property.ShouldSerialize = instance => false;
            }
            return property;
        }
    }
}