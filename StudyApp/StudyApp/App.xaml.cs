using Prism;
using Prism.Ioc;
using StudyApp.ViewModels;
using StudyApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.DryIoc;
using DryIoc;
using System.Globalization;
using System;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace StudyApp
{
    public partial class App : PrismApplication
    {

        public static Container _dic = null;
        
        public static string UserId { get; set; }

        public static Container Dic 
        {
            get { if (_dic == null) _dic = new Container(); return _dic; }
            private set { }
        }

        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null){}

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            var LanguageCode = "en";
            var platformCultureString = CultureInfo.CurrentCulture.ToString();
            var PlatformString = platformCultureString.Replace("_", "-"); // .NET expects dash, not underscore
            var dashIndex = PlatformString.IndexOf("-", StringComparison.Ordinal);
            if (dashIndex > 0)
            {
                var parts = PlatformString.Split('-');
                LanguageCode = parts[0];
            }
            else
            {
                LanguageCode = PlatformString;
            }

            Localization.LocalizationResources.Culture = new CultureInfo(LanguageCode);

            UserId = "0";

            await NavigationService.NavigateAsync("StartPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>("Navigation");
            containerRegistry.RegisterForNavigation<MainPage>("Dashboard");
            containerRegistry.RegisterForNavigation<MenuPage>();
            containerRegistry.RegisterForNavigation<Items>();
            containerRegistry.RegisterForNavigation<Subjects>();
            containerRegistry.RegisterForNavigation<NewItem>();
            containerRegistry.RegisterForNavigation<NewSubject>();
            containerRegistry.RegisterForNavigation<NewClass>();
            containerRegistry.RegisterForNavigation<NewTask>();
            containerRegistry.RegisterForNavigation<NewExam>();
            containerRegistry.RegisterForNavigation<NewSemester>();
            containerRegistry.RegisterForNavigation<StartPage>();
        }
    }
}
