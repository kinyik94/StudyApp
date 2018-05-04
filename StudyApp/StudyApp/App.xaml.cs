using Prism;
using Prism.Ioc;
using StudyApp.ViewModels;
using StudyApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.DryIoc;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace StudyApp
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("MenuPage/Navigation/Dashboard");
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
        }
    }
}
