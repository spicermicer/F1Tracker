using F1TournamentTracker.Data;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace F1TournamentTracker.ViewModels
{
    internal class TeamManagerViewModel : ViewModelBase
    {
        private ObservableCollection<TeamInfo> _teamData;
        public ObservableCollection<TeamInfo> TeamData
        {
            get => _teamData;
            set => this.RaiseAndSetIfChanged(ref _teamData, value);
        }

        public TeamManagerViewModel(TeamInfo[] teamData)
        {
            TeamData = new(teamData);
        }

    }
}
