namespace Tufanlar.UI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // ESKİ KOD: MainPage = new AppShell();
            // YENİ KOD: Uygulama NavigationPage içinde LoginPage ile başlasın
            MainPage = new NavigationPage(new Views.LoginPage());
        }
    }
}