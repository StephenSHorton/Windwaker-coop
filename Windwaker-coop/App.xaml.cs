using System.Windows;

namespace Windwaker_coop
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Program.Initialize();
        }
    }
}
