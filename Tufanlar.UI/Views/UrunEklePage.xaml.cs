using Tufanlar.UI.ViewModels;
using Tufanlar.UI.Services;
using Tufanlar.UI.Dtos;

namespace Tufanlar.UI.Views;

public partial class UrunEklePage : ContentPage
{
    // Opsiyonel parametre ekledik: urun gelirse düzenleme, gelmezse yeni kayýt.
    public UrunEklePage(UrunDto? urun = null)
    {
        InitializeComponent();

        // ViewModel'e hem servisi hem de (varsa) ürünü gönderiyoruz
        this.BindingContext = new UrunEkleViewModel(new ApiService(), urun);

        // Eðer düzenleme modundaysak baþlýðý deðiþtirelim
        Title = urun == null ? "Yeni Ürün Kaydý" : "Ürün Düzenle";
    }
}