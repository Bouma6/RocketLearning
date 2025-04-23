using Avalonia.Controls;
using Avalonia.Input;
using RocketLearning.ViewModels;

namespace RocketLearning.Views;

public partial class GameView : UserControl
{
    public GameView()
    {
        InitializeComponent();
        Focus();
    }

    private void UserControl_KeyDown(object? sender, KeyEventArgs e)
    {
        if (DataContext is GameViewModel vm)
        {
            vm.OnKeyPressed(e);
        }
    }
}