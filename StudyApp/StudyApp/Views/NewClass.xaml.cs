using StudyApp.ViewModels;
using System;
using Xamarin.Forms;

namespace StudyApp.Views
{
    public partial class NewClass : ContentPage
    {
        public NewClass()
        {
            InitializeComponent();
        }

        async void OnDeleteClicked(object sender, EventArgs args)
        {
            var vm = ((NewClassViewModel)BindingContext);
            var answer = await DisplayAlert(Localization.LocalizationResources.Warning,
                Localization.LocalizationResources.DeleteWarning,
                Localization.LocalizationResources.Yes,
                Localization.LocalizationResources.No);
            if (answer && vm.DeleteCommand.CanExecute())
                vm.DeleteCommand.Execute();
        }
    }
}
