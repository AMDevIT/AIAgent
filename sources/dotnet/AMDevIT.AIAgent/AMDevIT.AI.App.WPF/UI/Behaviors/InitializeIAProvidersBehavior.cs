using AMDevIT.AI.App.WPF.ViewModels;
using Microsoft.Xaml.Behaviors;
using System.Windows;

namespace AMDevIT.AI.App.WPF.UI.Behaviors
{
    public class InitializeIAProvidersBehavior
       : Behavior<Window>
    {
        #region Methods

        protected override void OnAttached()
        {
            base.OnAttached();

            if (this.AssociatedObject != null)
                this.AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (this.AssociatedObject != null)
                this.AssociatedObject.Loaded -= AssociatedObject_Loaded;
        }

        #endregion

        #region Events

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.AssociatedObject.DataContext != null &&
                this.AssociatedObject.DataContext is IAViewModel iaViewModel)
            {
                iaViewModel.InitializeIAProvider();
                Task greetingsMessageTask = iaViewModel.RequestGreetingMessageAsync();
                greetingsMessageTask.ContinueWith((taskResult) =>
                {
                    if (taskResult.IsCompletedSuccessfully)
                    {
                        // Do nothing
                    }
                });
            }
        }

        #endregion
    }
}
