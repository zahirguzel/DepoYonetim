namespace Tufanlar.API.Models
{
    public class Musteri
    {
        public int Id { get; set; }
        public string FirmaAdi { get; set; } = string.Empty;
        public string? VergiNo { get; set; }
        public decimal GuncelBakiye { get; set; } // (+) Alacak, (-) Borç
    }
}