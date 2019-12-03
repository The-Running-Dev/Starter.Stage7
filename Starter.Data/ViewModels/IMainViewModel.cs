using System.Windows.Input;
using System.ComponentModel;

namespace Starter.Data.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMainViewModel : INotifyPropertyChanged
    {
        ICommand CreateCommand { get; set; }

        ICommand SaveCommand { get; set; }

        ICommand DeleteCommand { get; set; }

        ICommand CancelCommand { get; set; }
    }
}