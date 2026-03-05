using Tufanlar.UI.ViewModels;

namespace Tufanlar.UI.Views;

public partial class DepoPage : ContentPage
{
    public DepoPage()
    {
        InitializeComponent();
        // Sayfa açýldýðýnda verilerin yüklenmesi için ViewModel'i baðlýyoruz
        BindingContext = new DepoViewModel();
    }
}