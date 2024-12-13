using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AMDevIT.AI.App.WPF.UI.Behaviors
{
    public class EnterKeyBehavior : Behavior<TextBox>
    {
        #region Properties

        // DependencyProperty per il comando
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                nameof(Command),
                typeof(ICommand),
                typeof(EnterKeyBehavior),
                new PropertyMetadata(null));

        // DependencyProperty per il parametro del comando
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(
                nameof(CommandParameter),
                typeof(object),
                typeof(EnterKeyBehavior),
                new PropertyMetadata(null));

        // Comando da eseguire
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        // Parametro da passare al comando
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        #endregion

        #region Methods

        // Associa l'evento al TextBox
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.KeyDown += OnKeyDown;
        }

        // Disassocia l'evento
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.KeyDown -= OnKeyDown;
        }

        #endregion

        #region Event Handlers

        // Esegui il comando quando viene premuto Invio
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Command?.CanExecute(CommandParameter) == true)
            {
                Command.Execute(CommandParameter);
                e.Handled = true; // Evita che il TextBox gestisca ulteriormente l'evento
            }
        }

        #endregion
    }
}
