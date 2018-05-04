using StudyApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace StudyApp.Views
{

    public partial class NewItem : TabbedPage
    {
        public NewItem()
        {
            InitializeComponent();
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            ((NewItemViewModel)BindingContext).Title = CurrentPage.Title;
        }

        protected override void OnAppearing()
        {
            CurrentPage = Children[((NewItemViewModel)BindingContext).ChildIndex];
            OnCurrentPageChanged();
        }
    }
}
