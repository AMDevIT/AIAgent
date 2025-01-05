using Microsoft.SemanticKernel.ChatCompletion;

namespace AMDevIT.AI.Core
{
    public class SemanticKernelAIProviderStateParameters(ChatHistory? chatHistory)
        : IProviderStateParameters
    {
        #region Fields

        protected ChatHistory? chatHistory = chatHistory;

        #endregion

        #region Properties

        public ChatHistory? History
        {
            get => this.chatHistory;
        }

        #endregion
    }
}
