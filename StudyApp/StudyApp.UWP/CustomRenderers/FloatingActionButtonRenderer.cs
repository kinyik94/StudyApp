using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(StudyApp.CustomControls.FloatingActionButton), typeof(StudyApp.UWP.CustomRenderers.FloatingActionButtonRenderer))]
namespace StudyApp.UWP.CustomRenderers
{
    class FloatingActionButtonRenderer : ViewRenderer<CustomControls.FloatingActionButton, Button>
    {
        Button button;
        CustomControls.FloatingActionButton ctrl;

        protected override void OnElementChanged(ElementChangedEventArgs<CustomControls.FloatingActionButton> e)
        {
            base.OnElementChanged(e);

            if (Control == null && e.NewElement != null)
            {
                button = new Button();
                var t = new ControlTemplate(); ;
                t.TargetType = typeof(Button);
                button.Template = t;

                Grid grid = new Grid();

                Ellipse ellipse = new Ellipse();
                ellipse.Height = 56;
                ellipse.Width = 56;
                ellipse.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 255, 255));

                grid.Children.Add(ellipse);

                Image icon = new Image();
                BitmapImage bitmapImage = new BitmapImage();
                Uri uri = new Uri("ms-appx:///Images/" + e.NewElement.Icon + ".png");
                bitmapImage.UriSource = uri;
                icon.Source = bitmapImage;
                icon.Height = 24;
                icon.Width = 24;

                grid.Children.Add(icon);

                button.Content = grid;
                SetNativeControl(button);
            }

            if (e.OldElement != null)
            {
                // Unsubscribe
                button.Click -= OnFABClicked;
                ctrl = null;
            }
            if (e.NewElement != null)
            {
                // Subscribe
                button.Click += OnFABClicked;
                ctrl = e.NewElement;
            }
        }

        void OnFABClicked(object sender, RoutedEventArgs e)
        {
            ctrl.TransitionCommand.Execute(null);
        }
    }
}
