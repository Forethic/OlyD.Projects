using ComicReader.Services;
using ComicReader.ViewModels;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ComicReader.Views
{
    public sealed partial class ExtendedSplash : Page
    {
        private Frame _rootFrame;
        private Rect _splashImageRect;
        private SplashScreen _splashScreen;
        private IActivatedEventArgs _activedEventArgs;

        public ExtendedSplash(IActivatedEventArgs e, bool loadState)
        {
            InitializeComponent();

            Window.Current.SizeChanged += OnWindowSizeChanged;

            _splashScreen = e.SplashScreen;
            _activedEventArgs = e;

            if (_splashScreen != null)
            {
                _splashImageRect = _splashScreen.ImageLocation;
            }

            ReSize();
            _rootFrame = new Frame();
        }

        public async void LoadDataAsync(IActivatedEventArgs e)
        {
            var activationInfo = ActivationService.GetActivationInfo(e);
            await Startup.ConfigureAsync();

#if DEBUG
            await Task.Delay(1000).ContinueWith(async (t) =>
#else
            await Task.Delay(2000).ContinueWith(async (t) =>
#endif
             {
                 await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                  {
                      var shellArgs = new ShellArgs
                      {
                          ViewModel = activationInfo.EntryViewModel,
                          Parameter = activationInfo.EntryArgs,
                      };

                      _rootFrame.Navigate(typeof(MainShellView), shellArgs);

                      Window.Current.Content = _rootFrame;
                      Window.Current.Activate();
                  });
             });
        }

        private void ReSize()
        {
            if (_splashScreen == null) return;

            splashImage.Height = _splashScreen.ImageLocation.Height;
            splashImage.Width = _splashScreen.ImageLocation.Width;

            splashImage.SetValue(Canvas.TopProperty, _splashScreen.ImageLocation.Top);
            splashImage.SetValue(Canvas.LeftProperty, _splashScreen.ImageLocation.Left);

            progressRing.SetValue(Canvas.TopProperty, _splashScreen.ImageLocation.Top + _splashScreen.ImageLocation.Height + 50);
            progressRing.SetValue(Canvas.LeftProperty, _splashScreen.ImageLocation.Left + _splashScreen.ImageLocation.Width / 2 - progressRing.Width / 2);

        }

        private void OnWindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            if (_splashScreen != null)
            {
                _splashImageRect = _splashScreen.ImageLocation;
                ReSize();
            }
        }
    }
}