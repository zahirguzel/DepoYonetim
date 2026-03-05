namespace Tufanlar.API.Dtos // Projenizdeki gerçek namespace neyse onu kullanın
{
    public class TartimKayitDto
    {
        public int Id { get; set; }
        public string Plaka { get; set; } = string.Empty;
        public string MusteriAdi { get; set; } = string.Empty;
        public string UrunAdi { get; set; } = string.Empty;

        // --- HATALARI ÇÖZEN YENİ ALANLAR ---
        public int UrunId { get; set; } // Hangi ürün olduğunu anlamak için şart
        public string IslemTuru { get; set; } = "ALIS"; // ALIS veya SATIS

        public double IlkTartim { get; set; }
        public double IkinciTartim { get; set; }
        public double NetAgirlik { get; set; }
    }
}