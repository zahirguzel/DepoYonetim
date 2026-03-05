using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tufanlar.API.Data;
using Tufanlar.API.Dtos;

namespace Tufanlar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("giris")]
        public async Task<IActionResult> GirisYap([FromBody] LoginDto dto)
        {
            // Veritabanında bu kullanıcı var mı?
            var kullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(u => u.KullaniciAdi == dto.KullaniciAdi && u.Sifre == dto.Sifre);

            if (kullanici == null)
            {
                return Unauthorized(new { Mesaj = "Kullanıcı adı veya şifre hatalı!" });
            }

            // Giriş Başarılı! Kullanıcı bilgilerini (şifre hariç) döndür.
            return Ok(new
            {
                Id = kullanici.Id,
                AdSoyad = kullanici.AdSoyad,
                Rol = kullanici.Rol,
                Mesaj = "Giriş başarılı"
            });
        }
    }
}