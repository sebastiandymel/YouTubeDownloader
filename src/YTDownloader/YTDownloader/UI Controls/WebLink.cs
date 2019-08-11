using System;
using System.Diagnostics;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace YTDownloader

{
    public class WebLink : Hyperlink
    {
        public WebLink()
        {            
            RequestNavigate += OnNavigate;
        }

        private void OnNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start("chrome.exe", e.Uri.AbsoluteUri);
            }
            catch
            {
                try
                {
                    Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
                }
                catch
                {
                    Process.Start("IExplore.exe", e.Uri.AbsoluteUri);
                }
            }
            
            e.Handled = true;
        }
    }
}
