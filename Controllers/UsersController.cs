using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Stazo.API.Data;
using Stazo.API.Dtos;
using Stazo.API.Models;

namespace Stazo.API.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly DataContext _context;
        public UserController(DataContext context)
        {
            _context = context;

        }

        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            List<UserForDisplayDto> usersForDisplayDto = TypeAdapter.Adapt<List<User>, List<UserForDisplayDto>>(users);            

            return Ok(usersForDisplayDto);
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUser(string username)
        {
            var user = await _context.Users
            .Include(x => x.Reviews)
            .ThenInclude(x => x.User)
            .Include(x => x.Reviews)
            .ThenInclude(x => x.Location)
            .ThenInclude(x => x.Photo)
            .Include(x => x.Reviews)
            .ThenInclude(x => x.Product)
            .ThenInclude(x => x.Photo)
            .Include(x => x.Reviews)
            .ThenInclude(x => x.Product)
            .ThenInclude(x => x.Brand)
            .Include(x => x.Reviews)
            .ThenInclude(x => x.Product)
            .ThenInclude(x => x.Category)
            .Include(x => x.Reviews)
            .ThenInclude(x => x.Photos)
            // .Include(x => x.Reviews)
            // .ThenInclude(x => x.Location)
            // .Include(x => x.Reviews)
            // .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Username.ToLower() == username);
            UserForDisplayDto userForDisplayDto = user.Adapt<UserForDisplayDto>();
            
            // IEnumerable<Review> reviews = await _context.Reviews
            // .Include(x => x.User)
            // .Include(x => x.Location)
            // .ThenInclude(x => x.Photo)
            // .Include(x => x.Product)
            // .ThenInclude(x => x.Photo)
            // .Include(x => x.Product.Category)
            // .Include(x => x.Product.Brand)
            // .Include(x => x.Photos)
            // .Where(x => x.UserId == user.Id).ToListAsync();

            // IEnumerable<ReviewForDisplayDto> reviewsForDisplayDto = TypeAdapter.Adapt<IEnumerable<Review>, IEnumerable<ReviewForDisplayDto>>(reviews);

            // userForDisplayDto.Reviews = reviewsForDisplayDto;

            return Ok(userForDisplayDto);
        }
    }
}