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
	public class NewTaskViewModel : BaseViewModelWithSubjects
	{
        private int _ID;

        private bool _deleteVisible;
        public bool DeleteVisible
        {
            get { return _deleteVisible; }

            set { SetProperty(ref _deleteVisible, value); }
        }

        public DelegateCommand FABCommand { get; }
        private async void ExecuteFABCommand()
        {
            if (Subjects == null || SelectedSubjectIndex < 0)
            {
                _ID = 0;
                await NavigationService.GoBackAsync(new NavigationParameters("Type=Tasks"));
                return;
            }
            int sID = Subjects[SelectedSubjectIndex].ID;
            if (sID > 0 && TaskTitle != null && TaskTitle.Length > 0)
            {
                try
                {
                    await TaskModel.SaveItemAsync(new TaskModel { ID = _ID, Title = TaskTitle, subjectID = sID, DueDate = TaskDueDate, Summary = TaskSummary });
                }
                finally
                {
                    _ID = 0;
                    await NavigationService.GoBackAsync(new NavigationParameters("Type=Tasks"));
                }
            }
        }

        public DelegateCommand DeleteCommand { get; }
        private async void ExecuteDeleteCommand()
        {
            try
            {
                TaskModel item = await TaskModel.GetItemByIDAsync(_ID);
                if (item != null)
                {
                    await TaskModel.DeleteItemAsync(item);
                }
            }
            finally
            {
                _ID = 0;
                await NavigationService.GoBackAsync(new NavigationParameters("Type=Tasks"));
            }

        }

        private string _taskTitle;
        public string TaskTitle
        {
            get { return _taskTitle; }

            set { SetProperty(ref _taskTitle, value); }
        }

        private DateTime _taskDueDate;
        public DateTime TaskDueDate
        {
            get { return _taskDueDate; }

            set { SetProperty(ref _taskDueDate, value); }
        }

        private string _taskSummary;
        public string TaskSummary
        {
            get { return _taskSummary; }

            set { SetProperty(ref _taskSummary, value); }
        }

        public NewTaskViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Task";

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

            Title = "Task";
            DeleteVisible = false;
            TaskDueDate = DateTime.Today;
            _ID = parameters.GetValue<int>("ID");

            if (parameters.GetValue<string>("Type") != "Task")
                return;

            try
            {
                TaskModel task = await TaskModel.GetItemByIDAsync(_ID);
                if (task != null)
                {
                    DeleteVisible = true;
                    TaskTitle = task.Title;
                    TaskDueDate = task.DueDate;
                    TaskSummary = task.Summary;
                    await Task.Run(() =>
                    {
                        for (int i = 0; i < Subjects.Count(); i++)
                        {
                            if (Subjects[i].ID == task.subjectID)
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
