using AMDevIT.AI.Core.Modules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace AMDevIT.AI.Core.Providers.OpenAI
{
    public class OpenAISemanticKernelProvider(string apiKey,
                                              string model,
                                              ILogger? logger)
        : SemanticKernelAIProvider(apiKey, model, logger)
    {
        #region Consts        

        #endregion

        #region Fields        

        private ChatHistory? chatHistory = null;
        private Kernel? kernel = null;
        private IChatCompletionService? chatCompletionService = null;

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
            IKernelBuilder kernelBuilder = Kernel.CreateBuilder();
            IProviderAIModule[] initChatModules;
            IProviderAIModule[] nonInitModules;

            this.AddDefaultKernelPlugins(kernelBuilder);


            this.chatHistory = [];

            initChatModules = this.Modules.GetInitChatModules()
                                          .ToArray();

            for (int i = 0; i < initChatModules.Length; i++)
            {
                IInitAgentAIModule initChatModule = (IInitAgentAIModule)initChatModules[i];
                string initChatMessage = initChatModule.BuildInitAgentMessage();

                this.Logger?.LogTrace("Adding init chat message {initChatMessage} to chat history",
                                      initChatMessage);
                this.chatHistory.AddSystemMessage(initChatMessage);
            }

            nonInitModules = this.Modules.GetNonInitChatModules()
                                         .ToArray();


            for (int i = 0; i < nonInitModules.Length; i++)
            {
                IProviderAIModule nonInitModule = nonInitModules[i];
                this.Logger?.LogTrace("Adding non-init chat module {module} to kernel builder",
                                      nonInitModule.GetType().Name);

                switch (nonInitModule)
                {
                    case ISemanticKernelAIModule semanticKernelModule:
                        this.Logger?.LogInformation("Current module {module} is of type semantic kernel. " +
                                                    "Can be added directly to the semantic kernel builder.",
                                                    semanticKernelModule.GetType().Name);
                        semanticKernelModule.AddToKernelBuilder(kernelBuilder);
                        break;

                    default:
                        this.Logger?.LogWarning("Current module {module} is not of a supported type. " +
                                                "Trying to add it as a service to the kernel builder.",
                                                nonInitModule.GetType().Name);
                        kernelBuilder.Services.AddSingleton(nonInitModule.GetType(), nonInitModule);
                        break;
                }
            }

            kernelBuilder.AddOpenAIChatCompletion(this.Model, this.APIKey);
            kernel = kernelBuilder.Build();

            this.chatCompletionService = this.kernel.GetRequiredService<IChatCompletionService>();
            this.Logger?.LogInformation("OpenAI Semantic Kernel Provider started.");
        }

        public override void Stop()
        {

        }

        public override async Task<string> RequestGreetingMessageAsync(CancellationToken cancellationToken = default)
        {
            IAsyncEnumerable<StreamingChatMessageContent> streamingMessageContent;
            string assistentCompleteMessage;

            if (this.Started == false)
                throw new InvalidOperationException("OpenAI Semantic Kernel Provider is not started.");

            this.chatHistory ??= [];
            this.chatCompletionService ??= this.kernel!.GetRequiredService<IChatCompletionService>();

            OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
            {
                MaxTokens = this.MaxTokens,
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            };

            assistentCompleteMessage = string.Empty;

            string greetingMessageQuestion = "In base alla tua personalità, presentati e saluta l'utente.";

            this.chatHistory.AddSystemMessage(greetingMessageQuestion);
            this.Logger?.LogTrace("Added greeting system message {question}. " +
                                  "Question added to history as user message.",
                                  greetingMessageQuestion);
            streamingMessageContent = this.chatCompletionService.GetStreamingChatMessageContentsAsync(this.chatHistory,
                                                                                                      executionSettings: openAIPromptExecutionSettings,
                                                                                                      kernel: this.kernel,
                                                                                                      cancellationToken: cancellationToken);
            try
            {
                await foreach (StreamingChatMessageContent? message in streamingMessageContent.WithCancellation(cancellationToken))
                {
                    if (message != null)
                    {
                        this.Logger?.LogTrace("Received streamed message content {message}", message);
                        assistentCompleteMessage += message.Content;
                    }
                }
            }
            catch (Exception exc)
            {
                this.Logger?.LogError(exc, "Error while asking greeting question {question}", greetingMessageQuestion);
                throw;
            }

            this.chatHistory.AddAssistantMessage(assistentCompleteMessage);
            return assistentCompleteMessage;
        }

        public override async Task<string> AskAsync(string question, CancellationToken cancellationToken = default)
        {
            IAsyncEnumerable<StreamingChatMessageContent> streamingMessageContent;
            string assistentCompleteMessage;

            if (this.Started == false)
                throw new InvalidOperationException("OpenAI Semantic Kernel Provider is not started.");

            this.chatHistory ??= [];
            this.chatCompletionService ??= this.kernel!.GetRequiredService<IChatCompletionService>();

            OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
            {
                MaxTokens = this.MaxTokens,
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            };

            assistentCompleteMessage = string.Empty;
            this.chatHistory.AddUserMessage(question);
            this.Logger?.LogTrace("Asking question {question}. Question added to history as user message.", question);
            streamingMessageContent = this.chatCompletionService.GetStreamingChatMessageContentsAsync(this.chatHistory,
                                                                                                      executionSettings: openAIPromptExecutionSettings,
                                                                                                      kernel: this.kernel,
                                                                                                      cancellationToken: cancellationToken);
            try
            {
                await foreach (StreamingChatMessageContent? message in streamingMessageContent.WithCancellation(cancellationToken))
                {
                    if (message != null)
                    {
                        this.Logger?.LogTrace("Received streamed message content {message}", message);
                        assistentCompleteMessage += message.Content;
                    }
                }
            }
            catch (Exception exc)
            {
                this.Logger?.LogError(exc, "Error while asking question {question}", question);
                throw;
            }

            this.chatHistory.AddAssistantMessage(assistentCompleteMessage);
            return assistentCompleteMessage;
        }

        #endregion
    }
}
