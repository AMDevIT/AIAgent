using AMDevIT.AI.Core.Modules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AMDevIT.AI.Core
{
    public abstract class SemanticKernelAIProvider(string apiKey, 
                                                   string model, 
                                                   ILogger? logger)
        : AIProvider(apiKey, model, logger), ISemanticKernelAIProvider
    {
        #region Fields        

        protected ChatHistory? chatHistory = null;
        protected Kernel? kernel = null;
        protected IChatCompletionService? chatCompletionService = null;

        #endregion

        #region Properties

        public override bool Started
        {
            get
            {
                return this.kernel != null;
            }
        }

        #endregion

        #region Methods

        public override void Start()
        {
            IKernelBuilder kernelBuilder = this.InitializeKernelBuilder();
            this.kernel = this.InitializeKernelWithAIService(kernelBuilder);
            this.chatCompletionService = this.InitChatCompletition(this.kernel);
        }

        public override void Stop()
        {
            if (this.chatCompletionService != null)
            {
                this.Logger?.LogTrace("Stopping chat completion service.");
                this.chatCompletionService = null;
            }

            if (this.kernel != null)
            {
                this.Logger?.LogTrace("Stopping kernel.");
                this.kernel = null;
            }
        }

        abstract protected Kernel InitializeKernelWithAIService(IKernelBuilder kernelBuilder);

        /// <summary>
        /// Initializes the kernel builder with the default initialization behavior for plugins and modules for a Semantic Kernel provider.
        /// </summary>
        /// <returns>The initialized kernel builder, with no reference for the underlying AI provider.</returns>
        /// <remarks>The selected AI provider must be initialized in the SemanticKernelAIProvider implementation class of choice.</remarks>
        protected IKernelBuilder InitializeKernelBuilder()
        {
            IKernelBuilder kernelBuilder = Kernel.CreateBuilder();
            IProviderAIModule[] initChatModules;
            IProviderAIModule[] nonInitModules;
            IProviderStateParameters providerStateParameters;

            this.AddDefaultKernelPlugins(kernelBuilder);

            this.chatHistory = [];

            initChatModules = this.Modules.GetInitChatModules()
                                          .ToArray();

            providerStateParameters = this.GetAIProviderStateParameters();

            for (int i = 0; i < initChatModules.Length; i++)
            {
                IInitAgentAIModule initChatModule = (IInitAgentAIModule)initChatModules[i];
                string initChatMessage = initChatModule.BuildInitAgentMessage();

                this.Logger?.LogTrace("Adding init chat message {initChatMessage} to chat history",
                                      initChatMessage);
                initChatModule.OnStart(providerStateParameters);
                this.chatHistory.AddSystemMessage(initChatMessage);
            }

            nonInitModules = this.Modules.GetNonInitChatModules()
                                         .ToArray();


            for (int i = 0; i < nonInitModules.Length; i++)
            {
                IProviderAIModule nonInitModule = nonInitModules[i];
                this.Logger?.LogTrace("Adding non-init chat module {module} to kernel builder",
                                      nonInitModule.GetType().Name);

                nonInitModule.OnStart(providerStateParameters);

                switch (nonInitModule)
                {
                    case ISemanticKernelAIModule semanticKernelModule:
                        this.Logger?.LogInformation("Current module {module} is of type semantic kernel. " +
                                                    "Can be added directly to the semantic kernel builder.",
                                                    semanticKernelModule.GetType().Name);
                        if (semanticKernelModule.RegisterKernelFunctions)
                        {
                            this.Logger?.LogDebug("Module {module} requires kernel functions registration.",
                                                  semanticKernelModule.ModuleName);
                            semanticKernelModule.AddToKernelBuilder(kernelBuilder);
                        }
                        else
                            this.Logger?.LogDebug("Module {module} does not require kernel functions registration.",
                                                  semanticKernelModule.ModuleName);
                        break;

                    default:
                        this.Logger?.LogWarning("Current module {module} is not of a supported type. " +
                                                "Trying to add it as a service to the kernel builder.",
                                                nonInitModule.GetType().Name);
                        kernelBuilder.Services.AddSingleton(nonInitModule.GetType(), nonInitModule);
                        break;
                }
            }

            return kernelBuilder;
        }

        /// <summary>
        /// Initializes the chat completion service for the Semantic Kernel provider.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the kernel is not initialized, the operation will fail.</exception>
        protected virtual IChatCompletionService? InitChatCompletition(Kernel kernel)
        {
            if (kernel == null)
                throw new InvalidOperationException("Semantic Kernel is not initialized.");

            IChatCompletionService? chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
            this.Logger?.LogInformation("OpenAI Semantic Kernel Provider started.");
            return chatCompletionService;
        }        

        protected virtual void AddDefaultKernelPlugins(IKernelBuilder kernelBuilder)
        {
        }

        protected SemanticKernelAIProviderStateParameters GetAIProviderStateParameters()
        {
            return new SemanticKernelAIProviderStateParameters(this.chatHistory);
        }

        #endregion
    }
}
