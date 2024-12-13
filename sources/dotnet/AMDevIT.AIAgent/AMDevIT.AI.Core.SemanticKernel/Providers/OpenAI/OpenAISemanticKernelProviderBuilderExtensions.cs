namespace AMDevIT.AI.Core.Providers.OpenAI
{
    public static class OpenAISemanticKernelProviderBuilderExtensions
    {
        #region Consts

        private const string DefaultModelID = "gpt-4o-mini";

        #endregion

        #region Methods

        public static ISemanticKernelAIProvider BuildOpenAISemanticKernelProvider(this SemanticKernelAIProviderBuilder source)
        {
            if (string.IsNullOrWhiteSpace(source.APIKey))
                throw new ArgumentNullException(nameof(source.APIKey), "Cannot build OpenAI Semantic Kernel Provider without an API Key.");

            string model = source.ModelID ?? DefaultModelID;

            OpenAISemanticKernelProvider provider = new OpenAISemanticKernelProvider(source.APIKey,
                                                                                     model,
                                                                                     source.Logger);
            provider.Modules.AddModules(source.Modules);
            return provider;
        }

        #endregion
    }
}
