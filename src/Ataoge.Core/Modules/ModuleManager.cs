using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Ataoge.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ataoge.Modules
{
    /// <summary>
    /// This class is used to manage modules.
    /// </summary>
    public class ModuleManager : IModuleManager
    {
        public ModuleInfo StartupModule { get; private set; }

        public IReadOnlyList<ModuleInfo> Modules => _modules.ToImmutableList();

        private ModuleCollection _modules;

        private SkeletonOptions _SkeletonOptions;

        public ModuleManager(SkeletonOptions skeletonOptions)
        {
            this._SkeletonOptions = skeletonOptions;
        }

        public SkeletonOptions SkeletonOptions => _SkeletonOptions;

        public virtual void Initialize(Type startupModule)
        {
            _modules = new ModuleCollection(startupModule);
            LoadAllModules();
        }

        public virtual void ConfigModules(IServiceCollection services)
        {
            var sortedModules = _modules.GetSortedModuleListByDependency();
            sortedModules.ForEach(module => module.Instance.ConfiguringService(services));
            sortedModules.Reverse();
            sortedModules.ForEach(module => module.Instance.ConfiguredService(services));
        }

        public virtual void StartModules(IServiceProvider serviceProvider)
        {
            var sortedModules = _modules.GetSortedModuleListByDependency();
            //sortedModules.ForEach(module => module.Instance.PreInitialize());
            sortedModules.ForEach(module => module.Instance.Initialize(serviceProvider));
            //sortedModules.ForEach(module => module.Instance.PostInitialize());
          
        }

        public virtual void ShutdownModules()
        {
            //Logger.Debug("Shutting down has been started");

            var sortedModules = _modules.GetSortedModuleListByDependency();
            sortedModules.Reverse();
            sortedModules.ForEach(sm => sm.Instance.Shutdown());

            //Logger.Debug("Shutting down completed.");
        }

        private void LoadAllModules()
        {
            //Logger.Debug("Loading Abp modules...");

            List<Type> plugInModuleTypes;
            var moduleTypes = FindAllModuleTypes(out plugInModuleTypes).Distinct().ToList();

            //Logger.Debug("Found " + moduleTypes.Count + " ABP modules in total.");

            RegisterModules(moduleTypes);
            CreateModules(moduleTypes, plugInModuleTypes);

            _modules.EnsureKernelModuleToBeFirst();
            _modules.EnsureStartupModuleToBeLast();

            SetDependencies();

            //Logger.DebugFormat("{0} modules loaded.", _modules.Count);
        }

        private List<Type> FindAllModuleTypes(out List<Type> plugInModuleTypes)
        {
            plugInModuleTypes = new List<Type>();

            var modules = ModuleBase.FindDependedModuleTypesRecursivelyIncludingGivenModule(_modules.StartupModuleType);
            
            //foreach (var plugInModuleType in _abpPlugInManager.PlugInSources.GetAllModules())
            //{
            //    if (modules.AddIfNotContains(plugInModuleType))
            //    {
            //        plugInModuleTypes.Add(plugInModuleType);
            //    }
            //}

            return modules;
        }

        private void CreateModules(ICollection<Type> moduleTypes, List<Type> plugInModuleTypes)
        {
            foreach (var moduleType in moduleTypes)
            {
                var moduleObject = Activator.CreateInstance(moduleType) as ModuleBase; //_iocManager.Resolve(moduleType) as AbpModule;
                if (moduleObject == null)
                {
                    throw new SafException("This type is not an ABP module: " + moduleType.AssemblyQualifiedName);
                }

                //moduleObject.IocManager = _iocManager;
                //moduleObject.Configuration = _iocManager.Resolve<IAbpStartupConfiguration>();
                moduleObject.ModuleManager = this;

                var moduleInfo = new ModuleInfo(moduleType, moduleObject, plugInModuleTypes.Contains(moduleType));

                _modules.Add(moduleInfo);

                if (moduleType == _modules.StartupModuleType)
                {
                    StartupModule = moduleInfo;
                }

                //Logger.DebugFormat("Loaded module: " + moduleType.AssemblyQualifiedName);
            }
        }

        private void RegisterModules(ICollection<Type> moduleTypes)
        {
            foreach (var moduleType in moduleTypes)
            {
                //_iocManager.RegisterIfNot(moduleType);
            }
        }

        private void SetDependencies()
        {
            foreach (var moduleInfo in _modules)
            {
                moduleInfo.Dependencies.Clear();

                //Set dependencies for defined DependsOnAttribute attribute(s).
                foreach (var dependedModuleType in ModuleBase.FindDependedModuleTypes(moduleInfo.Type))
                {
                    var dependedModuleInfo = _modules.FirstOrDefault(m => m.Type == dependedModuleType);
                    if (dependedModuleInfo == null)
                    {
                        throw new SafException("Could not find a depended module " + dependedModuleType.AssemblyQualifiedName + " for " + moduleInfo.Type.AssemblyQualifiedName);
                    }

                    if ((moduleInfo.Dependencies.FirstOrDefault(dm => dm.Type == dependedModuleType) == null))
                    {
                        moduleInfo.Dependencies.Add(dependedModuleInfo);
                    }
                }
            }
        }
    }

  
}