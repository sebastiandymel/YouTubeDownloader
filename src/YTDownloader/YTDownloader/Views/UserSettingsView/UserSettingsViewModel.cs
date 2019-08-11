using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace YTDownloader.Wpf
{
    public class UserSettingsViewModel: INotifyPropertyChanged
    {
        private readonly UserConfiguration config;

        public UserSettingsViewModel(UserConfiguration config)
        {
            this.config = config;
            DownloadDirectory = config.DownloadDir;            
            BrowseCommand = new UiCommand(Browse, () => true);
        }

        public UiCommand BrowseCommand { get; private set; }
        private string path;
        public string DownloadDirectory
        {
            get => this.path;
            set
            {
                this.path = value;
                OnPropertyChanged();
                this.config.DownloadDir = value;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private async Task Browse()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.InitialDirectory = Assembly.GetEntryAssembly().Location; // Use current value for initial dir
            dialog.Title = "Select a Directory"; // instead of default "Save As"
            dialog.Filter = "Directory|*.this.directory"; // Prevents displaying files
            dialog.FileName = "select"; // Filename will then be "select.this.directory"
            if (dialog.ShowDialog() == true)
            {
                string path = dialog.FileName;
                // Remove fake filename from resulting path
                path = path.Replace("\\select.this.directory", "");
                path = path.Replace(".this.directory", "");
                // If user has changed the filename, create the new directory
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                DownloadDirectory = path;
            }
        }
    }
}
