using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Windwaker_coop.Models;
using Windwaker_coop.Services;

namespace Windwaker_coop.ViewModels
{
    public partial class GameSelectionViewModel : ObservableObject
    {
        private readonly NavigationService _navigation;

        public ObservableCollection<GameInfo> Games { get; } = new();

        [ObservableProperty]
        private GameInfo _selectedGame;

        public GameSelectionViewModel(NavigationService navigation)
        {
            _navigation = navigation;

            if (Program.games != null)
            {
                foreach (var game in Program.games)
                {
                    Games.Add(new GameInfo(game.gameId, game.gameName, game.processName));
                }
            }

            // Pre-select the configured game
            if (Games.Count > Program.config.gameId)
                SelectedGame = Games[Program.config.gameId];
        }

        [RelayCommand]
        private void SelectGame(GameInfo game)
        {
            if (game == null) return;

            SelectedGame = game;
            Program.SelectGame(game.GameId);
            _navigation.NavigateTo(new ConnectionSetupViewModel(_navigation));
        }
    }
}
