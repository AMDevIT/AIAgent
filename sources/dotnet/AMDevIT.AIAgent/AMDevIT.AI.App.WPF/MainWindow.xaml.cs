using AMDevIT.AI.App.WPF.Runtime;
using AMDevIT.AI.App.WPF.Runtime.Messaging;
using AMDevIT.AI.App.WPF.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace AMDevIT.AI.App.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties

        public ILogger? Logger
        {
            get;
            private set;
        }

        #endregion

        #region .ctor

        public MainWindow()
        {
            MainWindowViewModel? viewModel = ServicesHelper.GetService<MainWindowViewModel>();
            ILogger<MainWindowViewModel>? logger = ServicesHelper.GetService<ILogger<MainWindowViewModel>>();

            InitializeComponent();

            this.DataContext = viewModel;
            this.Logger = logger;               
        }

        #endregion

        #region Methods

        protected void RegisterMessageHandlers()
        {
            try
            {
                WeakReferenceMessenger.Default.Register<ShowTextActionMessage>(this, async (r, m) =>
                {
                    try
                    {
                        await this.OnShowTextActionMessage(r, m);
                    }
                    catch (Exception exc)
                    {
                        this.Logger?.LogError(exc, "Error handling ShowTextActionMessage.");
                    }
                });
            }
            catch (Exception exc)
            {
                this.Logger?.LogError(exc, "Error registering show text message handler.");
            }         
        }

        protected void UnregisterMessageHandlers()
        {
            try
            {
                WeakReferenceMessenger.Default.Unregister<ShowTextActionMessage>(this);
            }
            catch (Exception exc)
            {
                this.Logger?.LogError(exc, "Error unregistering show text message handler.");
            }            
        }

        protected virtual Task OnShowTextActionMessage(object sender, ShowTextActionMessage message)
        {
            MessageBoxImage messageBoxImage = message.MessageDialogType switch { MessageDialogType.Information => MessageBoxImage.Information,
                MessageDialogType.Warning => MessageBoxImage.Warning,
                MessageDialogType.Error => MessageBoxImage.Error,
                _ => MessageBoxImage.None
            };

            try
            {
                MessageBox.Show(this,
                                message.Text,
                                message.Title,
                                MessageBoxButton.OK,
                                messageBoxImage);
            }
            catch(Exception exc)
            {
                this.Logger?.LogError(exc, "Error showing message box for ShowTextActionMessage.");
                return Task.FromException(exc);
            }

            return Task.CompletedTask;
        }

        #endregion

        #region Event Handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.RegisterMessageHandlers();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            this.UnregisterMessageHandlers();
        }

        #endregion
    }
}