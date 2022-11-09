using ComicReader.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ComicReader.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LibraryView : Page
    {
        public LibraryViewModel ViewModel { get; }

        public LibraryView()
        {
            ViewModel = ServiceLocator.Current.GetService<LibraryViewModel>();

            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ViewModel.LoadAsync();
        }
    }
}