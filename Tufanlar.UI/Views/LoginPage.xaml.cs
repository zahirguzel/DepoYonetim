using Tufanlar.UI.ViewModels;

namespace Tufanlar.UI.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        // Ekranýn arkasýndaki beyin LoginViewModel'dir diyoruz.
        BindingContext = new LoginViewModel();
    }
}