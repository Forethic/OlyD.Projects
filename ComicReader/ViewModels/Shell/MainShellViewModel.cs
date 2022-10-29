using OlyD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace ComicReader.ViewModels
{
    public class MainShellViewModel : ShellViewModel
    {
        private readonly NavigationItem LibraryItem = new NavigationItem(0xEA8A, "Library", typeof(LibraryViewModel));
        private readonly NavigationItem ReadHistoryItem = new NavigationItem(0xE81C, "Read History", typeof(ReadHistoryViewModel));
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

        public bool AlwaysShowHeader
        {
            get => _alwaysShowHeader;
            set => Update(ref _alwaysShowHeader, value);
        }
        private bool _alwaysShowHeader = false;

        public string Header
        {
            get => _header;
            set => Update(ref _header, value);
        }
        private string _header;

        public override Task LoadAsync(ShellArgs args)
        {
            Items = GetItems().ToArray();
            return base.LoadAsync(args);
        }

        public async void NavigateTo(Type viewModel)
        {
            switch (viewModel.Name)
            {
                case nameof(SettingsViewModel):
                    NavigationService.Navigate(viewModel, new SettingsArgs());
                    break;
                case nameof(LibraryViewModel):
                    NavigationService.Navigate(viewModel, new LibraryArgs());
                    break;
                case nameof(ReadHistoryViewModel):
                    NavigationService.Navigate(viewModel, new ReadHistoryArgs());
                    break;
                default:
                    throw new NotImplementedException();
            }
            Header = viewModel.Name;
        }

        private IEnumerable<NavigationItem> GetItems()
        {
            yield return LibraryItem;
            yield return ReadHistoryItem;
        }
    }
}