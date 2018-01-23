using System;
using Ataoge.Configuration;
using Ataoge.Modules;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Ataoge.AutoMapper
{
    [DependsOn(typeof(KernelModule))]
    public class AtaogeAutoMapperModule : ModuleBase
    {

     
        protected override void OnConfiguringService(IServiceCollection services)
        {
            //services.AddSingleton<IAtaogeAutoMapperConfiguration, AtaogeAutoMapperConfiguration>();
            services.AddSingleton<ObjectMapping.IObjectMapper, AutoMapperObjectMapper>();
            
            services.AddSingleton<FrontendUrlValueResolver>();
            services.AddSingleton<BackendUrlValueResolver>();
            services.AddSingleton<HtmlContentValueResolver>();

            services.AddSingleton<AtaogeUrlMembarValueResolver>();
            services.AddSingleton<ClaimExtensionValueResolver>();
        }

        protected override void OnConfiguredService(IServiceCollection services)
        {
            AutoMapperConfiguration  autoMapperConfiguration = GetOrCreateExtension<AutoMapperConfiguration>();
            Action<IMapperConfigurationExpression> configurer = configuration =>
            {
                //FindAndAutoMapTypes(configuration);
                if (autoMapperConfiguration.Configurators != null) {
                    foreach (var configurator in autoMapperConfiguration.Configurators)
                    {
                        configurator(configuration);
                    }
                }

                if (autoMapperConfiguration.Provider != null)
                {
                    foreach(var profile in autoMapperConfiguration.Provider.GetProfiles())
                    {
                        configuration.AddProfile(profile);
                    }
                }
            };


            
            if (autoMapperConfiguration.StaticMapper)
            {
                Mapper.Initialize(configurer);
                services.AddSingleton(Mapper.Configuration);
                //services.AddScoped<IMapper>(sp => 
                //  new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService));
                services.AddSingleton<IMapper>(sp => 
                   new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService));

                //services.AddSingleton<IMapper>(Mapper.Instance);
            }
            else
            {
                var config = new MapperConfiguration(configurer);
            
                services.AddSingleton<IMapper>(config.CreateMapper());
            }
        }

        protected override void OnInitilize(IServiceProvider serviceProvider)
        {

        }
    }
}
