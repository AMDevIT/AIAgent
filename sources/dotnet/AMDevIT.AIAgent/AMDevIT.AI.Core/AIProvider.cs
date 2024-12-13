using AMDevIT.AI.Core.Modules;
using Microsoft.Extensions.Logging;

namespace AMDevIT.AI.Core;

public abstract class AIProvider(string apiKey,
                                 string model,
                                 ILogger? logger)
   : IAIProvider
{
    #region Consts

    public const int DefaultMaxTokens = 1000;

    #endregion

    #region Fields

    private readonly ModuleManager moduleManager = new(logger);

    private readonly string apiKey = apiKey;

    #endregion

    #region Properties

    public abstract bool Started
    {
        get;
    }

    protected ILogger? Logger
    {
        get;
        private set;
    } = logger;

    protected string APIKey
    {
        get
        {
            return this.apiKey;
        }
    }

    public int MaxTokens
    {
        get;
        set;
    } = DefaultMaxTokens;

    public string Model
    {
        get;
        protected set;
    } = model;

    public ModuleManager Modules
    {
        get
        {
            return this.moduleManager;
        }
    }

    #endregion        

    #region Methods

    public abstract void Start();
    public abstract void Stop();

    public abstract Task<string> RequestGreetingMessageAsync(CancellationToken cancellationToken = default);

    public abstract Task<string> AskAsync(string question, CancellationToken cancellationToken = default);       

    #endregion
}
