using AMDevIT.AI.App.WPF.Models;
using AMDevIT.AI.Core;
using AMDevIT.AI.Core.Modules.Personality;
using AMDevIT.AI.Core.Modules.Utils;
using AMDevIT.AI.Core.Providers.OpenAI;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace AMDevIT.AI.App.WPF.ViewModels;

public partial class MainWindowViewModel(ILogger<MainWindowViewModel> logger,
                                      IConfiguration configuration)
 : ViewModelBase(logger, configuration), IAViewModel
{
    #region Fields

    private readonly ObservableCollection<ChatMessage> chatMessages = [];
    private string? currentMessageText;

    #endregion

    #region Commands

    private AsyncRelayCommand? sendCommand;

    public AsyncRelayCommand SendMessageCommand
    {
        get => this.sendCommand ??= new AsyncRelayCommand(this.SendMessageAsync, this.CanSendMessage);
    }

    #endregion

    #region Properties   

    public ISemanticKernelAIProvider? SemanticProvider
    {
        get;
        protected set;
    }

    public ObservableCollection<ChatMessage> ChatMessages
    {
        get => this.chatMessages;
    }

    public string? CurrentMessageText
    {
        get => this.currentMessageText;
        set
        {
            if (this.SetProperty(ref this.currentMessageText, value))
            {
                this.SendMessageCommand.NotifyCanExecuteChanged();
            }
        }
    }

    #endregion

    #region Methods

    public void InitializeIAProvider()
    {
        SemanticKernelAIProviderBuilder semanticKernelProviderBuilder = new();
        ISemanticKernelAIProvider semanticKernelProvider;
        PersonalityAIModule personalityModule;
        PersonalityAnswerMode personalityAnswerMode;

        IConfiguration aiConfiguration = this.Configuration.GetSection("ai");
        string? apiKey = this.Configuration["openAIKey"];
        string? aiModel = aiConfiguration["model"];
        string? personalityName = aiConfiguration["name"];
        string? personality = aiConfiguration["personality"];
        string? adjectivesGroup = aiConfiguration["adjectives"];

        if (string.IsNullOrEmpty(apiKey))
        {
            this.Logger.LogError("API Key is missing from configuration");
            return;
        }

        semanticKernelProviderBuilder.AddLogger(this.Logger);
        semanticKernelProviderBuilder.AddAPIKey(apiKey);

        if (string.IsNullOrWhiteSpace(personalityName))
            personalityName = "Agent 74";

        if (string.IsNullOrWhiteSpace(personality))
            personalityAnswerMode = PersonalityAnswerMode.Neutral;
        else
            if (!Enum.TryParse(personality, true, out personalityAnswerMode))
            personalityAnswerMode = PersonalityAnswerMode.Neutral;


        personalityModule = PersonalityAIModule.FromCommaSeparatedAdjectives(personalityName,
                                                                             adjectivesGroup,
                                                                             personalityAnswerMode,
                                                                             this.Logger);
        semanticKernelProviderBuilder.AddModule(personalityModule);

        if (!string.IsNullOrEmpty(aiModel))
            semanticKernelProviderBuilder.AddAIModelID(aiModel);
        else
            this.Logger?.LogWarning("AI Model ID is missing from configuration");

        semanticKernelProviderBuilder.AddModule(new TimeAIModule(this.Logger));

        semanticKernelProvider = semanticKernelProviderBuilder.BuildOpenAISemanticKernelAIProvider();
        this.SemanticProvider = semanticKernelProvider;
    }

    public async Task RequestGreetingMessageAsync(CancellationToken cancellationToken = default)
    {
        string greetingMessage;

        if (this.SemanticProvider == null)
        {
            this.Logger.LogError("Semantic Provider is not initialized");
            return;
        }

        if (this.SemanticProvider.Started == false)
        {
            this.Logger.LogWarning("Semantic Provider is not started");
            this.SemanticProvider.Start();
        }

        greetingMessage = await this.SemanticProvider.RequestGreetingMessageAsync(cancellationToken);
        this.ChatMessages.Add(new ChatMessage { Text = greetingMessage, IsSentByMe = false, AvatarColor = "Salmon" });
    }

    private bool CanSendMessage()
    {
        return this.SemanticProvider != null && !string.IsNullOrWhiteSpace(this.currentMessageText);
    }

    private async Task SendMessageAsync(CancellationToken cancellationToken = default)
    {
        string messageReceived;

        if (this.SemanticProvider == null)
        {
            this.Logger.LogError("Semantic Provider is not initialized");
            return;
        }

        if (string.IsNullOrWhiteSpace(this.CurrentMessageText))
        {
            this.Logger.LogWarning("Message text is empty");
            return;
        }

        if (this.SemanticProvider.Started == false)
        {
            this.Logger.LogWarning("Semantic Provider is not started");
            this.SemanticProvider.Start();
        }

        this.ChatMessages.Add(new ChatMessage { Text = this.CurrentMessageText, IsSentByMe = true, AvatarColor = "Blue" });

        messageReceived = await this.SemanticProvider.AskAsync(this.CurrentMessageText, cancellationToken);
        this.CurrentMessageText = null;

        this.ChatMessages.Add(new ChatMessage { Text = messageReceived, IsSentByMe = false, AvatarColor = "Salmon" });
    }

    #endregion
}
