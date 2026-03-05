using Tufanlar.UI.ViewModels;

namespace Tufanlar.UI.Views
{
    public partial class DashboardPage : ContentPage
    {
       
            // ARTIK PARAMETRE ALIYORUZ
        public DashboardPage(string gelenAdSoyad, string gelenGorev)
        {
            InitializeComponent();

            // Gelen veriyi ViewModel'e (Beyne) gönderiyoruz
            this.BindingContext = new DashboardViewModel(gelenAdSoyad, gelenGorev);
        }
    }
    }
