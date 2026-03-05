using System.Runtime.Versioning; // 1. Bu kütüphane þart!
using ZXing.Net.Maui;

namespace Tufanlar.UI.Views;

// Sýnýf seviyesinde bu sayfanýn Android ve Windows uyumlu olduðunu ilan ediyoruz
[SupportedOSPlatform("android")]
[SupportedOSPlatform("windows")]
[SupportedOSPlatform("ios")]
public partial class BarkodTaraPage : ContentPage
{
    private bool _isProcessing = false;
    private readonly Action<string> _onBarcodeDetected;

    public BarkodTaraPage(Action<string> onBarcodeDetected)
    {
        InitializeComponent();
        _onBarcodeDetected = onBarcodeDetected;
    }

    private async void CameraView_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        // 25. satýr ve civarý: Gereksiz iþlemci yükünü ve çakýþmayý önleme
        if (_isProcessing) return;

        // 30-31. satýr: Barkod sonuçlarýný güvenli bir þekilde alma
        var firstBarcode = e.Results?.FirstOrDefault();
        if (firstBarcode == null) return;

        _isProcessing = true;

        // 34. satýr: Kamerayý ana arayüz kanalýnda (MainThread) güvenle durdurma
        MainThread.BeginInvokeOnMainThread(() => {
            cameraView.IsDetecting = false;
        });

        // 36. satýr: Okunan deðeri deðiþkene atama
        string barkodDegeri = firstBarcode.Value;

        // 39-45. satýr: Sayfayý kapatýp veriyi geri gönderme
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            _onBarcodeDetected?.Invoke(barkodDegeri);

            if (Navigation.ModalStack.Count > 0)
            {
                await Navigation.PopModalAsync();
            }
        });
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (Navigation.ModalStack.Count > 0)
        {
            await Navigation.PopModalAsync();
        }
    }
}