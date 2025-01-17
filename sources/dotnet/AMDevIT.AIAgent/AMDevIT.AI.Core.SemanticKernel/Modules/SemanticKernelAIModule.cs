﻿using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

namespace AMDevIT.AI.Core.Modules
{
    public abstract class SemanticKernelAIModule(ILogger? logger)
       : ISemanticKernelAIModule
    {
        #region Properties

        public ILogger? Logger
        {
            get;
            protected set;
        } = logger;

        public abstract string ModuleName
        {
            get;
        }

        public abstract bool RegisterKernelFunctions
        {
            get;
        }

        #endregion        

        #region Methods

        public virtual IKernelBuilderPlugins AddToKernelBuilder(IKernelBuilder kernelBuilder)
        {
            this.Logger?.LogTrace("Adding plugin {moduleName} to kernel builder", 
                                  this.ModuleName);
            IKernelBuilderPlugins plugins = kernelBuilder.Plugins.AddFromObject(this);
            return plugins;
        }

        public virtual void OnStart(IProviderStateParameters? stateParameters)
        {
            this.Logger?.LogDebug("Module {moduleName} started.",
                                  this.ModuleName);
        }

        public virtual void OnStop(IProviderStateParameters? stateParameters)
        {
            this.Logger?.LogDebug("Module {moduleName} stopped.",
                                  this.ModuleName);
        }

        #endregion
    }
}
