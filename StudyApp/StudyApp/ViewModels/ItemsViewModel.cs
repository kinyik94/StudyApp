using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using StudyApp.Models;
using StudyApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace StudyApp.ViewModels
{
    public class ItemsViewModel : ViewModelBase
    {
        private string Type;
        public DelegateCommand FABCommand { get; }
        private async void ExecuteFABCommand()
        {
            string type = Type.Substring(0, (Type == "Classes" ? 5 : 4));
            await NavigationService.NavigateAsync("NewItem?Type=" + type);
        }

        public DelegateCommand<string> ItemSelectCommand { get; }
        private async void ExecuteItemSelectCommand(string id)
        {
            string type = Type.Substring(0, (Type == "Classes" ? 5 : 4));
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
            Type = "Items";
            Title = Localization.LocalizationResources.Items;
            Items = new ObservableCollection<ItemModelInterface>();

            FABCommand = new DelegateCommand(ExecuteFABCommand);
            ItemSelectCommand = new DelegateCommand<string>(ExecuteItemSelectCommand);
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            Type = parameters.GetValue<string>("Type");
            Type = Type ?? "Tasks";
            Title = Localization.LocalizationResources.ResourceManager.GetString(Type);
            Items.Clear();
            try
            {
                switch (Type)
                {
                    case "Classes":
                        Items = new ObservableCollection<ItemModelInterface>(await ClassModel.GetAllClass());
                        break;
                    case "Exams":
                        Items = new ObservableCollection<ItemModelInterface>(await ExamModel.GetAllExam());
                        break;
                    default:
                        Items = new ObservableCollection<ItemModelInterface>(await TaskModel.GetAllTask());
                        break;
                }
            }
            catch
            {
                Items.Clear();
            }
        }
    }
}
