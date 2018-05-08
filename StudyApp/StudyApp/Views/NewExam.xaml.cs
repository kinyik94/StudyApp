using StudyApp.ViewModels;
using System;
using Xamarin.Forms;

namespace StudyApp.Views
{
    public partial class NewExam : ContentPage
    {
        public NewExam()
        {
            InitializeComponent();
        }

        async void OnDeleteClicked(object sender, EventArgs args)
        {
            var vm = ((NewExamViewModel)BindingContext);
            var answer = await DisplayAlert("Warning", "Are you sure you want to delete this item?", "Yes", "No");
            if (answer && vm.DeleteCommand.CanExecute())
                vm.DeleteCommand.Execute();
        }
    }
}
