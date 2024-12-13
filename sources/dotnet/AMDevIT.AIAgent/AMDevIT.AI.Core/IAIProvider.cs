using AMDevIT.AI.Core.Modules;

namespace AMDevIT.AI.Core
{
    public interface IAIProvider
    {
        #region Properties

        bool Started
        {
            get;
        }

        ModuleManager Modules
        {
            get;
        }

        #endregion

        #region Methods

        void Start();
        void Stop();

        Task<string> RequestGreetingMessageAsync(CancellationToken cancellationToken = default);
        Task<string> AskAsync(string question, CancellationToken cancellationToken = default);

        #endregion
    }
}
