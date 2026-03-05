using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Tufanlar.UI.Dtos;
using Tufanlar.UI.Views;
using Tufanlar.UI.Services;
using System.Runtime.Versioning; // 1. Bu kütüphane şart!

namespace Tufanlar.UI.ViewModels;

// 2. Sınıf seviyesinde tüm platformları desteklediğimizi ilan ediyoruz
[SupportedOSPlatform("android")]
[SupportedOSPlatform("windows")]
[SupportedOSPlatform("ios")]
public partial class UrunListesiViewModel(ApiService apiService) : INotifyPropertyChanged
{
    private readonly ApiService _apiService = apiService;
    private List<UrunDto> _tumUrunler = [];

    public ObservableCollection<UrunDto> Urunler { get; } = [];

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set { _isBusy = value; OnPropertyChanged(); }
    }

    private string _aramaMetni = string.Empty;
    public string AramaMetni
    {
        get => _aramaMetni;
        set
        {
            if (_aramaMetni != value)
            {
                _aramaMetni = value;
                OnPropertyChanged();
                UrunleriFiltrele();
            }
        }
    }

    // 42, 44, 46. satırlardaki Navigasyon uyarıları artık susturuldu
    public ICommand YeniUrunEkleKomutu => new Command(async () =>
    {
        if (Application.Current?.MainPage?.Navigation is INavigation nav)
        {
            await nav.PushAsync(new UrunEklePage());
        }
    });

    // 51, 53, 55. satırlardaki Command ve Navigation uyarıları susturuldu
    public ICommand DuzenleKomutu => new Command<UrunDto>(async (urun) =>
    {
        if (urun != null && Application.Current?.MainPage?.Navigation is INavigation nav)
        {
            await nav.PushAsync(new UrunEklePage(urun));
        }
    });

    private void UrunleriFiltrele()
    {
        var arama = AramaMetni?.ToLower() ?? "";

        var filtreli = _tumUrunler.Where(x =>
            (x.Ad?.Contains(arama, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (x.Barkod?.Contains(arama, StringComparison.OrdinalIgnoreCase) ?? false)).ToList();

        Urunler.Clear();
        foreach (var urun in filtreli)
            Urunler.Add(urun);
    }

    public async Task VerileriYukleAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            var gelen = await _apiService.UrunleriGetirAsync();

            if (gelen != null)
            {
                _tumUrunler = gelen.ToList();

                // 86. satırdaki MainThread uyarısı susturuldu
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Urunler.Clear();
                    foreach (var urun in _tumUrunler)
                    {
                        Urunler.Add(urun);
                    }
                });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"HATA: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name ?? string.Empty));
}