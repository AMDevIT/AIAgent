using Microsoft.Extensions.Logging;

namespace AMDevIT.AI.Core.Modules
{
    public interface IProviderAIModule
    {
        ILogger? Logger
        {
            get;
        }

        string ModuleName
        {
            get;
        }
    }
}
