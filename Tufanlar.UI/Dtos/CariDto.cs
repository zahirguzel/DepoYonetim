namespace Tufanlar.UI.Dtos
{
    public class CariDto
    {
        public int Id { get; set; }
        public string Unvan { get; set; } = string.Empty; // Firma Adı
        public string Telefon { get; set; } = string.Empty;
        public string VergiNo { get; set; } = string.Empty;

        // Bakiye Durumu (Borç/Alacak)
        public decimal Bakiye { get; set; }

        // "ALICI", "SATICI" veya "HEPSI"
        public string CariTipi { get; set; } = "ALICI";
    }
}