using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace YTDownloader

{
    public class CustomItemsControl: ItemsControl
    {
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            if (e.Action == NotifyCollectionChangedAction.Add && ItemAdded != null && e.NewItems.Count == 1)
            {
                if (e.NewItems[0] is IRemovable removable)
                {
                    removable.RemoveRequested += OnRemoveRequested;
                }
                var container = ItemContainerGenerator.ContainerFromItem(e.NewItems[0]) as ContentPresenter;
                if (container.IsLoaded)
                {
                    container.BeginStoryboard(ItemAdded, HandoffBehavior.SnapshotAndReplace);
                }
                else
                {
                    container.Loaded += (s, e) =>
                    {
                        var c = (s as ContentPresenter);
                        var target = VisualTreeHelper.GetChild(c, 0) as FrameworkElement;
                        target.BeginStoryboard(ItemAdded, HandoffBehavior.SnapshotAndReplace);
                    };
                }                
            }            
        }

        private void OnRemoveRequested(object sender, EventArgs e)
        {
            var removable = (IRemovable)sender;
            removable.RemoveRequested -= OnRemoveRequested;

            if (ItemRemoved != null)
            {
                var container = ItemContainerGenerator.ContainerFromItem(sender) as ContentPresenter;
                var target = VisualTreeHelper.GetChild(container, 0) as FrameworkElement;
                ItemRemoved.Completed += (s, e) =>
                {
                    var collection = (ItemsSource as ObservableCollection<Video>);
                    if (collection != null && sender is Video di)
                    {
                        collection.Remove(di);
                    }
                };
                target.BeginStoryboard(ItemRemoved, HandoffBehavior.SnapshotAndReplace);
            }
        }

        public Storyboard ItemAdded
        {
            get { return (Storyboard)GetValue(ItemAddedProperty); }
            set { SetValue(ItemAddedProperty, value); }
        }
        public static readonly DependencyProperty ItemAddedProperty = DependencyProperty.Register("ItemAdded", typeof(Storyboard), typeof(CustomItemsControl), new PropertyMetadata(null));

        public Storyboard ItemRemoved
        {
            get { return (Storyboard)GetValue(ItemRemovedProperty); }
            set { SetValue(ItemRemovedProperty, value); }
        }
        public static readonly DependencyProperty ItemRemovedProperty = DependencyProperty.Register("ItemRemoved", typeof(Storyboard), typeof(CustomItemsControl), new PropertyMetadata(null));
    }
}
