﻿using StudyApp.Models;
using StudyApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudyApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Items : ContentPage
	{
		public Items ()
		{
			InitializeComponent ();
        }
        
        void OnItemTapped(object sender, EventArgs args)
        {
            var cmd = ((ItemsViewModel)BindingContext).ItemSelectCommand;
            ItemTappedEventArgs eargs = (ItemTappedEventArgs)args;
            if (eargs != null)
            {
                string id = ((ItemModelInterface)(eargs.Item)).GetID().ToString();
                if (cmd.CanExecute(id))
                    cmd.Execute(id);
            }
        }
    }
}