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
        string Type;

        private int _childIndex;
        public int ChildIndex
        {
            get { return _childIndex; }

            set { SetProperty(ref _childIndex, value); }
        }

        public NewItemViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Type = "Task";
            Title = "New " + Type;
            ChildIndex = 0;
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            Type = parameters.GetValue<string>("Type");
            Title = "New " + Type;
            switch (Type)
            {
                case "Task":
                    ChildIndex = 0;
                    break;
                case "Class":
                    ChildIndex = 1;
                    break;
                case "Exam":
                    ChildIndex = 2;
                    break;

            }
        }
    }
}
