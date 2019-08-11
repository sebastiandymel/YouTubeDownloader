using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace YTDownloader
{
    public class UiCommand : ICommand
    {
        private readonly Func<Task> toExecute;
        private readonly Func<bool> canExecute;

        public UiCommand(Func<Task> toExecute, Func<bool> canExecute)
        {
            this.toExecute = toExecute;
            this.canExecute = canExecute;
            CommandManager.RequerySuggested += (s, e) => CanExecuteChanged(this, EventArgs.Empty);
        }
        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            return this.canExecute();
        }

        public async void Execute(object parameter)
        {
            await this.toExecute();
        }

        public void Refresh()
        {            
            CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
