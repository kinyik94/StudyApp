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

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private string _currentDetailPage;

        private string _dashboardColor;
        public string DashboardColor
        {
            get { return _dashboardColor; }
            set { SetProperty(ref _dashboardColor, value); }
        }

        private string _dashboardIconSource;
        public string DashboardIconSource
        {
            get { return _dashboardIconSource; }
            set { SetProperty(ref _dashboardIconSource, value); }
        }

        private string _subjectsColor;
        public string SubjectsColor
        {
            get { return _subjectsColor; }
            set { SetProperty(ref _subjectsColor, value); }
        }

        private string _subjectsIconSource;
        public string SubjectsIconSource
        {
            get { return _subjectsIconSource; }
            set { SetProperty(ref _subjectsIconSource, value); }
        }

        private string _classesColor;
        public string ClassesColor
        {
            get { return _classesColor; }
            set { SetProperty(ref _classesColor, value); }
        }

        private string _classesIconSource;
        public string ClassesIconSource
        {
            get { return _classesIconSource; }
            set { SetProperty(ref _classesIconSource, value); }
        }

        private string _tasksColor;
        public string TasksColor
        {
            get { return _tasksColor; }
            set { SetProperty(ref _tasksColor, value); }
        }

        private string _tasksIconSource;
        public string TasksIconSource
        {
            get { return _tasksIconSource; }
            set { SetProperty(ref _tasksIconSource, value); }
        }

        private string _examsColor;
        public string ExamsColor
        {
            get { return _examsColor; }
            set { SetProperty(ref _examsColor, value); }
        }

        private string _examsIconSource;
        public string ExamsIconSource
        {
            get { return _examsIconSource; }
            set { SetProperty(ref _examsIconSource, value); }
        }

        private void setMenuColor(string name, string color)
        {
            string iconPost = color == "Black" ? "" : "_blue";

            switch (name)
            {
                case "Dashboard":
                    DashboardColor = color;
                    DashboardIconSource = "Images/icon_dashboard" + iconPost +".png";
                    break;
                case "Subjects":
                    SubjectsColor = color;
                    SubjectsIconSource = "Images/icon_subject" + iconPost + ".png";
                    break;
                case "Tasks":
                    TasksColor = color;
                    TasksIconSource = "Images/icon_tasks" + iconPost + ".png";
                    break;
                case "Classes":
                    ClassesColor = color;
                    ClassesIconSource = "Images/icon_class" + iconPost + ".png";
                    break;
                case "Exams":
                    ExamsColor = color;
                    ExamsIconSource = "Images/icon_exam" + iconPost + ".png";
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

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var name = parameters.GetValue<string>("name");

            if (name != null)
                UserName = name;
        }

        public MenuPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = Localization.LocalizationResources.ResourceManager.GetString("HomePage");
            _currentDetailPage = "Dashboard";

            UserName = "General User";

            ExamsColor = "Black";
            ClassesColor = "Black";
            TasksColor = "Black";
            SubjectsColor = "Black";
            DashboardColor = "Blue";

            ExamsIconSource = "Images/icon_exam.png";
            ClassesIconSource = "Images/icon_class.png";
            TasksIconSource = "Images/icon_tasks.png";
            SubjectsIconSource = "Images/icon_subject.png";
            DashboardIconSource = "Images/icon_dashboard_blue.png";

            NavigateCommand = new DelegateCommand<string>(ExecuteNavigateCommand);
        }

    }
}