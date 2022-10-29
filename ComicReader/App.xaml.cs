using ComicReader.Services;
using ComicReader.Views;
using OlyD.Services;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ComicReader
{
    sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
            await ActivatedAsync(e);
        }

        protected async override void OnActivated(IActivatedEventArgs e)
        {
            await ActivatedAsync(e);
        }

        private async Task ActivatedAsync(IActivatedEventArgs e)
        {
            var activationInfo = ActivationService.GetActivationInfo(e);

            if (Window.Current.Content is Frame frame)
            {
                var navigationService = ServiceLocator.Current.GetService<INavigationService>();
                await navigationService.CreateNewViewAsync(activationInfo.EntryViewModel, activationInfo.EntryArgs);
            }
            else
            {
                bool loadState = e.PreviousExecutionState == ApplicationExecutionState.Terminated;
                ExtendedSplash extendedSplash = new ExtendedSplash(e, loadState);
                Window.Current.Content = extendedSplash;
                Window.Current.Activate();

                extendedSplash.LoadDataAsync(e); // 加载数据
            }
        }
    }
}