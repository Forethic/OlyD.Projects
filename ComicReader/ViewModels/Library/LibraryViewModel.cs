using ComicReader.DataModels;
using Lia.Services;
using Lia.ViewModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;

namespace ComicReader.ViewModels
{
    public class LibraryArgs
    {
        public static LibraryArgs CreateDefault() => new LibraryArgs();

        public Shelf Shelf { get; set; }
    }

    public class LibraryViewModel : BaseViewModel
    {
        private Library Library => Library.Instance;

        public ObservableCollection<LibraryItemViewModel> Items { get => _items; set => Update(ref _items, value); }
        private ObservableCollection<LibraryItemViewModel> _items;

        public LibraryArgs _currentArgs;

        public bool IsEmpty { get => _isEmpty; set => Update(ref _isEmpty, value); }
        private bool _isEmpty = true;

        public LibraryViewModel(ICommonServices commonService)
            : base(commonService)
        {
            Items = new ObservableCollection<LibraryItemViewModel>();
        }

        public async Task LoadAsync(LibraryArgs args)
        {
            _currentArgs = args ?? LibraryArgs.CreateDefault();
            await RefreshItems();
        }

        public async Task RefreshItems()
        {
            Items.Clear();

            // 代表 Library
            if (_currentArgs.Shelf == null)
            {
                foreach (var shelf in Library.Shelves)
                {
                    Items.Add(LibraryItemViewModel.Create(shelf));
                }
            }
            // 代表具体的 Shelf
            else
            {
                foreach (var shelf in _currentArgs.Shelf.Shelves)
                {
                    Items.Add(LibraryItemViewModel.Create(shelf));
                }

                foreach (var book in _currentArgs.Shelf.Books)
                {
                    Items.Add(LibraryItemViewModel.Create(book));
                }
            }

            IsEmpty = Items.Count == 0;
        }

        public async void AddFolder(StorageFolder folder)
        {
            IsEmpty = false;
            await Library.AddFolderToLibrary(folder);
            await LoadAsync(null);
        }
    }
}