using System;
using AutoMapper;

namespace Ataoge.AutoMapper.Tests
{
    public class TestMapperProfile : Profile
    {
        public TestMapperProfile()
        {
             CreateMap<TestEntity, TestDto>(MemberList.Destination)
                .ForMember(t => t.Url, opt => opt.MapFrom<TestValueResolver>());
        }

    }

    public class TestSingleton
    {
        public TestSingleton()
        {
            BasePath = new Guid().ToString();
        }

        public string BasePath {get;}
    }

    public class TestValueResolver : IValueResolver<TestEntity, TestDto, string>
    {
        public TestValueResolver(TestSingleton testSingleton)
        {
            _singleton = testSingleton;
        }

        public readonly TestSingleton _singleton;
        public string Resolve(TestEntity source, TestDto destination, string destMember, ResolutionContext context)
        {
            return source.Uid + " And " +_singleton.BasePath;
        }
    }
}