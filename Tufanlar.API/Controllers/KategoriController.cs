using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tufanlar.API.Data;
using Tufanlar.API.Models;

namespace Tufanlar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KategoriController(AppDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kategori>>> Get()
        {
            return await context.Kategoriler.OrderBy(k => k.Ad).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Kategori>> Post(Kategori kategori)
        {
            if (await context.Kategoriler.AnyAsync(k => k.Ad == kategori.Ad))
                return BadRequest("Bu kategori zaten var.");

            context.Kategoriler.Add(kategori);
            await context.SaveChangesAsync();
            return Ok(kategori);
        }
    }
}