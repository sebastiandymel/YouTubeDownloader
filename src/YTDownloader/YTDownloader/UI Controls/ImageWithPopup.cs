using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace YTDownloader

{
    public class ImageWithPopup: ContentControl
    {
        public ImageWithPopup()
        {
            MouseLeftButtonUp += OnMouseLeftDown;
        }

        private void OnMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var popup = EmbededPopup;
            if (popup != null)
            {
                Dispatcher.BeginInvoke(new Action(() => popup.IsOpen = true));
            }
        }

        private Popup EmbededPopup
        {
            get
            {
                return Template.FindName("PART_POPUP", this) as Popup;
            }
        }
    }
}
