using ZXing.Net.Maui;

namespace Tufanlar.UI.Views
{
    public partial class BarkodTaraPage : ContentPage
    {
        private bool _isProcessing;
        private readonly Action<string> _onBarcodeDetected;

        public BarkodTaraPage(Action<string> onBarcodeDetected)
        {
            InitializeComponent();
            _onBarcodeDetected = onBarcodeDetected;

            cameraView.Options = new BarcodeReaderOptions
            {
                Formats = BarcodeFormats.All,
                AutoRotate = true,
                Multiple = false
            };
        }

        private async void CameraView_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
        {
            if (_isProcessing) return;

            var firstBarcode = e.Results?.FirstOrDefault();
            if (firstBarcode == null) return;

            _isProcessing = true;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                cameraView.IsDetecting = false;
            });

            string barkodDegeri = firstBarcode.Value;

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
}
