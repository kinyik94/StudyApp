using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(StudyApp.CustomControls.FloatingActionButton), typeof(StudyApp.Droid.CustomRenderers.FloatingActionButtonRenderer))]
namespace StudyApp.Droid.CustomRenderers
{
    class FloatingActionButtonRenderer : ViewRenderer<CustomControls.FloatingActionButton, FloatingActionButton>
    {
        FloatingActionButton FAB;
        CustomControls.FloatingActionButton ctrl;

        public FloatingActionButtonRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CustomControls.FloatingActionButton> e)
        {
            base.OnElementChanged(e);

            if (Control == null && e.NewElement != null)
            {
                FAB = new FloatingActionButton(Context);
                FAB.Id = GenerateViewId();

                string res = e.NewElement.Icon;
                var res_id = Resources.GetIdentifier(res, "drawable", "com.Study.StudyApp");
                FAB.SetImageResource(res_id);
                FAB.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Aqua);
                SetNativeControl(FAB);
            }

            if (e.OldElement != null)
            {
                // Unsubscribe
                FAB.Click -= OnFABClicked;
                ctrl = null;
            }
            if (e.NewElement != null)
            {
                // Subscribe
                FAB.Click += OnFABClicked;
                ctrl = e.NewElement;
            }
        }

        void OnFABClicked(object sender, EventArgs e)
        {
            ctrl.Command.Execute(ctrl.CommandParameter);
        }
    }
}