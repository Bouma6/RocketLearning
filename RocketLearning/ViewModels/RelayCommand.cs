using System;
using System.Windows.Input;

namespace RocketLearning.ViewModels;

public class RelayCommand(Action execute, Func<bool>? canExecute = null) : ICommand
{
    public bool CanExecute(object? parameter) => canExecute?.Invoke() ?? true;
    public void Execute(object? parameter) => execute();

    public event EventHandler? CanExecuteChanged;
}