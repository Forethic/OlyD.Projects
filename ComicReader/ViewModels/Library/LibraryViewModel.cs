using Lia.Services;
using Lia.ViewModels;

namespace ComicReader.ViewModels
{
    public class LibraryArgs
    {
    }

    public class LibraryViewModel : BaseViewModel
    {
        public LibraryViewModel(ICommonServices commonService)
            : base(commonService)
        {

        }
    }
}