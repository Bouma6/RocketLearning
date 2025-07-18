using System.ComponentModel;

namespace RocketLearning.ViewModels;

public class TrainingProgressViewModel : ViewModelBase
{
    private double _progress;
    public double Progress
    {
        get => _progress;
        set => SetProperty(ref _progress, value);
    }
}
