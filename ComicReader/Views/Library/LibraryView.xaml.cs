using ComicReader.ViewModels;
using Windows.UI.Xaml.Controls;

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
            InitializeComponent();
        }
    }
}