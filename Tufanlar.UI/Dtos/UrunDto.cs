namespace Tufanlar.UI.Dtos;

public class UrunDto
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;
    public string Barkod { get; set; } = string.Empty;

    // --- İLİŞKİSEL YAPI GÜNCELLEMESİ ---
    public int KategoriId { get; set; }

    // API'den "Kategori": { "id": 1, "ad": "Gıda" } şeklinde gelen veriyi tutar
    public KategoriDto? Kategori { get; set; }
    // -----------------------------------

    public string? Aciklama { get; set; }
    public string Birim { get; set; } = "KG";
    public decimal AlisFiyati { get; set; }
    public decimal SatisFiyati { get; set; }
    public double KdvOrani { get; set; }
    public double StokMiktari { get; set; }
    public double KritikStokSeviyesi { get; set; }
}