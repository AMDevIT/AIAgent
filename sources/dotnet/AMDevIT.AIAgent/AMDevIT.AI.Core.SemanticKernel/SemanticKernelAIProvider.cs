using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

namespace AMDevIT.AI.Core
{
    public abstract class SemanticKernelAIProvider(string apiKey, 
                                                   string model, 
                                                   ILogger? logger)
        : AIProvider(apiKey, model, logger), ISemanticKernelAIProvider
    {
        #region Methods

        protected virtual void AddDefaultKernelPlugins(IKernelBuilder kernelBuilder)
        {
        }

        #endregion
    }
}
