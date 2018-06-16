using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using StudyApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using DryIoc;
using System.Json;

namespace StudyApp.ViewModels
{
	public class StartPageViewModel : ViewModelBase
	{
        public DelegateCommand GeneralCommand { get; }
        private async void ExecuteGeneralCommand()
        {
            await NavigationService.NavigateAsync("/MenuPage/Navigation/Dashboard");
        }

        public DelegateCommand LoginCommand { get; }
        private async void ExecuteLoginCommand()
        {
            var auth = App.Dic.Resolve<IFacebookAuthenticator>();
            await auth.Authenticate(async (string id) =>
            {
                try
                {
                    await NavigationService.NavigateAsync("/MenuPage/Navigation/Dashboard");
                }
                catch{}
            });
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }


        public StartPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "StartPage";
            Email = "";
            GeneralCommand = new DelegateCommand(ExecuteGeneralCommand);
            LoginCommand = new DelegateCommand(ExecuteLoginCommand);
        }
	}
}
