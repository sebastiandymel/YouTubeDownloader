using System;
using System.Net;
using System.Threading.Tasks;

namespace YTDownloader.Engine
{
    public class DownloadJob
    {
        private readonly string internalUrl;

        public DownloadJob(string internalUrl, Quality quality)
        {
            this.internalUrl = internalUrl;
            VideoQuality = quality;
        }

        public Quality VideoQuality { get; } 
        public int DownloadProgress { get; private set; }
        public double CurrentBitRateKbps { get; private set; }
        public event EventHandler DownloadProgressChanged = delegate { };       
        public bool IsBusy { get; private set; }
        public event EventHandler IsBusyChanged = delegate { };

        public async Task Download(string targetPath)
        {
            IsBusy = true;
            IsBusyChanged(this, EventArgs.Empty);
            DownloadProgress = 0;
            WebClient webClient = new WebClient();
            this.downloadStartedTime = DateTime.Now;
            webClient.DownloadProgressChanged += OnDownloadProgress;
            await webClient.DownloadFileTaskAsync(this.internalUrl, targetPath);
            webClient.DownloadProgressChanged -= OnDownloadProgress;
            IsBusy = false;
            IsBusyChanged(this, EventArgs.Empty);
        }

        private void OnDownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            var diff = DateTime.Now - this.downloadStartedTime;
            var br = (e.BytesReceived / 1024.0) / diff.Seconds;
            CurrentBitRateKbps = br;
            DownloadProgress = e.ProgressPercentage;
            DownloadProgressChanged(this, EventArgs.Empty);            
        }

        private DateTime downloadStartedTime;
    }
}
