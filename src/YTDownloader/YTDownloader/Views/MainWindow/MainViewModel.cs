using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using YTDownloader.Wpf;

namespace YTDownloader
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool canExecuteFind = true;
        private string url;
        private string validationError;
        private UserConfiguration userConfiguration = new UserConfiguration();
        private ActivateViewRequest overlay;

        public MainViewModel()
        {
            FindCommand = new UiCommand(ExecuteFind, () => this.canExecuteFind && !string.IsNullOrWhiteSpace(Url));
            OpenSettingsCommand = new UiCommand(OpenSettings, () => true);
        }

        public UiCommand OpenSettingsCommand { get; }
        public UiCommand FindCommand { get; }
        public string Url
        {
            get => url;
            set
            {
                url = value;
                OnPropertyChanged();
            }
        }
        public string ValidationError
        {
            get => validationError;
            set
            {
                validationError = value;
                OnPropertyChanged();
            }
        }
        public ActivateViewRequest Overlay
        {
            get => this.overlay;
            set { this.overlay = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Video> Items { get; } = new ObservableCollection<Video>();
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private async Task ExecuteFind()
        {
            NotifyError(null);

            this.canExecuteFind = false;
            FindCommand.Refresh();
            var youtubeVideo = new Video(
                    (i, j) => new DownloadItem(j, i.Name, this.userConfiguration),
                    Url,
                    i => Items.Remove(i));
            await youtubeVideo.FindDownaloads();
            if (youtubeVideo.AvailableDownloads.Count == 0)
            {
                NotifyError("No video files found. Check link and try again.");
            }
            else
            {
                Items.Add(youtubeVideo);
            }

            this.canExecuteFind = true;
            FindCommand.Refresh();
        }

        private void NotifyError(string msg)
        {
            ValidationError = msg;
        }

        private async Task OpenSettings()
        {
            Overlay = new ActivateViewRequest
            {
                Name = "UserSettings",
                DataContext = new UserSettingsViewModel(this.userConfiguration)
            };
        }
    }
}
