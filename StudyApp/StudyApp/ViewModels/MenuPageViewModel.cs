using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudyApp.ViewModels
{
    public class MenuPageViewModel : ViewModelBase
    {
        private string _currentDetailPage;

        private string _dashboardColor;
        public string DashboardColor {
            get { return _dashboardColor; }
            set { SetProperty(ref _dashboardColor, value); }
        }

        private string _subjectsColor;
        public string SubjectsColor
        {
            get { return _subjectsColor; }
            set { SetProperty(ref _subjectsColor, value); }
        }

        private string _classesColor;
        public string ClassesColor
        {
            get { return _classesColor; }
            set { SetProperty(ref _classesColor, value); }
        }

        private string _tasksColor;
        public string TasksColor
        {
            get { return _tasksColor; }
            set { SetProperty(ref _tasksColor, value); }
        }

        private string _examsColor;
        public string ExamsColor
        {
            get { return _examsColor; }
            set { SetProperty(ref _examsColor, value); }
        }

        private void setMenuColor(string name, string color)
        {
            switch (name)
            {
                case "Dashboard":
                    DashboardColor = color;
                    break;
                case "Subjects":
                    SubjectsColor = color;
                    break;
                case "Tasks":
                    TasksColor = color;
                    break;
                case "Classes":
                    ClassesColor = color;
                    break;
                case "Exams":
                    ExamsColor = color;
                    break;
            }
        }

        public DelegateCommand<string> NavigateCommand { get; }

        private async void ExecuteNavigateCommand(string path)
        {
            setMenuColor(_currentDetailPage, "Black");

            string[] toSplit = path.Split('/')[1].Split('=');
            if (toSplit.Length > 1)
                _currentDetailPage = toSplit[1];
            else
                _currentDetailPage = toSplit[0];

            setMenuColor(_currentDetailPage, "Blue");

            await NavigationService.NavigateAsync(path);
        }

        public MenuPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Home Page";
            _currentDetailPage = "Dashboard";
            ExamsColor = "Black";
            ClassesColor = "Black";
            TasksColor = "Black";
            SubjectsColor = "Black";
            DashboardColor = "Blue";
            NavigateCommand = new DelegateCommand<string>(ExecuteNavigateCommand);
        }

    }
}