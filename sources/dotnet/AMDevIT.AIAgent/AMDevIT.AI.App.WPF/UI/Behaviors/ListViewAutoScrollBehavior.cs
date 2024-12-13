using Microsoft.Xaml.Behaviors;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace AMDevIT.AI.App.WPF.UI.Behaviors
{
    public class ListViewAutoScrollBehavior
       : Behavior<ListView>
    {
        #region Properties

        // Collezione osservabile da monitorare
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource),
                                        typeof(INotifyCollectionChanged),
                                        typeof(ListViewAutoScrollBehavior),
                                        new PropertyMetadata(null, OnItemsSourceChanged));

        public INotifyCollectionChanged ItemsSource
        {
            get => (INotifyCollectionChanged)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        #endregion

        #region Methods

        // Metodo chiamato quando cambia ItemsSource
        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListViewAutoScrollBehavior behavior)
            {
                if (e.OldValue is INotifyCollectionChanged oldCollection)
                {
                    oldCollection.CollectionChanged -= behavior.OnCollectionChanged;
                }

                if (e.NewValue is INotifyCollectionChanged newCollection)
                {
                    newCollection.CollectionChanged += behavior.OnCollectionChanged;
                }
            }
        }

        // Metodo per gestire il cambiamento della collezione
        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && AssociatedObject != null)
            {
                // Scorri all'ultimo elemento
                AssociatedObject.ScrollIntoView(AssociatedObject.Items[^1]);
            }
        }

        #endregion
    }
}
