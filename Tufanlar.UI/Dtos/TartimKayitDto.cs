namespace Tufanlar.UI.Dtos
{
    public class TartimKayitDto
    {
        public int Id { get; set; }
        public string Plaka { get; set; } = string.Empty;
        public string MusteriAdi { get; set; } = string.Empty;
        public string UrunAdi { get; set; } = string.Empty;

        // --- STOK YÖNETİMİ İÇİN KRİTİK ALANLAR ---
        public int UrunId { get; set; }
        public string IslemTuru { get; set; } = "ALIS"; // ALIS veya SATIS

        public double IlkTartim { get; set; }
        public double IkinciTartim { get; set; }
        public double NetAgirlik { get; set; }
    }
}