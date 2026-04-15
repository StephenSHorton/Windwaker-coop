using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Windwaker_coop.Services;

namespace Windwaker_coop.ViewModels
{
    public partial class ConnectionSetupViewModel : ObservableObject
    {
        private readonly NavigationService _navigation;

        [ObservableProperty]
        private bool _isServer = true;

        [ObservableProperty]
        private string _ipAddress = "";

        [ObservableProperty]
        private string _playerName = "";

        [ObservableProperty]
        private string _errorMessage = "";

        [ObservableProperty]
        private bool _isConnecting;

        public ObservableCollection<string> LocalIpAddresses { get; } = new();

        public string GameName => Program.currGame?.gameName ?? "Unknown";

        public ConnectionSetupViewModel(NavigationService navigation)
        {
            _navigation = navigation;

            // Load local IPs
            var ips = Program.GetLocalIpAddresses();
            foreach (var ip in ips)
                LocalIpAddresses.Add(ip);

            if (LocalIpAddresses.Count > 0)
                IpAddress = LocalIpAddresses[0];
        }

        [RelayCommand]
        private void SelectLocalIp(string ip)
        {
            if (!string.IsNullOrEmpty(ip))
                IpAddress = ip;
        }

        [RelayCommand]
        private async Task ConnectAsync()
        {
            ErrorMessage = "";

            // Validate IP
            if (string.IsNullOrWhiteSpace(IpAddress))
            {
                ErrorMessage = "Please enter an IP address.";
                return;
            }

            // Validate player name for client
            if (!IsServer)
            {
                if (string.IsNullOrWhiteSpace(PlayerName) || PlayerName.Length > 20 ||
                    PlayerName.Contains(' ') || PlayerName.Contains('~'))
                {
                    ErrorMessage = "Player name must be 1-20 characters, no spaces or '~'.";
                    return;
                }
            }

            IsConnecting = true;
            ErrorMessage = "";

            try
            {
                await Task.Run(() =>
                {
                    if (IsServer)
                    {
                        Program.StartAsServer(IpAddress);
                    }
                    else
                    {
                        Program.StartAsClient(IpAddress, PlayerName);
                    }
                });

                string role = IsServer ? "Server" : "Client";
                _navigation.NavigateTo(new DashboardViewModel(_navigation, IsServer));
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Connection failed: {ex.Message}";
                IsConnecting = false;
            }
        }

        [RelayCommand]
        private void Back()
        {
            _navigation.NavigateTo(new GameSelectionViewModel(_navigation));
        }
    }
}
