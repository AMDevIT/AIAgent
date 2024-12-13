namespace AMDevIT.AI.Core.Modules
{
    public interface IInitAgentAIModule
       : IProviderAIModule
    {
        #region Methods

        string BuildInitAgentMessage();

        #endregion
    }
}
