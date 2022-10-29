using OlyD.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicReader.ViewModels
{
    public class MainShellViewModel : ShellViewModel
    {
        private readonly NavigationItem SettingsItem = new NavigationItem(0x0000, "Settings", typeof(SettingsViewModel));

        public MainShellViewModel(ICommonServices commonService)
            : base(commonService)
        {
        }

        public object SelectedItem
        {
            get => _selectedItem;
            set => Update(ref _selectedItem, value);
        }
        private object _selectedItem;

        public IEnumerable<NavigationItem> Items
        {
            get => _items;
            set => Update(ref _items, value);
        }
        private IEnumerable<NavigationItem> _items;

        public bool IsPaneOpen
        {
            get => _isPaneOpen;
            set => Update(ref _isPaneOpen, value);
        }
        private bool _isPaneOpen = false;

        public bool IsPaneVisible
        {
            get => _isPaneVisible;
            set => Update(ref _isPaneVisible, value);
        }
        private bool _isPaneVisible = true;

        public override Task LoadAsync(ShellArgs args)
        {
            //Items = GetItems().ToArray();
            return base.LoadAsync(args);
        }

        public async void NavigateTo(Type viewModel)
        {
            switch (viewModel.Name)
            {
                case nameof(SettingsViewModel):
                    NavigationService.Navigate(viewModel, new SettingsArgs());
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private IEnumerable<NavigationItem> GetItems()
        {
            // TODO:
            return null;
        }
    }
}