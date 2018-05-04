using StudyApp.ViewModels;
using System;
using Xamarin.Forms;

namespace StudyApp.Views
{
    public partial class NewSubject : ContentPage
    {
        public NewSubject()
        {
            InitializeComponent();
        }

        async void OnSemesterMenuClicked(object sender, EventArgs args)
        {
            var vm = ((NewSubjectViewModel)BindingContext);
            string action = await DisplayActionSheet("", "Cancel", null, "Edit Semester", "New Semester");

            if (action != "Cancel" && vm.MenuCommand.CanExecute(action))
                vm.MenuCommand.Execute(action);
        }
    }
}
