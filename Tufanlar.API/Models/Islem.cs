using System.ComponentModel.DataAnnotations.Schema;

namespace Tufanlar.API.Models
{
    public class Islem
    {
        public int Id { get; set; }
        public string Plaka { get; set; } = string.Empty;

        // --- KANTAR VERİLERİ ---
        public double Dara { get; set; }  // Boş ağırlık
        public double? Brut { get; set; } // Dolu ağırlık

        // Net ağırlığı otomatik hesapla (Veritabanında tutulmaz, anlık hesaplanır)
        [NotMapped]
        public double Net => Brut.HasValue ? (Brut.Value - Dara) : 0;

        public DateTime GirisTarihi { get; set; } = DateTime.Now;
        public DateTime? CikisTarihi { get; set; }

        // DURUM: 0=Kantarda, 1=Depo Bekliyor, 2=Tamamlandi
        public int Durum { get; set; }


        // --- İLİŞKİLER (Foreign Keys) ---

        // 1. CARİ / MÜŞTERİ (Zorunlu)
        // İşlemin kime yapıldığı belli olmalı
        public int MusteriId { get; set; }
        [ForeignKey("MusteriId")]
        public Musteri? Musteri { get; set; }

        // 2. DEPO (Opsiyonel)
        // Hangi depoya girecek/çıkacak?
        public int? DepoId { get; set; }
        [ForeignKey("DepoId")]
        public Depo? Depo { get; set; }

        // --- STOK YÖNETİMİ İÇİN KRİTİK EKLEMELER ---

        // 3. ÜRÜN (Hangi mal tartılıyor?)
        public int? UrunId { get; set; }
        [ForeignKey("UrunId")]
        public Urun? Urun { get; set; }

        // 4. İŞLEM TÜRÜ
        // "ALIS" (Stok Artar) veya "SATIS" (Stok Azalır)
        public string IslemTuru { get; set; } = "SATIS";
    }
}