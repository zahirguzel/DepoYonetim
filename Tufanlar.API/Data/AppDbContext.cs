using Microsoft.EntityFrameworkCore;
using Tufanlar.API.Models;

namespace Tufanlar.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Musteri> Musteriler { get; set; }
        public DbSet<Depo> Depolar { get; set; }
        public DbSet<Islem> Islemler { get; set; }
        public DbSet<Urun> Urunler { get; set; }
        public DbSet<Cari> Cariler { get; set; }
        public DbSet<FinansHareket> FinansHareket { get; set; }
        public DbSet<Kategori> Kategoriler { get; set; } // Bu satırı DbContext içine ekle

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- 1. DECIMAL HASSASIYET AYARLARI ---
            modelBuilder.Entity<Musteri>()
                .Property(m => m.GuncelBakiye)
                .HasColumnType("decimal(18,2)");

            // Ürün fiyatları için hassasiyet ayarı (Paraşüt tarzı netlik için)
            modelBuilder.Entity<Urun>()
                .Property(u => u.AlisFiyati)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Urun>()
                .Property(u => u.SatisFiyati)
                .HasColumnType("decimal(18,2)");

            // --- 2. İLİŞKİ AYARLARI ---

            // İşlem - Müşteri İlişkisi
            modelBuilder.Entity<Islem>()
                .HasOne(i => i.Musteri)
                .WithMany()
                .HasForeignKey(i => i.MusteriId)
                .OnDelete(DeleteBehavior.Restrict);

            // İşlem - Ürün İlişkisi (Tanımsız ID hatasını çözen bağ)
            modelBuilder.Entity<Islem>()
                .HasOne(i => i.Urun)
                .WithMany()
                .HasForeignKey(i => i.UrunId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- 3. BAŞLANGIÇ VERİSİ (SEED DATA) ---
            modelBuilder.Entity<Kullanici>().HasData(
                new Kullanici { Id = 1, KullaniciAdi = "admin", Sifre = "123", Rol = "Yonetici", AdSoyad = "Sistem Yöneticisi" },
                new Kullanici { Id = 2, KullaniciAdi = "kantar", Sifre = "123", Rol = "Kantar", AdSoyad = "Kantar Personeli" },
                new Kullanici { Id = 3, KullaniciAdi = "depo", Sifre = "123", Rol = "Depo", AdSoyad = "Depo Sorumlusu" }
            );
        }
    }
}