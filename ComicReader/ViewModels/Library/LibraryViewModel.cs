using Lia.Services;
using Lia.ViewModels;
using System.Threading.Tasks;

namespace ComicReader.ViewModels
{
    public class LibraryViewModel : BaseViewModel
    {
        public LibraryViewModel(ICommonServices commonService)
            : base(commonService)
        {
        }

        public async Task LoadAsync()
        {
            // TODO: 加载 书库数据
        }
    }
}