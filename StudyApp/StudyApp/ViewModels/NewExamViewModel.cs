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
            try
            {
                ExamModel item = await ExamModel.GetItemByIDAsync(_ID);
                if (item != null)
                {
                    await ExamModel.DeleteItemAsync(item);
                }
            }
            finally
            {
                _ID = 0;
                await NavigationService.GoBackAsync(new NavigationParameters("Type=Exams"));
            }

        }

        public DelegateCommand FABCommand { get; }
        private async void ExecuteFABCommand()
        {
            if (Subjects == null || Subjects.Count == 0 || SelectedSubjectIndex < 0)
            {
                _ID = 0;
                await NavigationService.GoBackAsync(new NavigationParameters("Type=Exams"));
                return;
            }
            int sID = Subjects[SelectedSubjectIndex].ID;
            if (sID > 0 && ExamDuration > 0 && ExamDuration < 500)
            {
                try
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
                }
                finally
                {
                    _ID = 0;
                    await NavigationService.GoBackAsync(new NavigationParameters("Type=Exams"));
                }
            }
        }

        public NewExamViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = Localization.LocalizationResources.Exam;

            FABCommand = new DelegateCommand(ExecuteFABCommand);
            DeleteCommand = new DelegateCommand(ExecuteDeleteCommand);
        }

        public override async void OnNavigatingTo(NavigationParameters parameters)
        {
            try
            {
                Subjects = new ObservableCollection<SubjectModel>(await SubjectModel.GetAllItemsAsync());
            }
            catch
            {
                if(Subjects != null)
                    Subjects.Clear();
            }
            SelectedSubjectIndex = 0;

            Title = Localization.LocalizationResources.Exam;
            DeleteVisible = false;
            ExamDate = DateTime.Today;
            ExamDuration = 120;

            if (parameters.GetValue<string>("Type") != "Exam")
                return;

            _ID = parameters.GetValue<int>("ID");

            try
            {
                ExamModel exam = await ExamModel.GetItemByIDAsync(_ID);
                if (exam != null)
                {
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
            finally { }
        }
    }
}
