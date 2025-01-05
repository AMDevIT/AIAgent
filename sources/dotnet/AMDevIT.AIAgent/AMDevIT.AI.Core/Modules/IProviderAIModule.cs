using Microsoft.Extensions.Logging;

namespace AMDevIT.AI.Core.Modules
{
    public interface IProviderAIModule
    {
        #region Properties

        ILogger? Logger
        {
            get;
        }

        string ModuleName
        {
            get;
        }

        #endregion

        #region Methods

        void OnStart(IProviderStateParameters? stateParameters);
        void OnStop(IProviderStateParameters? stateParameters);

        #endregion
    }
}
