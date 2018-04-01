using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stazo.API.Data;
using Stazo.API.Models;

namespace Stazo.API.Controllers
{
    // Only Admin can add or remove categories.
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly DataContext _context;
        public CategoryController(DataContext context)
        {
            _context = context;

        }

        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();

            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody]Category category)
        {
            var categoryExists = await _context.Categories.FirstOrDefaultAsync(x => x.Name == category.Name);

            if (categoryExists != null)
            {
                return BadRequest("Category already exists");
            }

            _context.Categories.Add(category);
            _context.SaveChanges();

            return Ok(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> ChangeCategory([FromBody]Category category)
        {
            var categoryExists = await _context.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);

            if (categoryExists == null)
                return BadRequest("Category does not exist");

            categoryExists.Name = category.Name;

            if (await _context.SaveChangesAsync() > 0)
                return Ok(categoryExists);
            
            return BadRequest("something went wrong");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory([FromBody]Category category)
        {
            var categoryExists = await _context.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);

            if (categoryExists == null)
                return BadRequest("Category does not exist");

            _context.Categories.Remove(categoryExists);
            
            if (await _context.SaveChangesAsync() > 0)
                return Ok(categoryExists);
            
            return BadRequest("something went wrong");
        }
    }
}