using StudyApp.Models;
using StudyApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static StudyApp.ViewModels.MainPageViewModel;

namespace StudyApp.Views
{
	public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
			InitializeComponent();
        }

        void OnItemTapped(object sender, EventArgs args)
        {
            var cmd = ((MainPageViewModel)BindingContext).ItemSelectCommand;
            ItemTappedEventArgs eargs = (ItemTappedEventArgs)args;
            string id = ((ItemModelInterface)(eargs.Item)).GetID().ToString();
            string type = ((ItemCollection)eargs.Group).Name;
            if (cmd.CanExecute(new string[2]{ id, type}))
                cmd.Execute(new string[2] { id, type });
        }

        protected override void OnAppearing()
        {
            DashboardList.SelectedItem = null;
        }
    }
}