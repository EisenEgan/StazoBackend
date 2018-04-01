using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stazo.API.Data;
using Stazo.API.Dtos;
using Stazo.API.Models;

namespace Stazo.API.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly DataContext _context;
        public ProductController(DataContext context )
        {
            _context = context;
        }

        public async Task<IActionResult> GetProducts()
        {
            // Todo: Use Dto
            var products = await _context.Products
            .Include(x => x.Category)
            .Include(x => x.Photo)
            .Include(x => x.Brand)
            .Include(x => x.Reviews)        
            // .Include(x => x.LocationProducts)
            // .ThenInclude(x => x.Location)
            // .ThenInclude(x => x.Photo)
            .OrderByDescending(x => x.Created)
            .ToListAsync();

            // var locationsForDisplayDto = products.Select(x => x.LocationProducts.ToList().Select(y => y.Location));

            List<ProductForDisplayDto> productsForDisplayDto = TypeAdapter.Adapt<List<Product>, List<ProductForDisplayDto>>(products);

            return Ok(productsForDisplayDto);
        }

        // think about adding this to a helper function somehow
        string UppercaseFirstEach(string s)
        {
            char[] a = s.ToLower().ToCharArray();

            for (int i = 0; i < a.Count(); i++ )
            {
                a[i] = i == 0 || a[i-1] == ' ' ? char.ToUpper(a[i]) : a[i];
            }

            return new string(a);
        }
        
        [HttpGet("category/{categoryName}")]
        public async Task<IActionResult> GetProductsByCategory(string categoryName)
        {
            

            var categoryNameWithSpaces = UppercaseFirstEach(categoryName.Replace("_", " "));
            // Todo: Use Dto
            var products = await _context.Products.Where(x => x.Category.Name == categoryNameWithSpaces)
            .Include(x => x.Category)
            .Include(x => x.Photo)
            .Include(x => x.Brand)
            .Include(x => x.Reviews)        
            // .Include(x => x.LocationProducts)
            // .ThenInclude(x => x.Location)
            // .ThenInclude(x => x.Photo)
            .OrderByDescending(x => x.Created)
            .ToListAsync();

            // var locationsForDisplayDto = products.Select(x => x.LocationProducts.ToList().Select(y => y.Location));

            List<ProductForDisplayDto> productsForDisplayDto = TypeAdapter.Adapt<List<Product>, List<ProductForDisplayDto>>(products);

            return Ok(productsForDisplayDto);
        }

        [HttpGet("review/{reviewType}")]
        public async Task<IActionResult> GetProductsByReviews(string reviewType)
        {
            var products = await _context.Products
            .Include(x => x.Category)
            .Include(x => x.Photo)
            .Include(x => x.Brand)
            .Include(x => x.Reviews)
            .ToListAsync();

            if (reviewType == "most")
                products = products.OrderByDescending(x => x.ReviewCount).ToList();
            else if (reviewType == "highest")
                products = products.OrderByDescending(x => x.StarRating).ToList();                

            List<ProductForDisplayDto> productsForDisplayDto = TypeAdapter.Adapt<List<Product>, List<ProductForDisplayDto>>(products);

            return Ok(productsForDisplayDto);
        }

        [HttpGet("find/{brand}/{productName}")]
        public async Task<IActionResult> GetProduct(string brand, string productName)
        {
            var product = await _context.Products
            .Include(x => x.Category)
            .Include(x => x.Photo)
            .Include(x => x.Brand)
            .Include(x => x.Reviews)            
            // .Include(x => x.LocationProducts)
            // .ThenInclude(x => x.Location)
            // .ThenInclude(x => x.Photo)
            .FirstOrDefaultAsync(x => Regex.Replace(x.Brand.Name, @"[\.']", "").Replace(" ", "_").ToLower() == brand &&
            Regex.Replace(x.Name, @"[\.']", "").Replace(" ", "_").ToLower() == productName);

            // List<Location> locations = product.LocationProducts.Select(x => x.Location).ToList();

            // List<LocationForDisplayDto> locationsForDisplayDto = new List<LocationForDisplayDto>();
            // locations.ForEach(x => locationsForDisplayDto.Add(new LocationForDisplayDto() {
            //     Name = x.Name,
            //     Address = x.Address,
            //     City = x.City,
            //     State = x.State,
            //     Zip = x.Zip,
            //     Country = x.Country,
            //     PhotoUrl = x.Photo.Url,
            //     MarketType = Enum.GetName(typeof(Market), x.Market)
            // }));
            // locations.ForEach(x => locationsForDisplayDto.Add(x.Adapt<LocationForDisplayDto>()));

            // List<LocationForDisplayDto> locationsForDisplayDto = locations.Select(x => new LocationForDisplayDto(){
            //     Name = x.Name,
            //     Address = x.Address,
            //     City = x.City,
            //     State = x.State,
            //     Zip = x.Zip,
            //     Country = x.Country,
            //     PhotoUrl = x.Photo.Url,
            //     MarketType = Enum.GetName(typeof(Market), x.Market)
            // }).ToList();
            // var herp = locationsForDisplayDto;
            
            // List<LocationForDisplayDto> locationsForDisplayDto = TypeAdapter.Adapt<List<Location>, List<LocationForDisplayDto>>(locations);            

            // if (product == null)
            //     return BadRequest("Product does not exist");

            ProductForDisplayDto productForDisplayDto = product.Adapt<ProductForDisplayDto>();

            return Ok(productForDisplayDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody]ProductForCreationDto productForCreationDto)
        {
            var productExists = await _context.Products
            .FirstOrDefaultAsync(x => x.Brand.Name.ToLower().Replace(" ", String.Empty) == productForCreationDto.Brand.ToLower().Replace(" ", String.Empty) &&
            x.Name.ToLower().Replace(" ", String.Empty) == productForCreationDto.Name.ToLower().Replace(" ", String.Empty));

            if (productExists == null) {
                Product product = productForCreationDto.Adapt<Product>();
                Brand brandExists = _context.Brands.FirstOrDefault(x => x.Name == productForCreationDto.Brand);
                Brand brand;

                if (brandExists == null)
                    brand = new Brand() { Name = productForCreationDto.Brand };
                else
                    brand = brandExists;
                
                product.Brand = brand;

                product.AdminApproved = false;

                _context.Add(product);
                // May have to save here.
                _context.SaveChanges();

                // var locationProduct = new LocationProduct() {
                //     LocationId = productForCreationDto.LocationId,
                //     ProductId = product.Id
                // };

                // _context.Add(locationProduct);
                // _context.SaveChanges();

                return Ok(product);
            }
            else
            {
                return BadRequest("Product already exists");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("approve/{productId}")]
        public async Task<IActionResult> AdminApproveProduct(int productId)
        {
            Product product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);

            product.AdminApproved = true;

            if (await _context.SaveChangesAsync() > 0)
                return Ok(product);
            
            return BadRequest("something went wrong");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("edit/{productId}")]
        public async Task<IActionResult> EditProduct([FromBody]ProductForCreationDto productForCreationDto, int productId)
        {
            Product product = await _context.Products
                // .Include(x => x.Brand)
                .Include(x => x.Photo)
                .FirstOrDefaultAsync(x => x.Id == productId);

            // could search for brand name or create new brand.
            product.CategoryId = productForCreationDto.CategoryId;
            // product.Brand.Name = productForCreationDto.Brand;
            product.Name = productForCreationDto.Name;
            product.Photo.Url = productForCreationDto.PhotoUrl;
            product.MarketType = (MarketType)Enum.Parse(typeof(MarketType), productForCreationDto.Market);

            if (await _context.SaveChangesAsync() > 0)
                return Ok(product);
            
            return BadRequest("something went wrong");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            Product product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);

            _context.Remove(product);

            var locationProducts = await _context.LocationProducts.Where(x => x.ProductId == productId).ToListAsync();

            _context.RemoveRange(locationProducts);

            if (await _context.SaveChangesAsync() > 0)
                return Ok(product);
            
            return BadRequest("something went wrong");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{productId}/deletefromlocation/{locationId}")]
        public async Task<IActionResult> DeleteProductFromLocation(int productId, int locationId)
        {
            LocationProduct locationProduct = await _context.LocationProducts
            .FirstOrDefaultAsync(x => x.ProductId == productId && x.LocationId == locationId);

            if (locationProduct == null)
                return BadRequest("Product is not currently found at that location.");

            _context.Remove(locationProduct);

            if (await _context.SaveChangesAsync() > 0)
                return Ok(locationProduct);
            
            return BadRequest("something went wrong");
        }
    }
}