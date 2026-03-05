using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tufanlar.API.Data;
using Tufanlar.API.Dtos;
using Tufanlar.API.Models;

namespace Tufanlar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KantarController : ControllerBase
    {
        private readonly AppDbContext _context;

        public KantarController(AppDbContext context)
        {
            _context = context;
        }

        // 1. TARTIM KAYDET (Async Yapı ve UrunId Desteği)
        [HttpPost("kaydet")]
        public async Task<IActionResult> TartimKaydet([FromBody] TartimKayitDto dto)
        {
            // Veritabanında ürün var mı kontrol et
            var urun = await _context.Urunler.FindAsync(dto.UrunId);
            if (urun == null) return BadRequest("Seçilen ürün geçerli değil.");

            var yeniIslem = new Islem
            {
                Plaka = dto.Plaka,
                Dara = dto.IlkTartim,
                Brut = dto.IkinciTartim,
                GirisTarihi = DateTime.Now,
                Durum = 1, // 1: Depo Onayı Bekliyor
                UrunId = dto.UrunId, // Paraşüt mantığı: Her işlem bir ürüne bağlıdır
                IslemTuru = dto.IslemTuru ?? "ALIS", // Varsayılan Alış

                // Müşteri ataması (Basitleştirilmiş)
                Musteri = new Musteri { FirmaAdi = dto.MusteriAdi, GuncelBakiye = 0 }
            };

            _context.Islemler.Add(yeniIslem);
            await _context.SaveChangesAsync();

            return Ok(new { Mesaj = "Tartım başarıyla kaydedildi!", IslemId = yeniIslem.Id });
        }

        // 2. BEKLEYEN TARTIMLAR (İsim Karışıklığı Çözüldü)
        [HttpGet("bekleyenler")]
        public async Task<IActionResult> BekleyenTartimlariGetir()
        {
            var liste = await _context.Islemler
                .Include(x => x.Urun) // Ürün bilgilerini (Ad, Birim vb.) çekiyoruz
                .Include(x => x.Musteri)
                .Where(x => x.Durum == 1)
                .Select(x => new TartimKayitDto
                {
                    Id = x.Id,
                    Plaka = x.Plaka,
                    MusteriAdi = x.Musteri != null ? x.Musteri.FirmaAdi : "Genel",

                    // Ürün adı ve İşlem türünü birleştirerek depocuya net bilgi veriyoruz
                    UrunAdi = $"{(x.Urun != null ? x.Urun.Ad : "Tanımsız")} ({(x.IslemTuru == "SATIS" ? "ÇIKIŞ" : "GİRİŞ")})",

                    IlkTartim = (double)x.Dara,
                    IkinciTartim = x.Brut.HasValue ? (double)x.Brut.Value : 0,
                    NetAgirlik = x.Brut.HasValue ? (double)(x.Brut.Value - x.Dara) : 0,
                    IslemTuru = x.IslemTuru
                })
                .ToListAsync();

            return Ok(liste);
        }

        // 3. DEPO ONAYI VE STOK GÜNCELLEME
        [HttpPost("onayla/{id}")]
        public async Task<IActionResult> TartimOnayla(int id)
        {
            var islem = await _context.Islemler
                .Include(x => x.Urun)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (islem == null)
                return NotFound("İşlem bulunamadı.");

            if (islem.Durum == 2)
                return BadRequest("Bu işlem zaten onaylanmış.");

            if (islem.Urun == null)
                return BadRequest("İşleme ait ürün bulunamadı, stok güncellenemez.");

            // Net ağırlık hesabı
            double netMiktar = (double)((islem.Brut ?? 0) - islem.Dara);

            // STOK YÖNETİMİ (Paraşüt Mantığı)
            if (islem.IslemTuru == "ALIS")
            {
                islem.Urun.StokMiktari += netMiktar;
            }
            else if (islem.IslemTuru == "SATIS")
            {
                // Kritik Stok Kontrolü: Eksiye düşmeyi engelle
                if (islem.Urun.StokMiktari < netMiktar)
                {
                    return BadRequest($"Yetersiz stok! Mevcut: {islem.Urun.StokMiktari} {islem.Urun.Birim}.");
                }
                islem.Urun.StokMiktari -= netMiktar;
            }

            islem.Durum = 2; // İşlem Tamamlandı
            islem.CikisTarihi = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Mesaj = "Depo onayı verildi, stok güncellendi.",
                Urun = islem.Urun.Ad,
                YeniStok = $"{islem.Urun.StokMiktari} {islem.Urun.Birim}"
            });
        }
    }
}