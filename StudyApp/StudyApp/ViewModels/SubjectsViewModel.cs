using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using StudyApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace StudyApp.ViewModels
{
    
    public class SubjectsViewModel : BaseViewModelWithSubjects
    {
        public DelegateCommand FABCommand { get; }
        private async void ExecuteFABCommand()
        {
            await NavigationService.NavigateAsync("NewSubject");
        }

        public DelegateCommand<string> SubjectSelectCommand { get; }
        private async void ExecuteSubjectSelectCommand(string id)
        {
            await NavigationService.NavigateAsync("NewSubject?ID=" + id);
        }

        public DelegateCommand<string> MenuCommand { get; }
        private async void ExecuteMenuCommand(string action)
        {
            string ID = "";
            if (action == Localization.LocalizationResources.EditSemester)
                ID = "&ID=" + Semesters[SelectedSemesterIndex].ID;
            await NavigationService.NavigateAsync("NewSemester?Action=" + action + ID);
        }

        private ObservableCollection<SemesterModel> _semesters;
        public ObservableCollection<SemesterModel> Semesters
        {
            get { return _semesters; }

            set { SetProperty(ref _semesters, value); }
        }

        public DelegateCommand SemesterChangedCommand { get; }

        private async void ExecuteSemesterChangedCommand()
        {
            if (SelectedSemesterIndex >= 0 && Semesters.Count != 0)
            {
                int sID = Semesters[SelectedSemesterIndex].ID;
                try
                {
                    Subjects = new ObservableCollection<SubjectModel>(await SubjectModel.GetItemsAsync(sID));
                }
                catch
                {
                    if(Subjects != null)
                        Subjects.Clear();
                }
            }
        }

        private int _selectedSemesterIndex;
        public int SelectedSemesterIndex
        {
            get { return _selectedSemesterIndex; }

            set { SetProperty(ref _selectedSemesterIndex, value); RaisePropertyChanged("SelectedSemesterIndex"); }
        }

        public SubjectsViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = Localization.LocalizationResources.Subjects;
            Semesters = new ObservableCollection<SemesterModel>();

            SemesterChangedCommand = new DelegateCommand(ExecuteSemesterChangedCommand);
            FABCommand = new DelegateCommand(ExecuteFABCommand);
            MenuCommand = new DelegateCommand<string>(ExecuteMenuCommand);
            SubjectSelectCommand = new DelegateCommand<string>(ExecuteSubjectSelectCommand);
            
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                Semesters = new ObservableCollection<SemesterModel>(await SemesterModel.GetItemsAsync());
                SelectedSemesterIndex = 0;
                ExecuteSemesterChangedCommand();
            }
            finally { }
        }
    }
}
