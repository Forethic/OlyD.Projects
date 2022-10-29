using OlyD.Infrastructure;
using OlyD.Services;
using System;
using System.Threading.Tasks;

namespace ComicReader.ViewModels
{
    public class ShellArgs
    {
        public Type ViewModel { get; set; }
        public object Parameter { get; set; }
    }

    public class ShellViewModel : BaseViewModel
    {
        public ShellArgs ViewModelArgs { get; protected set; }

        public ShellViewModel(ICommonServices commonService)
            : base(commonService)
        {
        }

        public virtual Task LoadAsync(ShellArgs args)
        {
            ViewModelArgs = args;
            if (ViewModelArgs != null)
            {
                NavigationService.Navigate(ViewModelArgs.ViewModel, ViewModelArgs.Parameter);
            }
            return Task.CompletedTask;
        }
    }
}