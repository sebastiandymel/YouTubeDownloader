using System;

namespace YTDownloader
{
    public class UserConfiguration
    {
        /// <summary>
        /// Directory to download files
        /// </summary>
        public string DownloadDir { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
    }
}
