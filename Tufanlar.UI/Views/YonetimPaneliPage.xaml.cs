using Tufanlar.UI.ViewModels;

namespace Tufanlar.UI.Views;

public partial class YonetimPaneliPage : ContentPage
{
    public YonetimPaneliPage()
    {
        InitializeComponent();

        // BU SATIR KRÝTÝK: Komutlarýn çalýþmasý için beyni (ViewModel) baðlýyoruz.
        // Eðer ApiService kullanýyorsan: new YonetimPaneliViewModel(new ApiService())
        this.BindingContext = new YonetimPaneliViewModel();
    }
}