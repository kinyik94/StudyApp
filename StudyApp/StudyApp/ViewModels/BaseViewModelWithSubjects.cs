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
	public class BaseViewModelWithSubjects : ViewModelBase
    {
        private ObservableCollection<SubjectModel> _subjects;
        public ObservableCollection<SubjectModel> Subjects
        {
            get { return _subjects; }

            set { SetProperty(ref _subjects, value); }
        }

        private int _selectedSubjectIndex;
        public int SelectedSubjectIndex
        {
            get { return _selectedSubjectIndex; }

            set { SetProperty(ref _selectedSubjectIndex, value); RaisePropertyChanged("SelectedSubjectIndex"); }
        }

        public BaseViewModelWithSubjects(INavigationService navigationService)
            : base(navigationService)
        {
            SelectedSubjectIndex = 0;
        }
	}
}
