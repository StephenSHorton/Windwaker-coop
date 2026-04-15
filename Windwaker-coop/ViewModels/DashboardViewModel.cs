using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Windwaker_coop.Models;
using Windwaker_coop.Services;

namespace Windwaker_coop.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        private readonly NavigationService _navigation;
        private readonly DispatcherTimer _statusTimer;

        [ObservableProperty]
        private string _commandInput = "";

        [ObservableProperty]
        private string _statusText = "Connected";

        [ObservableProperty]
        private string _connectionInfo = "";

        [ObservableProperty]
        private bool _isSyncing;

        [ObservableProperty]
        private bool _isServerMode;

        public ObservableCollection<LogEntry> LogEntries => LogService.Instance.LogEntries;

        public ObservableCollection<string> ConnectedPlayers { get; } = new();

        public string GameName => Program.currGame?.gameName ?? "Unknown";

        public DashboardViewModel(NavigationService navigation, bool isServer)
        {
            _navigation = navigation;
            IsServerMode = isServer;

            string role = isServer ? "Server" : "Client";
            ConnectionInfo = $"{role} - {Program.currGame?.gameName}";

            // Poll status every second
            _statusTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _statusTimer.Tick += UpdateStatus;
            _statusTimer.Start();
        }

        private void UpdateStatus(object sender, EventArgs e)
        {
            IsSyncing = Program.programSyncing;
            StatusText = IsSyncing ? "Syncing" : "Connected";

            if (IsServerMode && Program.currUser is Server server)
            {
                ConnectedPlayers.Clear();
                foreach (var kvp in server.clientIps)
                    ConnectedPlayers.Add($"{kvp.Value} ({kvp.Key})");
            }
        }

        [RelayCommand]
        private void SendCommand()
        {
            if (string.IsNullOrWhiteSpace(CommandInput)) return;

            string input = CommandInput.Trim();
            CommandInput = "";

            Output.text("> " + input, ConsoleColor.Yellow);
            string response = Program.ProcessCommand(input);
            if (!string.IsNullOrEmpty(response))
                Output.text(response, ConsoleColor.Yellow);
        }

        [RelayCommand]
        private void Stop()
        {
            _statusTimer.Stop();
            Program.Shutdown();
            StatusText = "Disconnected";
            Output.text("Disconnected.", ConsoleColor.Gray);
        }
    }
}
