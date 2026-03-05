using Tufanlar.UI.Dtos;

namespace Tufanlar.UI.Views
{
    public partial class UrunSecimPage : ContentPage
    {
        private List<UrunDto> _tumUrunler; // Listenin tamamýný burada tutacaðýz
        private Action<UrunDto> _secimYapildiCallback; // Seçimi geri göndermek için

        // Bu sayfayý açarken ona ürün listesini veriyoruz
        public UrunSecimPage(List<UrunDto> urunler, Action<UrunDto> callback)
        {
            InitializeComponent();
            _tumUrunler = urunler;
            _secimYapildiCallback = callback;

            // Ýlk açýlýþta hepsini göster
            MyCollectionView.ItemsSource = _tumUrunler;
        }

        // Kullanýcý harf yazdýkça çalýþýr
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var aranan = e.NewTextValue?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(aranan))
            {
                MyCollectionView.ItemsSource = _tumUrunler;
            }
            else
            {
                // Filtreleme: Ýçinde aranan kelime geçenleri bul
                MyCollectionView.ItemsSource = _tumUrunler
                    .Where(x => x.Ad.ToLower().Contains(aranan))
                    .ToList();
            }
        }

        // Listeden bir þeye týklayýnca çalýþýr
        private async void MyCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var secilen = e.CurrentSelection.FirstOrDefault() as UrunDto;
            if (secilen == null) return;

            // Seçimi ana sayfaya bildir
            _secimYapildiCallback?.Invoke(secilen);

            // Sayfayý kapat
            await Navigation.PopModalAsync();
        }
        // UrunSecimPage.xaml.cs içine ekle:

        private async void Kapat_Clicked(object sender, EventArgs e)
        {
            // Modal sayfayý kapatýr ve önceki ekrana döner
            await Navigation.PopModalAsync();
        }
    }
}