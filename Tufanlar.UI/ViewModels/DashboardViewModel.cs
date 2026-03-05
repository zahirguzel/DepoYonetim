using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Tufanlar.UI.Views;

namespace Tufanlar.UI.ViewModels
{
    public partial class DashboardViewModel : INotifyPropertyChanged
    {
        // --- GÖRÜNÜM AYARLARI ---
        private string _adSoyad = string.Empty;
        public string AdSoyad
        {
            get => _adSoyad;
            set { _adSoyad = value; OnPropertyChanged(); }
        }

        // Düğmelerin Görünürlük Ayarları
        private bool _isYonetici;
        public bool IsYonetici
        {
            get => _isYonetici;
            set { _isYonetici = value; OnPropertyChanged(); }
        }

        private bool _isKantar;
        public bool IsKantar
        {
            get => _isKantar;
            set { _isKantar = value; OnPropertyChanged(); }
        }

        private bool _isDepo;
        public bool IsDepo
        {
            get => _isDepo;
            set { _isDepo = value; OnPropertyChanged(); }
        }

        public ICommand KantarSayfasinaGitKomutu { get; }
        public ICommand DepoSayfasinaGitKomutu { get; }

        public DashboardViewModel(string adSoyad, string gorev)
        {
            AdSoyad = adSoyad;

            // --- YETKİ KONTROLÜ BURADA BAŞLIYOR ---
            // Gelen göreve göre butonların IsVisible (Görünürlük) değerini ayarlıyoruz
            if (gorev == "Yönetici")
            {
                IsYonetici = true;
                IsKantar = true;
                IsDepo = true;
            }
            else if (gorev == "Kantar Memuru")
            {
                IsKantar = true;
                IsYonetici = false;
                IsDepo = false;
            }
            else if (gorev == "Depo Sorumlusu")
            {
                IsDepo = true;
                IsYonetici = false;
                IsKantar = false;
            }
            else
            {
                // Bilinmeyen bir görev gelirse güvenlik için her şeyi gizle
                IsYonetici = IsKantar = IsDepo = false;
            }

            KantarSayfasinaGitKomutu = new Command(KantarSayfasinaGit);
            DepoSayfasinaGitKomutu = new Command(DepoSayfasinaGit);
        }

        private async void KantarSayfasinaGit()
        {
            if (Application.Current?.MainPage != null)
                await Application.Current.MainPage.Navigation.PushAsync(new KantarPage());
        }

        private async void DepoSayfasinaGit()
        {
            if (Application.Current?.MainPage != null)
                await Application.Current.MainPage.Navigation.PushAsync(new DepoPage());
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // DashboardViewModel.cs içine eklenecek komut
        public ICommand YoneticiPanelineGitKomutu => new Command(async () => {
            if (Application.Current?.MainPage is not null)
                await Application.Current.MainPage.Navigation.PushAsync(new YonetimPaneliPage());
        });
    }
}