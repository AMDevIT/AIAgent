using AMDevIT.AI.Core.Modules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Diagnostics;

namespace AMDevIT.AI.Core.Providers.OpenAI
{
    public class OpenAISemanticKernelProvider(string apiKey,
                                              string model,
                                              ILogger? logger)
        : SemanticKernelAIProvider(apiKey, model, logger)
    {
        #region Consts        

        #endregion

        #region Methods           

        override protected Kernel InitializeKernelWithAIService(IKernelBuilder kernelBuilder)
        {
            Kernel kernel;

            kernelBuilder.AddOpenAIChatCompletion(this.Model, this.APIKey);
            kernel = kernelBuilder.Build();

            return kernel;
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
