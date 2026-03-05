namespace Tufanlar.API.Models
{
    public class Depo
    {
        public int Id { get; set; }
        public string DepoAdi { get; set; } = string.Empty; // "Antrepo 1"
        public string? Konum { get; set; }
    }
}