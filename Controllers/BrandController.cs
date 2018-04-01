using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stazo.API.Data;

namespace Stazo.API.Controllers
{
    [Route("api/[controller]")]
    public class BrandController : Controller
    {
        private readonly DataContext _context;
        public BrandController(DataContext context)
        {
            _context = context;

        }

        public async Task<IActionResult> GetBrands()
        {
            var brands = await _context.Brands.ToListAsync();

            return Ok(brands);
        }
    }
}