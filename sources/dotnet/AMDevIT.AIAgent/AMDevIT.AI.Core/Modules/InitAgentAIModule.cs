using Microsoft.Extensions.Logging;

namespace AMDevIT.AI.Core.Modules
{
    [UniqueModule]
    public abstract class InitAgentAIModule(ILogger? logger)
        : IInitAgentAIModule
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

        #endregion

        #region Methods

        public abstract string BuildInitAgentMessage();

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
