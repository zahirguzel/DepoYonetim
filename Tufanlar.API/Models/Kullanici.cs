namespace Tufanlar.API.Models
{
    public class Kullanici
    {
        public int Id { get; set; }
        public string KullaniciAdi { get; set; } = string.Empty;
        public string Sifre { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty; // "Yonetici", "Kantar", "Depo"
        public string? AdSoyad { get; set; }
    }
}