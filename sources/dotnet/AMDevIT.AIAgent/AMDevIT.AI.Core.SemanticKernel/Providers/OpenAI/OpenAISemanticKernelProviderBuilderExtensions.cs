﻿using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace AMDevIT.AI.Core.Providers.OpenAI
{
    public static class OpenAISemanticKernelProviderBuilderExtensions
    {
        #region Consts

        private const string DefaultModelID = "gpt-4o-mini";

        #endregion

        #region Methods

        public static ISemanticKernelAIProvider BuildOpenAISemanticKernelAIProvider(this SemanticKernelAIProviderBuilder source)
        {
            if (string.IsNullOrWhiteSpace(source.APIKey))
                throw new NullReferenceException("Cannot build OpenAI Semantic Kernel Provider without an API Key.");

            string model = source.ModelID ?? DefaultModelID;

            source.Logger?.LogInformation("Building OpenAI Semantic Kernel Provider with model {model}.", model);

            OpenAISemanticKernelProvider provider = new(source.APIKey, model, source.Logger);
            provider.Modules.AddModules(source.Modules);
            return provider;
        }

        #endregion
    }
}
