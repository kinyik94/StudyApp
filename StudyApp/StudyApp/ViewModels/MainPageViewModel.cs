using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
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
            if (parameters.Length != 2)
                return;
            string type = parameters[1].Substring(0, (parameters[1] == "Classes" ? 5 : 4));
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
            Title = "Dashboard";

            FABCommand = new DelegateCommand(ExecuteFABCommand);
            ItemSelectCommand = new DelegateCommand<string[]>(ExecuteItemSelectCommand);

            Items = new ObservableCollection<ItemCollection>();
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            Items.Clear();
            SemesterModel currSemester = await SemesterModel.GetLastAsync();
            int week = 0;

            if (currSemester != null)
            {
                Calendar c = new GregorianCalendar();
                int currWeek = c.GetWeekOfYear(DateTime.Today, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                int firstWeek = c.GetWeekOfYear(currSemester.StartDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                week = ((currWeek - firstWeek) % 2) + 1;
            }

            ItemCollection Tasks = new ItemCollection("Tasks", new List<ItemModelInterface>(await TaskModel.GetItemsAsync(DateTime.Today)));
            ItemCollection Classes = new ItemCollection("Classes", new List<ItemModelInterface>(await ClassModel.GetItemsAsync(DateTime.Today, NewClassViewModel.DaysOfWeek.IndexOf(DateTime.Now.DayOfWeek.ToString()), week)));
            ItemCollection Exams = new ItemCollection("Exams", new List<ItemModelInterface>(await ExamModel.GetItemsAsync(DateTime.Today)));

            Items.Add(Exams);
            Items.Add(Classes);
            Items.Add(Tasks);
        }
    }
}
