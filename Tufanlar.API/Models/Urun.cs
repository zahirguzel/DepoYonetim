using Tufanlar.API.Models;

public class Urun
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;
    public string Barkod { get; set; } = string.Empty;
    public string UrunKodu { get; set; } = string.Empty; // UI DTO'sunda da olmalı!
    public string Birim { get; set; } = "KG";
    public int KategoriId { get; set; }

    // Bu "Navigation Property" sayesinde ürünün kategorisine erişebileceğiz
    // JsonIgnore ekliyoruz ki döngüye girmesin
    [System.Text.Json.Serialization.JsonIgnore]
    public Kategori? Kategori { get; set; }
    public string? Aciklama { get; set; }

    public decimal AlisFiyati { get; set; }
    public decimal SatisFiyati { get; set; }
    public double KdvOrani { get; set; } = 20; // UI'da eklemiştik, buraya da şart!

    public double StokMiktari { get; set; }
    public double KritikStokSeviyesi { get; set; }
    public DateTime KayitTarihi { get; set; } = DateTime.Now;
}