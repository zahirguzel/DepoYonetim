using Tufanlar.UI.ViewModels;

namespace Tufanlar.UI.Views;

public partial class KantarPage : ContentPage
{
    public KantarPage()
    {
        InitializeComponent();
        BindingContext = new KantarViewModel();
    }
}