using Tufanlar.UI.ViewModels;
using Tufanlar.UI.Services;

namespace Tufanlar.UI.Views;

public partial class UrunListesiPage : ContentPage
{
    public UrunListesiPage()
    {
        InitializeComponent();

        // ViewModel baðlantýsý (ApiService ile birlikte)
        this.BindingContext = new UrunListesiViewModel(new ApiService());
    }

    // Sayfa ekrana her geldiðinde verileri API'den tazele
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is UrunListesiViewModel vm)
        {
            await vm.VerileriYukleAsync();
        }
    }
}