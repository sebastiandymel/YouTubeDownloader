using System.Windows;
using System.Windows.Controls;

namespace YTDownloader

{
    public class PathButton: Button
    {
        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }
        public static readonly DependencyProperty PathProperty = DependencyProperty.Register(
            "Path", 
            typeof(string), 
            typeof(PathButton), 
            new PropertyMetadata(null, OnPathChanged));

        private static void OnPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PathButton)d).UpdatePath();
        }

        private void UpdatePath()
        {
            if (!string.IsNullOrEmpty(Path))
            {
                var res = TryFindResource(Path);
                if (res != null)
                {
                    Content = res;
                }
            }
        }
    }
}
