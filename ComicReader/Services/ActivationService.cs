using ComicReader.ViewModels;
using OlyD.Infrastructure;
using System;
using Windows.ApplicationModel.Activation;

namespace ComicReader.Services
{
    public class ActivationInfo
    {
        public static ActivationInfo CreateDefault() => Create<LibraryViewModel>();

        public static ActivationInfo Create<TViewModel>(object entryArgs = null)
            where TViewModel : BaseViewModel
        {
            return new ActivationInfo
            {
                EntryViewModel = typeof(TViewModel),
                EntryArgs = entryArgs,
            };
        }

        public Type EntryViewModel { get; set; }
        public object EntryArgs { get; set; }
    }

    /// <summary>
    /// 程序启动时要进入哪个界面
    /// </summary>
    public static class ActivationService
    {
        public static ActivationInfo GetActivationInfo(IActivatedEventArgs args)
        {
            return ActivationInfo.CreateDefault();
        }
    }
}