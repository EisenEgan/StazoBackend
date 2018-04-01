using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stazo.API.Data;
using System.Linq;
using Stazo.API.Dtos;
using Microsoft.EntityFrameworkCore;
using Mapster;
using Stazo.API.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Stazo.API.Controllers
{
    [Route("api/[controller]")]
    public class ReviewController : Controller
    {
        private readonly DataContext _context;
        public ReviewController(DataContext context )
        {
            _context = context;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetReviews(int productId)
        {
            var reviews = await _context.Reviews
            .Where(x => x.ProductId == productId)
            // Use DTOs for both of these
            .Include(x => x.User)
            .Include(x => x.Location)
            .ThenInclude(x => x.Photo)
            .Include(x => x.Photos)            
            .ToListAsync();
            
            if (reviews == null)
                return Ok("No reviews");

            List<ReviewForDisplayDto> reviewsForDisplayDto = TypeAdapter.Adapt<List<Review>, List<ReviewForDisplayDto>>(reviews);

            return Ok(reviewsForDisplayDto);
        }

        [HttpGet("detail/{reviewId}")]
        public async Task<IActionResult> GetReview(int reviewId)
        {
            var review = await _context.Reviews
            .Include(x => x.User)
            .Include(x => x.Location)
            .ThenInclude(x => x.Photo)
            .Include(x => x.Product)
            .ThenInclude(x => x.Photo)
            .Include(x => x.Product.Category)
            .Include(x => x.Product.Brand)
            .Include(x => x.Photos)
            .FirstOrDefaultAsync(x => x.Id == reviewId);
            
            if (review == null)
                return Ok("Review does not exist");

            ReviewForDisplayDto reviewsForDisplayDto = review.Adapt<ReviewForDisplayDto>();

            return Ok(reviewsForDisplayDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody]ReviewForCreationDto reviewForCreationDto)
        {
            // try with photo
            // consider deleting reviews and adding reviewCount to user
            var reviewed = await _context.Reviews
            .FirstOrDefaultAsync(x => x.UserId == reviewForCreationDto.UserId &&
            x.ProductId == reviewForCreationDto.ProductId);

            if (reviewed != null)
                return BadRequest("User has already reviewed this product");

            var review = reviewForCreationDto.Adapt<Review>();

            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == reviewForCreationDto.ProductId);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == reviewForCreationDto.UserId);

            product.StarRating = ((product.StarRating * 2) * product.ReviewCount + reviewForCreationDto.Star) / (2 * (product.ReviewCount + 1));

            product.ReviewCount++;
            user.ReviewCount++;

            _context.Add(review);
            _context.SaveChanges();

            return Ok(review);
        }

        [Authorize(Roles = "User")]
        [HttpPut("edit/{reviewId}")]
        public async Task<IActionResult> EditReview([FromBody]ReviewForCreationDto reviewForCreationDto, int reviewId)
        {
            if (reviewForCreationDto.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            // make sure logged in user is the creator of the review
            Review review = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == reviewId);

            var previousScore = review.StarScore;

            review.LocationId = reviewForCreationDto.LocationId;
            review.Text = reviewForCreationDto.Text;
            review.StarScore = reviewForCreationDto.Star;

            Product product = await _context.Products.FirstOrDefaultAsync(x => x.Id == reviewForCreationDto.ProductId);
            product.StarRating = ((product.StarRating * 2) * (product.ReviewCount - 1) + (product.StarRating * 2 + (review.StarScore - previousScore))) / (2 * product.ReviewCount);
            // product.StarRating = ((product.StarRating * 2) * (product.ReviewCount - 1) + (review.StarScore)) / (2 * product.ReviewCount);

            if (await _context.SaveChangesAsync() > 0)
                return Ok(review);
            
            return BadRequest("something went wrong");
        }

        [Authorize(Roles = "User, Admin")]
        [HttpDelete("delete/{reviewId}")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            Review review = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == reviewId);

            // if (!User.IsInRole("Admin"))
            if (!User.IsInRole("Admin"))
            {
                if (review.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                    return Unauthorized();
            }
            // if (!User.IsInRole("Admin")  review.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            //     return Unauthorized();

            Product product = await _context.Products.FirstOrDefaultAsync(x => x.Id == review.ProductId);
            product.StarRating = ((product.StarRating * 2) * (product.ReviewCount - 1) + (product.StarRating * 2 - review.StarScore)) / (2 * (product.ReviewCount - 1));
            product.ReviewCount--;

            _context.Reviews.Remove(review);

            if (await _context.SaveChangesAsync() > 0)
                return Ok(review);
            
            return BadRequest("something went wrong");
        }
    }
}