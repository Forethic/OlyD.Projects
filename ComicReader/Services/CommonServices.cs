using OlyD.Services;

namespace ComicReader.Services
{
    public class CommonServices : ICommonServices
    {
        public INavigationService NavigationService { get; }

        public CommonServices(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }
}