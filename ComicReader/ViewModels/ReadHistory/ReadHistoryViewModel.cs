using Lia.ViewModels;
using Lia.Services;

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