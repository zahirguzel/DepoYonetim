using System.Windows.Input;
using Tufanlar.UI.Views;

namespace Tufanlar.UI.ViewModels;

// Primary Constructor: Parametre gerekmiyorsa boş bırakabiliriz
public partial class YonetimPaneliViewModel()
{
    // Ürün Listesi Sayfasına Gitme Komutu
    public ICommand UrunlereGitKomutu => new Command(async () =>
    {
        if (Application.Current?.MainPage is not null)
        {
            // UrunListesiPage sayfasına yönlendiriyoruz
            await Application.Current.MainPage.Navigation.PushAsync(new UrunListesiPage());
        }
    });

    // Diğer butonlar (Stok, Müşteri vb.) için komutları buraya ekleyeceğiz
    public ICommand StokTakibineGitKomutu => new Command(() => { /* Gelecek... */ });
    public ICommand MusterilereGitKomutu => new Command(() => { /* Gelecek... */ });
    public ICommand RaporlaraGitKomutu => new Command(() => { /* Gelecek... */ });
}