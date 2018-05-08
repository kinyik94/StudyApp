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
            int sID = Subjects[SelectedSubjectIndex].ID;
            if (sID > 0 && TaskTitle != null && TaskTitle.Length > 0)
            {
                await TaskModel.SaveItemAsync(new TaskModel { ID = _ID, Title = TaskTitle, subjectID = sID, DueDate = TaskDueDate, Summary = TaskSummary });
                _ID = 0;
                await NavigationService.GoBackAsync(new NavigationParameters("Type=Tasks"));
            }
        }

        public DelegateCommand DeleteCommand { get; }
        private async void ExecuteDeleteCommand()
        {
            TaskModel item = await TaskModel.GetItemByIDAsync(_ID);
            if (item != null)
            {
                await TaskModel.DeleteItemAsync(item);
            }
            _ID = 0;
            await NavigationService.GoBackAsync(new NavigationParameters("Type=Tasks"));

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
            Title = "New Task";

            FABCommand = new DelegateCommand(ExecuteFABCommand);
            DeleteCommand = new DelegateCommand(ExecuteDeleteCommand);
        }

        public override async void OnNavigatingTo(NavigationParameters parameters)
        {
            Subjects = new ObservableCollection<SubjectModel>(await SubjectModel.GetAllItemsAsync());
            SelectedSubjectIndex = 0;

            Title = "New Task";
            DeleteVisible = false;
            _ID = parameters.GetValue<int>("ID");

            TaskModel task = await TaskModel.GetItemByIDAsync(_ID);
            if (task != null)
            {
                Title = "Edit Task";
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
    }
}
