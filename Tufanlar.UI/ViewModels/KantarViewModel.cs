using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Tufanlar.UI.Dtos;
using Tufanlar.UI.Services;
// UrunSecimPage sayfasına erişmek için Views'i ekliyoruz
using Tufanlar.UI.Views;

namespace Tufanlar.UI.ViewModels
{
    public partial class KantarViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService = new ApiService();

        //giriş çokış alanaı

        // Varsayılan olarak Giriş seçili olsun
        private bool _isGiris = true;
        public bool IsGiris
        {
            get => _isGiris;
            set
            {
                _isGiris = value;
                OnPropertyChanged();
                // Giriş seçilirse Çıkış false olur (Otomatik)
                if (value) IsCikis = false;
            }
        }

        private bool _isCikis;
        public bool IsCikis
        {
            get => _isCikis;
            set
            {
                _isCikis = value;
                OnPropertyChanged();
                // Çıkış seçilirse Giriş false olur
                if (value) IsGiris = false;
            }
        }



        // --- GİRİŞ ALANLARI ---
        private string _plaka = string.Empty;
        public string Plaka
        {
            get => _plaka;
            set { _plaka = value; OnPropertyChanged(); }
        }

        private string _musteriAdi = string.Empty;
        public string MusteriAdi
        {
            get => _musteriAdi;
            set { _musteriAdi = value; OnPropertyChanged(); }
        }

        // --- TARTIM ALANLARI ---
        private double _ilkTartim;
        public double IlkTartim
        {
            get => _ilkTartim;
            set
            {
                _ilkTartim = value;
                OnPropertyChanged();
                Hesapla();
            }
        }

        private double _ikinciTartim;
        public double IkinciTartim
        {
            get => _ikinciTartim;
            set
            {
                _ikinciTartim = value;
                OnPropertyChanged();
                Hesapla();
            }
        }

        private double _netAgirlik;
        public double NetAgirlik
        {
            get => _netAgirlik;
            set { _netAgirlik = value; OnPropertyChanged(); }
        }

        private string _mesaj = string.Empty;
        public string Mesaj
        {
            get => _mesaj;
            set { _mesaj = value; OnPropertyChanged(); }
        }

        // --- ÜRÜN SEÇİM KISMI (ARAMA DESTEKLİ) ---

        // Ürün listesini burada tutuyoruz ama ekranda göstermiyoruz (Popup'a göndereceğiz)
        public ObservableCollection<UrunDto> UrunlerListesi { get; set; } = new();

        private UrunDto? _seciliUrun;
        public UrunDto? SeciliUrun
        {
            get => _seciliUrun;
            set
            {
                _seciliUrun = value;
                OnPropertyChanged();
            }
        }

        // 1. KOMUTLAR
        public ICommand KaydetKomutu { get; }
        public ICommand UrunSecimSayfasiniAcKomutu { get; } // Yeni Arama Komutu

        // --- KURUCU METOT ---
        public KantarViewModel()
        {
            KaydetKomutu = new Command(Kaydet);

            // Arama sayfasına gitme komutunu bağlıyoruz
            UrunSecimSayfasiniAcKomutu = new Command(UrunSecimSayfasiniAc);

            // Arka planda listeyi hazırla
            UrunleriYukle();
        }

        // --- METOTLAR ---

        // Arama Sayfasını Açan Metot
        private async void UrunSecimSayfasiniAc()
        {
            // Eğer liste boşsa önce çekmeye çalış
            if (UrunlerListesi.Count == 0)
            {
                var liste = await _apiService.UrunleriGetirAsync();
                foreach (var item in liste) UrunlerListesi.Add(item);
            }

            // Arama sayfasını oluştur ve aç
            // Parametre olarak listeyi ve seçim yapılınca çalışacak kodu gönderiyoruz
            var aramaSayfasi = new UrunSecimPage(UrunlerListesi.ToList(), (secilen) =>
            {
                // Kullanıcı seçim yapınca burası çalışır
                SeciliUrun = secilen;
            });

            await Application.Current!.MainPage!.Navigation.PushModalAsync(aramaSayfasi);
        }

        public async void UrunleriYukle()
        {
            var liste = await _apiService.UrunleriGetirAsync();
            UrunlerListesi.Clear();
            foreach (var item in liste)
            {
                UrunlerListesi.Add(item);
            }
        }

        private void Hesapla()
        {
            if (IkinciTartim > IlkTartim)
                NetAgirlik = IkinciTartim - IlkTartim;
            else
                NetAgirlik = 0;
        }

        private async void Kaydet()
        {
            // Doğrulama
            if (string.IsNullOrEmpty(Plaka) || string.IsNullOrEmpty(MusteriAdi))
            {
                Mesaj = "Lütfen plaka ve müşteri adını giriniz!";
                return;
            }

            if (SeciliUrun == null)
            {
                await Application.Current!.MainPage!.DisplayAlert("Uyarı", "Lütfen taşınan malı seçiniz!", "Tamam");
                return;
            }

            Mesaj = "Kaydediliyor...";

            // Veriyi Hazırla
            var veri = new TartimKayitDto
            {
                Plaka = this.Plaka,
                MusteriAdi = this.MusteriAdi,
                IlkTartim = this.IlkTartim,
                IkinciTartim = this.IkinciTartim,
                NetAgirlik = this.NetAgirlik,
                UrunId = SeciliUrun.Id,

                // BURASI DEĞİŞTİ: Hangi kutucuk seçiliyse ona göre işlem türü belirle
                IslemTuru = IsGiris ? "ALIS" : "SATIS"
            };
            // API'ye Gönder
            bool sonuc = await _apiService.TartimKaydetAsync(veri);

            if (sonuc)
            {
                Mesaj = "✅ Kayıt Başarılı!";

                // Temizlik
                Plaka = "";
                MusteriAdi = "";
                IlkTartim = 0;
                IkinciTartim = 0;
                SeciliUrun = null;
            }
            else
            {
                Mesaj = "❌ Hata! Kayıt yapılamadı.";
            }
        }

        // --- Boilerplate ---
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}