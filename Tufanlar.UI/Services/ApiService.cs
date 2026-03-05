using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Maui.Storage;
using Tufanlar.UI.Dtos;

namespace Tufanlar.UI.Services;

public class ApiService
{
    private const string ApiBaseUrlPreferenceKey = "api_base_url";
    private readonly HttpClient _httpClient;

    // JSON Ayarları
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ApiService()
    {
        // 1. SSL Hatasını Yoksay (Geliştirme ortamı için)
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };

        _httpClient = new HttpClient(handler);

        // 2. BAĞLANTI AYARLARI (KRİTİK BÖLÜM)

        string kayitliBaseUrl = Preferences.Default.Get(ApiBaseUrlPreferenceKey, string.Empty);

        string baseUrl = !string.IsNullOrWhiteSpace(kayitliBaseUrl)
            ? kayitliBaseUrl
            : DeviceInfo.Platform == DevicePlatform.Android
                ? "http://192.168.1.131:5000/api/"
                : "https://localhost:7274/api/";

        _httpClient.BaseAddress = new Uri(baseUrl);

        // Zaman aşımı süresini biraz artıralım (Telefon ağda yavaş kalabilir)
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
    }

    public static void ApiBaseUrlKaydet(string baseUrl)
    {
        if (string.IsNullOrWhiteSpace(baseUrl)) return;

        string normalized = baseUrl.Trim();
        if (!normalized.EndsWith('/')) normalized += "/";

        Preferences.Default.Set(ApiBaseUrlPreferenceKey, normalized);
    }

    // --- KANTAR İŞLEMLERİ ---

    public async Task<bool> TartimKaydetAsync(TartimKayitDto veri)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("Kantar/kaydet", veri);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"HATA: {ex.Message}");
            return false;
        }
    }

    public async Task<List<TartimKayitDto>> BekleyenTartimlariGetirAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("Kantar/bekleyenler");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<TartimKayitDto>>(_options) ?? [];
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"HATA: {ex.Message}");
        }
        return [];
    }

    public async Task<bool> TartimOnaylaAsync(int id)
    {
        try
        {
            var response = await _httpClient.PostAsync($"Kantar/onayla/{id}", null);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    // --- ÜRÜN İŞLEMLERİ ---

    public async Task<List<UrunDto>> UrunleriGetirAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("Urun");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<UrunDto>>(_options) ?? [];
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"HATA: {ex.Message}");
        }
        return [];
    }

    // Bu metot hata mesajını string olarak döner (UrunEkleViewModel kullanır)
    public async Task<(bool IsSuccess, string ErrorMessage)> UrunEkleDetayliAsync(UrunDto urun)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("Urun", urun);
            if (response.IsSuccessStatusCode)
            {
                return (true, string.Empty);
            }
            else
            {
                var hata = await response.Content.ReadAsStringAsync();
                return (false, hata);
            }
        }
        catch (Exception ex)
        {
            return (false, $"Bağlantı Hatası: {ex.Message}");
        }
    }

    // Eski basit metot (Geriye uyumluluk için)
    public async Task<bool> UrunEkleAsync(UrunDto urun)
    {
        var sonuc = await UrunEkleDetayliAsync(urun);
        return sonuc.IsSuccess;
    }

    // DETAYLI GÜNCELLEME METODU
    public async Task<(bool IsSuccess, string ErrorMessage)> UrunGuncelleDetayliAsync(UrunDto urun)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"Urun/{urun.Id}", urun);

            if (response.IsSuccessStatusCode)
            {
                return (true, string.Empty);
            }
            else
            {
                var hataMesaji = await response.Content.ReadAsStringAsync();
                return (false, hataMesaji);
            }
        }
        catch (Exception ex)
        {
            return (false, $"Bağlantı Hatası: {ex.Message}");
        }
    }

    public async Task<bool> UrunGuncelleAsync(UrunDto urun)
    {
        var sonuc = await UrunGuncelleDetayliAsync(urun);
        return sonuc.IsSuccess;
    }

    // --- CARİ (MÜŞTERİ) İŞLEMLERİ ---

    public async Task<List<CariDto>> CarileriGetirAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("Cariler");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<CariDto>>(_options) ?? [];
            }
        }
        catch { }
        return [];
    }

    public async Task<bool> CariEkleAsync(CariDto yeniCari)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("Cariler", yeniCari);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    // --- KATEGORİ İŞLEMLERİ ---

    public async Task<List<KategoriDto>> KategorileriGetirAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("Kategori");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<KategoriDto>>(_options) ?? [];
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Kategori Getirme Hatası: {ex.Message}");
        }
        return [];
    }

    public async Task<bool> KategoriEkleAsync(KategoriDto kategori)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("Kategori", kategori);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Kategori Ekleme Hatası: {ex.Message}");
            return false;
        }
    }
}
