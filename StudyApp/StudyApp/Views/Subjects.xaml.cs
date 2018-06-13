using StudyApp.Models;
using StudyApp.ViewModels;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudyApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Subjects : ContentPage
    {
        public Subjects()
        {
            InitializeComponent();
        }

        async void OnSemesterMenuClicked(object sender, EventArgs args)
        {
            var vm = ((SubjectsViewModel)BindingContext);
            string action = await DisplayActionSheet("", Localization.LocalizationResources.Cancel, null,
                Localization.LocalizationResources.EditSemester,
                Localization.LocalizationResources.NewSemester);

            if (action != "Cancel" && vm.MenuCommand.CanExecute(action))
                vm.MenuCommand.Execute(action);
        }

        void OnSubjectTapped(object sender, EventArgs args)
        {
            var cmd = ((SubjectsViewModel)BindingContext).SubjectSelectCommand;
            ItemTappedEventArgs eargs = (ItemTappedEventArgs)args;
            if (eargs != null)
            {
                string id = ((SubjectModel)(eargs.Item)).ID.ToString();
                if (cmd.CanExecute(id))
                    cmd.Execute(id);
            }
        }

    }
}
