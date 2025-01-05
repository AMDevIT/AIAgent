using AMDevIT.AI.Core;
using AMDevIT.AI.Core.Modules;
using Microsoft.Extensions.Logging;

namespace AMDevIT.AI.App.WPF.Modules
{
    // The aim of this module is to show how to create a custom semantic kernel module
    // and to test the new features of the SemanticKernelAIModule that allows to catch the Start and Stop event
    // of the semantic kernel provider to eventually initialize or do other tasks before the user can start to interact with the AI.

    internal class TestSemanticKernelModule(ILogger? logger)
        : SemanticKernelAIModule(logger)
    {
        #region Consts

        private const string TestSemanticKernelModuleName = "TestSemanticKernelModule";

        #endregion

        #region Properties

        public override string ModuleName
        {
            get => TestSemanticKernelModuleName;            
        }

        /// <summary>
        /// This module doesn't need to register kernel functions. So it returns false. If set to true without implementing 
        /// the RegisterKernelFunctions method, an exception will be thrown.
        /// </summary>
        public override bool RegisterKernelFunctions
        {
            get => false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method is called when the semantic kernel provider is started.
        /// </summary>
        /// <param name="stateParameters">The state parameter of the AI provider. For the Semantic Kernel provider, it includes chat history to manipulate system and user messages.</param>
        /// <exception cref="ArgumentException">If the state parameter is not a valid <see cref="SemanticKernelAIProviderStateParameters"/> 
        /// state parameter then an exception will be thrown.</exception>
        override public void OnStart(IProviderStateParameters? stateParameters)
        {
            if (stateParameters is SemanticKernelAIProviderStateParameters semanticKernelAIProviderStateParameters)
            {
                this.Logger?.LogInformation("Regitering test semantic kernel module system messages.");
                // I'm a man of colture, so I'm going to add a system message to the history of the semantic kernel using a quote from the movie "Ghostbusters" (1984).
                // It's localized in italian, hope you understand it.
                semanticKernelAIProviderStateParameters.History?.AddSystemMessage("Quando ti chiedono se il modulo di test è installato, tu gli devi rispondere \"Come " +
                                                                                  "quando ti chiedono sei sei un Dio, si!\"");
            }
            else
            {
                throw new ArgumentException("Invalid state parameters type.");
            }
        }

        #endregion
    }
}
