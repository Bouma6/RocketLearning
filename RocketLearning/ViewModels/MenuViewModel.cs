using System.Windows.Input;

namespace RocketLearning.ViewModels;

public class MenuViewModel(MainWindowViewModel main) : ViewModelBase
{
    public ICommand StartCommand { get; } = new RelayCommand(() => main.CurrentView = new GameViewModel(main));
}