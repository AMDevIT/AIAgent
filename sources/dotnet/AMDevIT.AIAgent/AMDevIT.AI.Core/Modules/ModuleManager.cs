using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AMDevIT.AI.Core.Modules
{
    public class ModuleManager(ILogger? logger)
    {
        #region Fields

        private readonly Dictionary<Type, IProviderAIModule> uniqueModules = [];
        private readonly List<IProviderAIModule> duplicableModules = [];

        #endregion

        #region Properties

        protected ILogger? Logger
        {
            get;
            private set;
        } = logger;

        #endregion

        #region .ctor

        #endregion

        #region Methods

        public void AddModule<T>(T module)
            where T : IProviderAIModule
        {
            this.AddModule(module);         
        }

        public void AddModule(IProviderAIModule module)
        {
            Type moduleType = module.GetType();

            bool isUniqueModule = Attribute.IsDefined(moduleType, typeof(UniqueModuleAttribute));

            if (isUniqueModule)
            {
                if (uniqueModules.ContainsKey(moduleType))
                {
                    this.Logger?.LogError("An object with attribute UniqueModuleAttribute " +
                                          "of {typeName} is already registered.",
                                          moduleType.Name);

                    throw new InvalidOperationException($"An object with attribute UniqueModuleAttribute " +
                                                        $"of {moduleType.Name} is already registered.");
                }
                uniqueModules[moduleType] = module;
                this.Logger?.LogDebug("Unique module of type {typeName} added.", moduleType.Name);
            }
            else
            {
                duplicableModules.Add(module);
                this.Logger?.LogDebug("Duplicable module of type {typeName} added.", moduleType.Name);
            }
        }

        public void AddModules(IEnumerable<IProviderAIModule> modules)
        {
            foreach (IProviderAIModule module in modules)
            {
                try
                {
                    this.Logger?.LogDebug("Adding module of type {moduleType}.",
                                          module.ModuleName);
                    this.AddModule(module);
                }
                catch (Exception exc)
                {
                    this.Logger?.LogError(exc, "Cannot add modules to the ModuleManager.");
                    throw;
                }
            }
        }

        public T? GetModule<T>()
            where T : IProviderAIModule
        {
            Type type = typeof(T);

            if (uniqueModules.TryGetValue(type, out var uniqueModule))
                return (T)uniqueModule;

            T? duplicableModule = duplicableModules.OfType<T>()
                                                   .FirstOrDefault();
            return duplicableModule;
        }

        public IEnumerable<T> GetModules<T>()
            where T : IProviderAIModule
        {
            Type type = typeof(T);

            if (uniqueModules.TryGetValue(type, out var uniqueModule))
                return [(T)uniqueModule];

            return duplicableModules.OfType<T>();
        }

        public IEnumerable<IProviderAIModule> GetInitChatModules()
        {
            List<IProviderAIModule> initChatProvidersModules = [];

            if (this.uniqueModules.Count > 0)
            {
                this.uniqueModules.Values.OfType<IInitAgentAIModule>()
                                         .ToList()
                                         .ForEach(initChatProvidersModules.Add);
            }

            if (this.duplicableModules.Count > 0)
            {
                this.duplicableModules.OfType<IInitAgentAIModule>()
                                      .ToList()
                                      .ForEach(initChatProvidersModules.Add);
            }

            return [.. initChatProvidersModules];
        }

        public IEnumerable<IProviderAIModule> GetNonInitChatModules()
        {
            List<IProviderAIModule> nonInitChatProvidersModules = [];

            if (this.uniqueModules.Count > 0)
            {
                this.uniqueModules.Values.Where(module => module is not IInitAgentAIModule)
                                         .ToList()
                                         .ForEach(nonInitChatProvidersModules.Add);
            }

            if (this.duplicableModules.Count > 0)
            {
                this.duplicableModules.Where(module => module is not IInitAgentAIModule)
                                      .ToList()
                                      .ForEach(nonInitChatProvidersModules.Add);
            }

            return nonInitChatProvidersModules;
        }

        public bool RemoveModule<T>()
            where T : IProviderAIModule
        {
            Type type = typeof(T);

            if (uniqueModules.ContainsKey(type))
            {
                return uniqueModules.Remove(type);
            }
            else
            {
                T? moduleToRemove = duplicableModules.OfType<T>()
                                                     .FirstOrDefault();
                if (moduleToRemove != null)
                    return duplicableModules.Remove(moduleToRemove);
            }

            return false;
        }

        #endregion
    }
}
