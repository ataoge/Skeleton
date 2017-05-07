using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Ataoge.Collections.Extensions;
using Ataoge.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ataoge.Modules
{
    public abstract class Module : IModule
    {
        protected Module()
        {
            
        }

        //public IStartupConfiguration StartupConfig {get; private set;}

        public IModuleManager ModuleManager {get; internal set;}

        public void ConfigureService(IServiceCollection services)
        {
            OnConfigureService(services);
        }

        protected virtual void OnConfigureService(IServiceCollection services)
        {

        }

        public void Initialize(IServiceProvider serviceProvider)
        {
            OnInitilize(serviceProvider);
        }

        protected virtual void OnInitilize(IServiceProvider serviceProvider)
        {

        }

        public void Shutdown()
        {

        }

        /// <summary>
        /// Gets the assembly in which the concrete module type is located. To avoid bugs whereby deriving from a module will
        /// change the target assembly, this property can only be used by modules that inherit directly from
        /// <see cref="Module"/>.
        /// </summary>
        protected virtual Assembly ThisAssembly
        {
            get
            {
                var thisType = GetType();
                var baseType = thisType.GetTypeInfo().BaseType;
                if (baseType != typeof(Module))
                    throw new InvalidOperationException(string.Format("{0} Assembly for {1} Unavailable", thisType, baseType));

                return thisType.GetTypeInfo().Assembly;
            }
        }

        /// <summary>
        /// Checks if given type is an Abp module class.
        /// </summary>
        /// <param name="type">Type to check</param>
        public static bool IsModule(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return
                typeInfo.IsClass &&
                !typeInfo.IsAbstract &&
                !typeInfo.IsGenericType &&
                typeof(Module).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo());
        }

        /// <summary>
        /// Finds direct depended modules of a module (excluding given module).
        /// </summary>
        public static List<Type> FindDependedModuleTypes(Type moduleType)
        {
            if (!IsModule(moduleType))
            {
                throw new SafException("This type is not a module: " + moduleType.AssemblyQualifiedName);
            }

            var list = new List<Type>();

            if (moduleType.GetTypeInfo().IsDefined(typeof(DependsOnAttribute), true))
            {
                var dependsOnAttributes = moduleType.GetTypeInfo().GetCustomAttributes(typeof(DependsOnAttribute), true).Cast<DependsOnAttribute>();
                foreach (var dependsOnAttribute in dependsOnAttributes)
                {
                    foreach (var dependedModuleType in dependsOnAttribute.DependedModuleTypes)
                    {
                        list.Add(dependedModuleType);
                    }
                }
            }

            return list;
        }

        public static List<Type> FindDependedModuleTypesRecursivelyIncludingGivenModule(Type moduleType)
        {
            var list = new List<Type>();
            AddModuleAndDependenciesRecursively(list, moduleType);
            list.AddIfNotContains(typeof(KernelModule));
            return list;
        }

        private static void AddModuleAndDependenciesRecursively(List<Type> modules, Type module)
        {
            if (!IsModule(module))
            {
                throw new SafException("This type is not an ABP module: " + module.AssemblyQualifiedName);
            }

            if (modules.Contains(module))
            {
                return;
            }

            modules.Add(module);

            var dependedModules = FindDependedModuleTypes(module);
            foreach (var dependedModule in dependedModules)
            {
                AddModuleAndDependenciesRecursively(modules, dependedModule);
            }
        }

    }
}