using ComicReader.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ComicReader.Selectors
{
    public class LibraryItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ShelfTemplate { get; set; }

        public DataTemplate BookTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is LibraryItemViewModel libraryItemViewModel)
            {
                if (libraryItemViewModel.IsShelf)
                {
                    return ShelfTemplate;
                }
                return BookTemplate;
            }
            return ShelfTemplate;
        }
    }
}