using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using StudyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudyApp.ViewModels
{
	public class NewSemesterViewModel : ViewModelBase
	{
        public DelegateCommand FABCommand { get; }
        private async void ExecuteFABCommand()
        {
            if (SemesterName != null && SemesterName.Length > 0)
            {
                try
                {
                    await SemesterModel.SaveItemAsync(new SemesterModel { ID = _ID, Name = SemesterName, StartDate = StartDate, EndDate = EndDate });
                }
                finally
                {
                    await NavigationService.GoBackAsync();
                }
            }
        }

        private int _ID;

        private string _semesterName;
        public string SemesterName
        {
            get { return _semesterName; }

            set { SetProperty(ref _semesterName, value); }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }

            set { SetProperty(ref _startDate, value); }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get { return _endDate; }

            set { SetProperty(ref _endDate, value); }
        }

        public NewSemesterViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "New Semester";

            FABCommand = new DelegateCommand(ExecuteFABCommand);

            _ID = 0;
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            string Action = parameters.GetValue<string>("Action");
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
            if (Action == "Edit Semester")
            {
                _ID = parameters.GetValue<int>("ID");
                try
                {
                    SemesterModel sem = await SemesterModel.GetItemByIDAsync(_ID);
                    if (sem != null)
                    {
                        SemesterName = sem.Name;
                        StartDate = sem.StartDate;
                        EndDate = sem.EndDate;
                    }
                }
                finally { }
            }
        }
    }
}
