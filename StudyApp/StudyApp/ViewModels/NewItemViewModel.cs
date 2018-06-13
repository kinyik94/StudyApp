using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudyApp.ViewModels
{
    public class NewItemViewModel : ViewModelBase
    {
        private int _childIndex;
        public int ChildIndex
        {
            get { return _childIndex; }

            set { SetProperty(ref _childIndex, value); }
        }

        public NewItemViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = Localization.LocalizationResources.Task;
            ChildIndex = 0;
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            Title = parameters.GetValue<string>("Type");
            switch (Title)
            {
                case "Class":
                    ChildIndex = 1;
                    break;
                case "Exam":
                    ChildIndex = 2;
                    break;
                default:
                    ChildIndex = 0;
                    break;

            }
        }
    }
}
