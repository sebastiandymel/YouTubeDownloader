using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace YTDownloader.Engine
{
    public interface IYoutubeVideo<T>
    {
        ObservableCollection<T> AvailableDownloads { get; }
        string BaseUrl { get; }
        string Name { get; }

        Task FindDownaloads();
    }
}