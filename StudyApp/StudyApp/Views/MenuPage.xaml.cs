using StudyApp.Views;
using Xamarin.Forms;

namespace StudyApp.Views
{
    public partial class MenuPage : MasterDetailPage
    {
        public MenuPage()
        {
            InitializeComponent();
            Detail = new NavigationPage(new MainPage());
        }

        public bool IsPresentedAfterNavigation => Device.Idiom != TargetIdiom.Phone;
    }
}