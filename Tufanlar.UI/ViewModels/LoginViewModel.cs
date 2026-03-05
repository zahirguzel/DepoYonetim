using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Tufanlar.UI.Views; // DashboardPage'e gitmek için gerekli

namespace Tufanlar.UI.ViewModels
{
    public partial class LoginViewModel : INotifyPropertyChanged
    {
        // Değişkenler (Başlangıçta boş olsunlar ki null hatası almayalım)
        private string _kullaniciAdi = string.Empty;
        private string _sifre = string.Empty;
        private string _mesaj = string.Empty;

        // Ekrana Bağlanan Özellikler (Properties)
        public string KullaniciAdi
        {
            get => _kullaniciAdi;
            set { _kullaniciAdi = value; OnPropertyChanged(); }
        }

        public string Sifre
        {
            get => _sifre;
            set { _sifre = value; OnPropertyChanged(); }
        }

        public string Mesaj
        {
            get => _mesaj;
            set { _mesaj = value; OnPropertyChanged(); }
        }

        // Butonun Tetikleyeceği Komut
        public ICommand GirisYapKomutu { get; }

        // Kurucu Metot (Sınıf ilk oluştuğunda çalışır)
        public LoginViewModel()
        {
            GirisYapKomutu = new Command(GirisYap);
        }

        // Giriş Yapma Mantığı
        private void GirisYap()
        {
            // Basit Admin Kontrolü
            if (KullaniciAdi == "admin" && Sifre == "123")
            {
                Mesaj = "Giriş Başarılı! Yönlendiriliyorsunuz...";

                // Giriş yapan kişinin bilgileri (İleride API'den gelecek)
                string adSoyad = "Zahir Güzel";
                string gorev = "Yönetici";

                // Ana Sayfaya Yönlendir ve Parametreleri Gönder
                if (Application.Current?.MainPage != null)
                {
                    // NavigationPage içine DashboardPage'i koyuyoruz ve bilgileri gönderiyoruz
                    Application.Current.MainPage = new NavigationPage(new DashboardPage(adSoyad, gorev));
                }
            }
            else
            {
                Mesaj = "Hatalı Kullanıcı Adı veya Şifre!";
            }
        }

        // --- BEYİN (ViewModel) ile GÖZÜN (View) Haberleşmesi İçin Gerekli Kodlar ---
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}