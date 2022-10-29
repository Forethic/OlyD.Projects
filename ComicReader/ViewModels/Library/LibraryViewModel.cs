using OlyD.Infrastructure;
using OlyD.Services;

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