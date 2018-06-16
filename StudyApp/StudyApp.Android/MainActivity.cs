using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using DryIoc;
using Prism;
using Prism.Ioc;
using StudyApp.Droid.Services;
using StudyApp.Services;

namespace StudyApp.Droid
{
    [Activity(Label = "StudyApp", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Container c = StudyApp.App.Dic;
            c.Register<IStudyNotifier, AndroidStudyNotifier>(Reuse.Singleton);
            c.Register<IFacebookAuthenticator, AndroidFacebookAuthenticator>(Reuse.Singleton);

            Intent intent = new Intent("StartStudyService");
            SendBroadcast(intent);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new AndroidInitializer()));
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry container)
        {
            // Register any platform specific implementations
        }
    }
}

