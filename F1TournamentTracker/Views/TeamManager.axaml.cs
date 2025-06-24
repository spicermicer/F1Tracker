using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using F1TournamentTracker.Data;
using F1TournamentTracker.ViewModels;

namespace F1TournamentTracker.Views;

internal partial class TeamManager : Window
{
    public TeamManager(TeamInfo[] teamData)
    {
        InitializeComponent();

        DataContext = new TeamManagerViewModel(teamData);
    }
}