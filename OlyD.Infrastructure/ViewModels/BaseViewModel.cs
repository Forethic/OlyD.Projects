﻿using OlyD.Services;

namespace OlyD.Infrastructure
{
    public class BaseViewModel : ObservableObject
    {
        public BaseViewModel(ICommonServices commonService)
        {
            NavigationService = commonService.NavigationService;
        }

        public INavigationService NavigationService { get; }

        public virtual string Title => string.Empty;
    }
}