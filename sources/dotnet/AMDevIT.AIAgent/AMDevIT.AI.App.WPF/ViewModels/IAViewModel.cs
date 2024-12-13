using AMDevIT.AI.Core;
using CommunityToolkit.Mvvm.Input;

namespace AMDevIT.AI.App.WPF.ViewModels
{
    public interface IAViewModel
    {
        #region Commands

        AsyncRelayCommand SendMessageCommand
        {
            get;
        }

        #endregion

        #region Properties

        string? CurrentMessageText
        {
            get;
            set;
        }

        ISemanticKernelAIProvider? SemanticProvider
        {
            get;
        }

        #endregion

        #region Methods

        void InitializeIAProvider();
        Task RequestGreetingMessageAsync(CancellationToken cancellationToken = default);

        #endregion
    }
}
