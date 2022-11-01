using Lia.ViewModels;
using Lia.Services;

namespace ComicReader.ViewModels
{
    public class SettingsArgs
    {
        public static SettingsArgs CreateDefault() => new SettingsArgs();
    }

    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel(ICommonServices commonService)
            : base(commonService)
        {
        }
    }
}