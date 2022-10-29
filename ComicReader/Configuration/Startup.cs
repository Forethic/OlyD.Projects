using ComicReader.Services;
using ComicReader.ViewModels;
using ComicReader.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace ComicReader
{
    public static class Startup
    {
        private static readonly ServiceCollection _serviceCollection = new ServiceCollection();

        public static async Task ConfigureAsync()
        {
            ServiceLocator.Configure(_serviceCollection);

            ConfigureNavigation();
        }

        private static void ConfigureNavigation()
        {
            NavigationService.Register<ShellViewModel, ShellView>();
            NavigationService.Register<MainShellViewModel, MainShellView>();

            NavigationService.Register<SettingsViewModel, SettingsView>();
        }
    }
}