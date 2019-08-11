using YTDownloader.Wpf;

namespace YTDownloader
{
    public static class ViewsRegistration
    {
        public static void Run()
        {
            ViewHost.Register("UserSettings", () => new UserSettings());
        }
    }
}
