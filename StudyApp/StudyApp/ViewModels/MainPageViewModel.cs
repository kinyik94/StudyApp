using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using StudyApp.Helper;
using StudyApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace StudyApp.ViewModels
{

    public class MainPageViewModel : ViewModelBase
    {

        private Dictionary<string, string> groupNamesToType;

        public class ItemCollection : ObservableCollection<ItemModelInterface>
        {
            public ItemCollection(string _name) : base()
            {
                Name = _name;
            }
            public ItemCollection(string _name, List<ItemModelInterface> l) : base(l)
            {
                Name = _name;
            }
            public string Name { get; set; }
        }

        public DelegateCommand FABCommand { get; }
        private async void ExecuteFABCommand()
        {
            await NavigationService.NavigateAsync("NewItem?Type=Task");
        }

        public DelegateCommand<string[]> ItemSelectCommand { get; }
        private async void ExecuteItemSelectCommand(string[] parameters)
        {
            if (parameters.Length != 2 || parameters[0] == null || parameters[1] == null)
                return;
            string type = parameters[1];
            type = groupNamesToType[type];
            type = type.Substring(0, (type == "Classes" ? 5 : 4));
            await NavigationService.NavigateAsync("NewItem?Type=" + type + "&ID=" + parameters[0]);
        }

        private ObservableCollection<ItemCollection> _items;
        public ObservableCollection<ItemCollection> Items {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = Localization.LocalizationResources.Dashboard;

            FABCommand = new DelegateCommand(ExecuteFABCommand);
            ItemSelectCommand = new DelegateCommand<string[]>(ExecuteItemSelectCommand);

            Items = new ObservableCollection<ItemCollection>();

            groupNamesToType = new Dictionary<string, string>();
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {

            string id = parameters.GetValue<string>("id");
            if (id != null)
            {
                App.UserId = id;
            }

            Items.Clear();

            List<DateTime> dates = new List<DateTime>();
            dates.Add(DateTime.Today);
            dates.Add(DateTime.Today.AddDays(1));

            ItemCollection Tasks;
            ItemCollection Classes;
            ItemCollection Exams;

            try
            {
                Tasks = new ItemCollection(Localization.LocalizationResources.Tasks, new List<ItemModelInterface>(await TaskModel.GetItemsAsync(dates)));
            }
            catch
            {
                Tasks = new ItemCollection(Localization.LocalizationResources.Tasks);
            }
            groupNamesToType[Localization.LocalizationResources.Tasks] =  "Tasks";
            try
            {
                Classes = new ItemCollection(Localization.LocalizationResources.Classes, new List<ItemModelInterface>(await ClassModel.GetItemsAsync(DateTime.Today, DateHelper.GetDayOfWeek(), await DateHelper.GetWeek())));
            }
            catch
            {
                Classes = new ItemCollection(Localization.LocalizationResources.Classes);
            }
            groupNamesToType[Localization.LocalizationResources.Classes] =  "Classes";
            try
            {
                Exams = new ItemCollection(Localization.LocalizationResources.Exams, new List<ItemModelInterface>(await ExamModel.GetItemsAsync(dates)));
            }
            catch
            {
                Exams = new ItemCollection(Localization.LocalizationResources.Exams);
            }
            groupNamesToType[Localization.LocalizationResources.Exams] = "Exams";
            Items.Add(Exams);
            Items.Add(Classes);
            Items.Add(Tasks);
        }
    }
}
