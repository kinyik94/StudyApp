using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using StudyApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace StudyApp.ViewModels
{
    public class ItemsViewModel : ViewModelBase
    {
        public DelegateCommand FABCommand { get; }
        private async void ExecuteFABCommand()
        {
            string type = Title.Substring(0, (Title == "Classes" ? 5 : 4));
            await NavigationService.NavigateAsync("NewItem?Type=" + type);
        }

        public DelegateCommand<string> ItemSelectCommand { get; }
        private async void ExecuteItemSelectCommand(string id)
        {
            string type = Title.Substring(0, (Title == "Classes" ? 5 : 4));
            await NavigationService.NavigateAsync("NewItem?Type=" + type + "&ID=" + id);
        }

        private ObservableCollection<ItemModelInterface> _items;
        public ObservableCollection<ItemModelInterface> Items
        {
            get { return _items; }

            set { SetProperty(ref _items, value); }
        }

        public ItemsViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Items";
            Items = new ObservableCollection<ItemModelInterface>();

            FABCommand = new DelegateCommand(ExecuteFABCommand);
            ItemSelectCommand = new DelegateCommand<string>(ExecuteItemSelectCommand);

        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            String type = parameters.GetValue<string>("Type");
            Title = type;
            Items.Clear();
            switch (type)
            {
                case "Tasks":
                    Items = new ObservableCollection<ItemModelInterface>( await TaskModel.GetItemsAsync(DateTime.Today) );
                    break;
                case "Classes":
                    Items = new ObservableCollection<ItemModelInterface>( await ClassModel.GetItemsAsync(DateTime.Today, NewClassViewModel.DaysOfWeek.IndexOf(DateTime.Now.DayOfWeek.ToString()), 0)  );
                    break;
                case "Exams":
                    Items = new ObservableCollection<ItemModelInterface>( await ExamModel.GetItemsAsync(DateTime.Today) );
                    break;
            }
        }
    }
}
