using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Runtime.Versioning;
using Tufanlar.UI.Dtos;
using Tufanlar.UI.Services;

namespace Tufanlar.UI.ViewModels;

[SupportedOSPlatform("android")]
[SupportedOSPlatform("ios")]
[SupportedOSPlatform("windows")]
public partial class UrunEkleViewModel(ApiService apiService, UrunDto? urun = null) : INotifyPropertyChanged
{
    private readonly ApiService _apiService = apiService;
    public UrunDto Urun { get; set; } = urun ?? new UrunDto();
    public ObservableCollection<KategoriDto> Kategoriler { get; } = [];

    // --- KDV YÖNETİMİ (81. Satırdaki Hatayı Çözen Kısım) ---
    public List<double> KdvOranlari { get; } = [1, 10, 20];

    public double SecilenKdv
    {
        get => Urun.KdvOrani;
        set
        {
            if (Urun.KdvOrani != value)
            {
                Urun.KdvOrani = value;
                OnPropertyChanged();
            }
        }
    }

    // --- KATEGORİ YÖNETİMİ ---
    private KategoriDto? _secilenKategoriNesnesi;
    public KategoriDto? SecilenKategoriNesnesi
    {
        get => _secilenKategoriNesnesi;
        set
        {
            if (_secilenKategoriNesnesi != value)
            {
                _secilenKategoriNesnesi = value;
                if (value != null) Urun.KategoriId = value.Id;
                OnPropertyChanged();
            }
        }
    }

    public async Task KategorileriYukle()
    {
        var liste = await _apiService.KategorileriGetirAsync();
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Kategoriler.Clear();
            foreach (var item in liste) Kategoriler.Add(item);
            if (Urun.KategoriId > 0)
                SecilenKategoriNesnesi = Kategoriler.FirstOrDefault(k => k.Id == Urun.KategoriId);
        });
    }

    // --- KOMUTLAR (XAML 52. Satır İçin) ---
    public ICommand YeniKategoriEkleKomutu => new Command(async () =>
    {
        if (Application.Current?.MainPage == null) return;
        string ad = await Application.Current.MainPage.DisplayPromptAsync("Yeni Kategori", "Kategori Adı:");
        if (!string.IsNullOrWhiteSpace(ad))
        {
            var sonuc = await _apiService.KategoriEkleAsync(new KategoriDto { Ad = ad.Trim() });
            if (sonuc) await KategorileriYukle();
        }
    });

    // --- BARKOD İŞLEMLERİ ---
    public ICommand BarkodOkuKomutu => new Command(async () =>
    {
        var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
            status = await Permissions.RequestAsync<Permissions.Camera>();

        if (status == PermissionStatus.Granted && Application.Current?.MainPage != null)
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new Views.BarkodTaraPage((okunanKod) =>
            {
                Urun.Barkod = okunanKod;
                OnPropertyChanged(nameof(Urun));
            }));
            return;
        }

        if (Application.Current?.MainPage != null)
        {
            await Application.Current.MainPage.DisplayAlert("Kamera İzni", "QR/Barkod okutmak için kamera izni vermelisiniz.", "Tamam");
        }
    });

    public ICommand BarkodOlusturKomutu => new Command(() =>
    {
        Urun.Barkod = new Random().Next(10000000, 99999999).ToString();
        OnPropertyChanged(nameof(Urun));
    });

    public ICommand KaydetKomutu => new Command(async () => await UrunuKaydet());

    private async Task UrunuKaydet()
    {
        if (Application.Current?.MainPage == null) return;

        if (string.IsNullOrWhiteSpace(Urun.Ad) || Urun.KategoriId == 0)
        {
            await Application.Current.MainPage.DisplayAlert("Hata", "Ad ve Kategori zorunludur.", "Tamam");
            return;
        }

        bool basarili = Urun.Id == 0
            ? await _apiService.UrunEkleAsync(Urun)
            : (await _apiService.UrunGuncelleDetayliAsync(Urun)).IsSuccess;

        if (basarili)
        {
            await Application.Current.MainPage.DisplayAlert("Zak Yazılım", "Başarıyla Kaydedildi!", "Tamam");
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name ?? string.Empty));
}