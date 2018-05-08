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
	public class NewExamViewModel : BaseViewModelWithSubjects
    {
        private int _ID;

        private bool _deleteVisible;
        public bool DeleteVisible
        {
            get { return _deleteVisible; }

            set { SetProperty(ref _deleteVisible, value); }
        }

        private TimeSpan _examStartTime;
        public TimeSpan ExamStartTime
        {
            get { return _examStartTime; }

            set { SetProperty(ref _examStartTime, value); }
        }

        private int _examDuration;
        public int ExamDuration
        {
            get { return _examDuration; }

            set { SetProperty(ref _examDuration, value); }
        }

        private string _examLocation;
        public string ExamLocation
        {
            get { return _examLocation; }

            set { SetProperty(ref _examLocation, value); }
        }
        private DateTime _examDate;
        public DateTime ExamDate
        {
            get { return _examDate; }

            set { SetProperty(ref _examDate, value); }
        }

        public DelegateCommand DeleteCommand { get; }
        private async void ExecuteDeleteCommand()
        {
            ExamModel item = await ExamModel.GetItemByIDAsync(_ID);
            if (item != null)
            {
                await ExamModel.DeleteItemAsync(item);
            }
            _ID = 0;
            await NavigationService.GoBackAsync(new NavigationParameters("Type=Exams"));

        }

        public DelegateCommand FABCommand { get; }
        private async void ExecuteFABCommand()
        {
            int sID = Subjects[SelectedSubjectIndex].ID;
            if (sID > 0 && ExamDuration > 0 && ExamDuration < 500)
            {
                await ExamModel.SaveItemAsync(new ExamModel
                {
                    ID = _ID,
                    StartTime = ExamStartTime,
                    Duration = ExamDuration,
                    Location = ExamLocation,
                    Date = ExamDate,
                    subjectID = sID
                });
                _ID = 0;
                await NavigationService.GoBackAsync(new NavigationParameters("Type=Exams"));
            }
        }

        public NewExamViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "New Exam";

            FABCommand = new DelegateCommand(ExecuteFABCommand);
            DeleteCommand = new DelegateCommand(ExecuteDeleteCommand);
        }

        public override async void OnNavigatingTo(NavigationParameters parameters)
        {
            Subjects = new ObservableCollection<SubjectModel>(await SubjectModel.GetAllItemsAsync());
            SelectedSubjectIndex = 0;

            Title = "New Exam";
            DeleteVisible = false;
            ExamDate = DateTime.Now;
            ExamDuration = 120;

            _ID = parameters.GetValue<int>("ID");

            ExamModel exam = await ExamModel.GetItemByIDAsync(_ID);
            if (exam != null)
            {
                Title = "Edit Exam";
                DeleteVisible = true;
                ExamDate = exam.Date;
                ExamDuration = exam.Duration;
                ExamLocation = exam.Location;
                ExamStartTime = exam.StartTime;
                await Task.Run(() =>
                {
                    for (int i = 0; i < Subjects.Count(); i++)
                    {
                        if (Subjects[i].ID == exam.subjectID)
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
