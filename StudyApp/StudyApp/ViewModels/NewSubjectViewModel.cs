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
	public class NewSubjectViewModel : ViewModelBase
	{
        public DelegateCommand FABCommand { get; }
        private async void ExecuteFABCommand()
        {
            if (SelectedSemesterIndex >= 0 && SubjectName != null && SubjectName.Length >= 0)
            {
                try
                {
                    int sID = Semesters[SelectedSemesterIndex].ID;
                    await SubjectModel.SaveItemAsync(new SubjectModel { ID = _ID, Name = SubjectName, semesterID = sID });
                }
                finally
                {
                    _ID = 0;
                    await NavigationService.GoBackAsync();
                }
            }
            else
            {
                _ID = 0;
                await NavigationService.GoBackAsync();
            }
        }

        private int _ID;

        private string _subjectName;
        public string SubjectName
        {
            get { return _subjectName; }

            set { SetProperty(ref _subjectName, value); }
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

        private int _selectedSemesterIndex;
        public int SelectedSemesterIndex
        {
            get { return _selectedSemesterIndex; }

            set { SetProperty(ref _selectedSemesterIndex, value); RaisePropertyChanged("SelectedSemesterIndex"); }
        }

        public NewSubjectViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = Localization.LocalizationResources.NewSubject;

            FABCommand = new DelegateCommand(ExecuteFABCommand);
            MenuCommand = new DelegateCommand<string>(ExecuteMenuCommand);
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                Semesters = new ObservableCollection<SemesterModel>(await SemesterModel.GetItemsAsync());
            }
            catch
            {
                if(Semesters != null)
                    Semesters.Clear();
            }
            SelectedSemesterIndex = 0;

            Title = Localization.LocalizationResources.NewSubject;
            _ID = parameters.GetValue<int>("ID");
            try
            {
                SubjectModel subject = await SubjectModel.GetItemByIDAsync(_ID);
                if (subject != null)
                {
                    Title = Localization.LocalizationResources.EditSubject;
                    SubjectName = subject.Name;
                    await Task.Run(() =>
                    {
                        for (int i = 0; i < Semesters.Count(); i++)
                        {
                            if (Semesters[i].ID == subject.semesterID)
                            {
                                SelectedSemesterIndex = i;
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
