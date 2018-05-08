using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using StudyApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace StudyApp.ViewModels
{
    
    public class NewClassViewModel : BaseViewModelWithSubjects
    {
        private int _ID;

        private bool _deleteVisible;
        public bool DeleteVisible
        {
            get { return _deleteVisible; }

            set { SetProperty(ref _deleteVisible, value); }
        }

        private int _classDay;
        public int ClassDay
        {
            get { return _classDay; }

            set { SetProperty(ref _classDay, value); }
        }
        private TimeSpan _classStartTime;
        public TimeSpan ClassStartTime
        {
            get { return _classStartTime; }

            set { SetProperty(ref _classStartTime, value); }
        }

        private int _classDuration;
        public int ClassDuration
        {
            get { return _classDuration; }

            set { SetProperty(ref _classDuration, value); }
        }

        private bool _classRepeats;
        public bool ClassRepeats
        {
            get { return _classRepeats; }

            set { SetProperty(ref _classRepeats, value); StartDateName = value ? "Start Date" : "Date"; }
        }

        private int _classWeek;
        public int ClassWeek
        {
            get { return _classWeek; }

            set { SetProperty(ref _classWeek, value); }
        }

        private string _classLocation;
        public string ClassLocation
        {
            get { return _classLocation; }

            set { SetProperty(ref _classLocation, value); }
        }

        private DateTime _classStartDate;
        public DateTime ClassStartDate
        {
            get { return _classStartDate; }

            set { SetProperty(ref _classStartDate, value); }
        }
        private DateTime _classEndDate;
        public DateTime ClassEndDate
        {
            get { return _classEndDate; }

            set { SetProperty(ref _classEndDate, value); }
        }
        
        private string _startDateName;
        public string StartDateName
        {
            get { return _startDateName; }

            set { SetProperty(ref _startDateName, value); }
        }

        public DelegateCommand FABCommand { get; }
        private async void ExecuteFABCommand()
        {
            int sID = Subjects[SelectedSubjectIndex].ID;
            if (sID > 0 && ClassDay > 0 && ClassDuration > 0 && ClassDuration < 500 && ClassWeek >= 0)
            {
                await ClassModel.SaveItemAsync(new ClassModel { ID = _ID, Day = ClassDay, StartTime = ClassStartTime, Duration = ClassDuration, Repeats = ClassRepeats, Week = ClassWeek,
                    Location = ClassLocation, StartDate = ClassStartDate, EndDate = ClassEndDate, subjectID = sID });
                _ID = 0;
                await NavigationService.GoBackAsync(new NavigationParameters("Type=Classes"));
            }
        }

        public DelegateCommand DeleteCommand { get; }
        private async void ExecuteDeleteCommand()
        {
            ClassModel item = await ClassModel.GetItemByIDAsync(_ID);
            if (item != null)
            {
                await ClassModel.DeleteItemAsync(item);
            }
            _ID = 0;
            await NavigationService.GoBackAsync(new NavigationParameters("Type=Classes"));
            
        }

        public NewClassViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "New Class";

            FABCommand = new DelegateCommand(ExecuteFABCommand);
            DeleteCommand = new DelegateCommand(ExecuteDeleteCommand);
        }

        public static List<string> DaysOfWeek = new List<string>
        {
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday"
        };

        public override async void OnNavigatingTo(NavigationParameters parameters)
        {
            Subjects = new ObservableCollection<SubjectModel>(await SubjectModel.GetAllItemsAsync());
            SelectedSubjectIndex = 0;

            Title = "New Class";
            DeleteVisible = false;
            ClassRepeats = true;
            ClassStartDate = DateTime.Now;
            ClassEndDate = DateTime.Now;
            ClassDuration = 120;
            ClassDay = DaysOfWeek.IndexOf(DateTime.Now.DayOfWeek.ToString());
            _ID = parameters.GetValue<int>("ID");
            
            ClassModel c = await ClassModel.GetItemByIDAsync(_ID);
            if (c != null)
            {
                Title = "Edit Class";
                DeleteVisible = true;
                ClassDay = c.Day;
                ClassDuration = c.Duration;
                ClassLocation = c.Location;
                ClassStartTime = c.StartTime;
                ClassRepeats = c.Repeats;
                ClassStartDate = c.StartDate;
                ClassEndDate = c.EndDate;
                ClassWeek = c.Week;
                await Task.Run(() =>
                {
                    for (int i = 0; i < Subjects.Count(); i++)
                    {
                        if (Subjects[i].ID == c.subjectID)
                        {
                            SelectedSubjectIndex = i;
                            break;
                        }
                    }
                });
            }

        }
    }
}
