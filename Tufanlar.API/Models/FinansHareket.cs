using System.ComponentModel.DataAnnotations.Schema;

namespace Tufanlar.API.Models
{
    public class FinansHareket
    {
        public int Id { get; set; }
        public DateTime Tarih { get; set; } = DateTime.Now;

        // İşlem Tipi: "TAHSILAT" (Para Girişi) veya "ODEME" (Para Çıkışı)
        public string IslemTuru { get; set; } = "TAHSILAT";

        // Ödeme Yöntemi: "NAKIT", "HAVALE", "KREDIKARTI"
        public string OdemeSekli { get; set; } = "NAKIT";

        public decimal Tutar { get; set; }
        public string Aciklama { get; set; } = string.Empty; // Örn: Fatura No 123 ödemesi

        // İlişki: Hangi Cari?
        public int CariId { get; set; }
        [ForeignKey("CariId")]
        public Cari? Cari { get; set; }
    }
}