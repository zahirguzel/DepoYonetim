namespace Tufanlar.API.Dtos;


public class UrunDto
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;
    public string Barkod { get; set; } = string.Empty;

    // --- DEĞİŞİKLİK ---
    public int KategoriId { get; set; } // Artık ID taşıyoruz
    public string? KategoriAdi { get; set; } // Listede göstermek için (API'den dolup gelecek)
    // ------------------

    public string? Aciklama { get; set; }
    public string Birim { get; set; } = "KG";

    // ... Diğer alanlar aynı kalsın (Fiyat, Stok vb.) ...
    public decimal AlisFiyati { get; set; }
    public decimal SatisFiyati { get; set; }
    public double KdvOrani { get; set; }
    public double StokMiktari { get; set; }
    public double KritikStokSeviyesi { get; set; }
}