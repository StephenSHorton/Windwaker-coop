using CommunityToolkit.Mvvm.ComponentModel;
using Windwaker_coop.Services;

namespace Windwaker_coop.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly NavigationService _navigation = new();

        [ObservableProperty]
        private string _windowTitle = "The Legend of Zelda Co-op";

        public object CurrentView => _navigation.CurrentView;

        public MainViewModel()
        {
            _navigation.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(NavigationService.CurrentView))
                    OnPropertyChanged(nameof(CurrentView));
            };

            _navigation.NavigateTo(new GameSelectionViewModel(_navigation));
        }

        public void UpdateTitle(string subtitle)
        {
            WindowTitle = string.IsNullOrEmpty(subtitle)
                ? "The Legend of Zelda Co-op"
                : $"The Legend of Zelda Co-op - {subtitle}";
        }
    }
}
