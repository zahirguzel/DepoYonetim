using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Tufanlar.UI.Dtos;
using Tufanlar.UI.Services;

namespace Tufanlar.UI.ViewModels
{
    // 1. DÜZELTME: 'partial' kelimesi eklendi
    public partial class DepoViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService = new ApiService();

        // Listemiz
        public ObservableCollection<TartimKayitDto> BekleyenAraclar { get; set; } = new();

        private string _durumMesaji = string.Empty;
        public string DurumMesaji
        {
            get => _durumMesaji;
            set { _durumMesaji = value; OnPropertyChanged(); }
        }

        public ICommand YenileKomutu { get; }
        public ICommand OnaylaKomutu { get; }

        public DepoViewModel()
        {
            YenileKomutu = new Command(VerileriGetir);
            OnaylaKomutu = new Command<TartimKayitDto>(Onayla);

            // Sayfa açılınca verileri çek
            VerileriGetir();
        }

        private async void VerileriGetir()
        {
            DurumMesaji = "Yükleniyor...";

            // API'den verileri çekiyoruz
            var liste = await _apiService.BekleyenTartimlariGetirAsync();

            BekleyenAraclar.Clear();
            foreach (var item in liste)
            {
                BekleyenAraclar.Add(item);
            }

            // 2. DÜZELTME: 'list' yerine 'liste' yazdık
            DurumMesaji = liste.Count > 0 ? "" : "Bekleyen araç yok.";
        }

        private async void Onayla(TartimKayitDto? secilenArac)
        {
            if (secilenArac == null) return;

            // 3. DÜZELTME: 'await' kelimesi burada kesinlikle olmalı
            bool sonuc = await _apiService.TartimOnaylaAsync(secilenArac.Id);

            if (sonuc)
            {
                DurumMesaji = $"{secilenArac.Plaka} depoya alındı.";
                VerileriGetir(); // Listeyi yenile
            }
            else
            {
                DurumMesaji = "Hata oluştu, işlem yapılamadı!";
            }
        }

        // Standart PropertyChanged kodu
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}