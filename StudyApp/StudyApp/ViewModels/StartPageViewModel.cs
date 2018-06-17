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
        private bool _inLogin = false;
        public DelegateCommand GeneralCommand { get; }
        private async void ExecuteGeneralCommand()
        {
            if (!_inLogin)
            {
                await NavigationService.NavigateAsync("/MenuPage/Navigation/Dashboard");
                try
                {
                    App.Dic.Resolve<IStudyNotifier>().StartNotify();
                }
                catch { }
            }
        }

        public DelegateCommand LoginCommand { get; }
        private async void ExecuteLoginCommand()
        {
            if (!_inLogin)
            {
                _inLogin = true;
                var auth = App.Dic.Resolve<IFacebookAuthenticator>();
                await auth.Authenticate(async (string json) =>
                {
                    try
                    {
                        var obj = JsonValue.Parse(json);
                        string id = obj["id"];
                        string name = obj["name"];
                        _inLogin = false;
                        await NavigationService.NavigateAsync("/MenuPage?name=" + name + "/Navigation/Dashboard?id=" + id);
                        try
                        {
                            App.Dic.Resolve<IStudyNotifier>().StartNotify();
                        }
                        catch { }
                    }
                    catch { }
                });
            }
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            try
            {
                App.Dic.Resolve<IStudyNotifier>().StopNotify();
            }
            catch { }
        }

        public StartPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "StartPage";
            GeneralCommand = new DelegateCommand(ExecuteGeneralCommand);
            LoginCommand = new DelegateCommand(ExecuteLoginCommand);
        }
	}
}
