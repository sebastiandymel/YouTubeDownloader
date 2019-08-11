using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace YTDownloader
{
    public class ViewHost: ContentControl
    {
        private static Dictionary<string, Func<FrameworkElement>> registeredViews = new Dictionary<string, Func<FrameworkElement>>();

        public static void Register(string name, Func<FrameworkElement> factory)
        {
            registeredViews[name] = factory;
        }

        public ViewHost()
        {
            CloseCommand = new SimpleCommand(() => ActivateViewRequest = null);
            Visibility = Visibility.Collapsed;
        }

        public ActivateViewRequest ActivateViewRequest
        {
            get { return (ActivateViewRequest)GetValue(ActivateViewRequestProperty); }
            set { SetValue(ActivateViewRequestProperty, value); }
        }
        public static readonly DependencyProperty ActivateViewRequestProperty =
            DependencyProperty.Register("ActivateViewRequest", typeof(ActivateViewRequest), typeof(ViewHost), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnRequestChanged));

        private static void OnRequestChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ViewHost)d).Update();
        }
               
        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }
        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register("CloseCommand", typeof(ICommand), typeof(ViewHost), new PropertyMetadata(null));

        private void Update()
        {
            if (ActivateViewRequest == null)
            {
                Content = null;
                Visibility = Visibility.Collapsed;
            }
            else
            {
                if (registeredViews.TryGetValue(ActivateViewRequest.Name, out var factory))
                {
                    var view = factory();
                    view.DataContext = ActivateViewRequest.DataContext;
                    Visibility = Visibility.Visible;
                    Content = view;
                }               
            }
        }

        private class SimpleCommand : ICommand
        {
            private Action action;
            public SimpleCommand(Action a)
            {
                this.action = a;
            }
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                this.action();
            }
        }
    }
}
