using ComicReader.Services;
using ComicReader.ViewModels;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
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

            ViewModel.LoadAsync(e.Parameter as LibraryArgs);
        }

        private async void BtnAddFolder_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                var folderPicker = new FolderPicker();
                folderPicker.FileTypeFilter.Add(".jpg");
                var folder = await folderPicker.PickSingleFolderAsync();
                if (folder != null)
                {
                    await TryAddFolder(folder);
                }
            }
            catch (System.Exception)
            {
                // TODO： 弹窗警告
                throw;
            }
        }

        private async Task TryAddFolder(StorageFolder folder) => ViewModel.AddFolder(folder);

        /// <summary>
        /// 长按 加选或者反选
        /// </summary>
        private void OnHoding(object sender, HoldingRoutedEventArgs e)
        {
            if (sender is GridView grid && e.HoldingState == HoldingState.Started)
            {
                if (e.OriginalSource is FrameworkElement element && element.DataContext is LibraryItemViewModel itemViewModel)
                {
                    if (grid.SelectedItems.Contains(itemViewModel))
                    {
                        grid.SelectedItems.Remove(itemViewModel);
                    }
                    else
                    {
                        grid.SelectedItems.Add(itemViewModel);
                    }
                }
            }
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is LibraryItemViewModel itemViewModel)
            {
                itemViewModel.Click();
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}