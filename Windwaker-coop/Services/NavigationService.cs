using CommunityToolkit.Mvvm.ComponentModel;

namespace Windwaker_coop.Services
{
    public partial class NavigationService : ObservableObject
    {
        [ObservableProperty]
        private object _currentView;

        public void NavigateTo(object viewModel)
        {
            CurrentView = viewModel;
        }
    }
}
