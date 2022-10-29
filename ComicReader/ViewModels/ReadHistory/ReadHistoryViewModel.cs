using OlyD.Infrastructure;
using OlyD.Services;

namespace ComicReader.ViewModels
{
    public class ReadHistoryArgs { }

    public class ReadHistoryViewModel : BaseViewModel
    {
        public ReadHistoryViewModel(ICommonServices commonService)
            : base(commonService)
        {
        }
    }
}