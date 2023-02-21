using CommunityToolkit.Mvvm.ComponentModel;

namespace JAppParty.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private int scoreTeamA;

    [ObservableProperty]
    private int scoreTeamB;

    [ObservableProperty]
    private string name;

    public MainViewModel()
    {
        scoreTeamA = 0;
        scoreTeamB = 0;

        name = "cool breeze";
    }

}
