using ComicReader.DataModels;
using Lia.Services;
using Lia.ViewModels;
using System;

namespace ComicReader.ViewModels
{
    public class LibraryItemViewModel : BaseViewModel
    {
        public string Name => Item.Name;

        public LibraryItem Item { get; set; }

        public Book Book => Item as Book;

        public bool IsShelf => Item is Shelf;

        public LibraryItemViewModel(ICommonServices commonService)
            : base(commonService)
        {
        }

        public static LibraryItemViewModel Create(LibraryItem item)
        {
            var itemViewModel = new LibraryItemViewModel(ServiceLocator.Current.GetService<ICommonServices>())
            {
                Item = item,
            };
            return itemViewModel;
        }

        public void Click()
        {
            if (IsShelf)
            {
                NavigationService.Navigate<LibraryViewModel>(new LibraryArgs() { Shelf = Item as Shelf });
            }
            else
            {
                // TODO: Book 页面
                Console.WriteLine($"导航到Book： {Book.Name}");
            }
        }
    }
}