using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tufanlar.API.Data;
using Tufanlar.API.Models;

namespace Tufanlar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrunController(AppDbContext context) : ControllerBase
    {
        // 1. TÜM ÜRÜNLERİ LİSTELE
        // 1. TÜM ÜRÜNLERİ LİSTELE
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Urun>>> GetUrunler()
        {
            // Include(u => u.Kategori): SQL'deki JOIN işlemini yapar.
            // Böylece ön yüze sadece ID değil, kategori ismi de gidebilir.
            return await context.Urunler
                                .Include(u => u.Kategori)
                                .ToListAsync();
        }

        // 2. YENİ ÜRÜN EKLE (POST)
        [HttpPost]
        public async Task<ActionResult<Urun>> PostUrun(Urun urun)
        {
            // GÜVENLİK 1: Aynı barkoddan sistemde var mı?
            if (await context.Urunler.AnyAsync(x => x.Barkod == urun.Barkod))
            {
                return BadRequest("Bu barkod zaten sistemde kayıtlı! Lütfen başka barkod kullanın.");
            }

            // ID'yi sıfırlıyoruz ki veritabanı otomatik artan sayı versin
            urun.Id = 0;

            context.Urunler.Add(urun);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetUrunler", new { id = urun.Id }, urun);
        }

        // 3. ÜRÜNÜ GÜNCELLE (PUT) - KRİTİK DÜZELTME
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUrun(int id, Urun urun)
        {
            // URL'deki ID ile Paket'teki ID uyuşuyor mu?
            if (id != urun.Id) return BadRequest("Ürün ID uyuşmazlığı!");

            // GÜVENLİK 2: Güncellenen barkod BAŞKA bir üründe var mı?
            // (Kendi ID'si hariç diğerlerinde arıyoruz)
            bool barkodBaskasindaVarMi = await context.Urunler
                .AnyAsync(x => x.Barkod == urun.Barkod && x.Id != id);

            if (barkodBaskasindaVarMi)
            {
                return BadRequest("Bu barkod başka bir üründe kullanılıyor. Çakışma engellendi.");
            }

            // Veritabanına "Bu ürün değişti" bilgisini veriyoruz
            context.Entry(urun).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
                // Başarılı olursa 204 No Content veya 200 OK dönebiliriz.
                // Log görmek için OK dönüyoruz.
                return Ok($"Ürün (ID: {id}) başarıyla güncellendi.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UrunExists(id)) return NotFound("Ürün bulunamadı.");
                else throw;
            }
            catch (Exception ex)
            {
                // Hatanın detayını konsola yazıyoruz (Debug için)
                Console.WriteLine($"GÜNCELLEME HATASI: {ex.InnerException?.Message ?? ex.Message}");
                return StatusCode(500, "Veritabanı hatası oluştu.");
            }
        }

        // YARDIMCI METOT: Ürün var mı yok mu kontrolü
        private bool UrunExists(int id)
        {
            return context.Urunler.Any(e => e.Id == id);
        }
    }
}