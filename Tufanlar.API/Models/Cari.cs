namespace Tufanlar.API.Models
{
    public class Cari
    {
        public int Id { get; set; }

        // Bu alan kim olduğunu belirleyecek:
        // "ALICI" -> Sadece satış yaptığımız müşteriler
        // "SATICI" -> Mal aldığımız tedarikçiler
        // "HEPSI" -> Hem alıp hem sattığımız firmalar
        public string CariTipi { get; set; } = "ALICI";

        public string Unvan { get; set; } = string.Empty;
        public string VergiNo { get; set; } = string.Empty;
        public string Telefon { get; set; } = string.Empty;
        public string Adres { get; set; } = string.Empty;

        // Bakiye Tek Olacak:
        // (+) Bakiye: Biz alacaklıyız (Müşteri borçlu)
        // (-) Bakiye: Biz borçluyuz (Tedarikçiye ödeme yapmalıyız)
        public decimal Bakiye { get; set; } = 0;
    }
}