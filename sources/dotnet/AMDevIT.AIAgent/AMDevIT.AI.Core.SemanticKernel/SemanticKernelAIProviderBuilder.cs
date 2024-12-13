using AMDevIT.AI.Core.Modules;
using Microsoft.Extensions.Logging;

namespace AMDevIT.AI.Core;

public class SemanticKernelAIProviderBuilder
{
    #region Fields

    private readonly List<IProviderAIModule> modules = [];

    #endregion

    #region Properties

    public string? APIKey
    {
        get;
        set;
    }

    public string? ModelID
    {
        get;
        set;
    }

    public ILogger? Logger
    {
        get;
        set;
    }

    public IList<IProviderAIModule> Modules
    {
        get => this.modules;
    }

    #endregion

    #region Methods

    public SemanticKernelAIProviderBuilder AddAPIKey(string apiKey)
    {
        this.APIKey = apiKey;
        return this;
    }

    public SemanticKernelAIProviderBuilder AddAIModelID(string modelID)
    {
        this.ModelID = modelID;
        return this;
    }

    public SemanticKernelAIProviderBuilder AddLogger(ILogger logger)
    {
        this.Logger = logger;
        return this;
    }

    public SemanticKernelAIProviderBuilder AddModule(IProviderAIModule module)
    {
        this.modules.Add(module);
        return this;
    }

    public override string ToString()
    {
        return $"APIKey: {this.APIKey}, ModelID: {this.ModelID}";
    }

    #endregion        
}
