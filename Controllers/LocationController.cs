using System;
using System.Collections.Generic;
using System.Linq;
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
    public class LocationController : Controller
    {
        private readonly DataContext _context;
        public LocationController(DataContext context)
        {
            _context = context;

        }
        public async Task<IActionResult> GetAllLocations()
        {
            List<Location> locations = await _context.Locations.Include(x => x.Photo).ToListAsync();
            // List<LocationForDisplayDto> locationsForDisplayDto = new List<LocationForDisplayDto>();
            // for (var i = 0; i < locations.Count; i++)
            // {
            //     Location location = locations[i];
            //     LocationForDisplayDto locationForDisplayDto = location.Adapt<LocationForDisplayDto>();
            //     locationsForDisplayDto.Add(locationForDisplayDto);
            // }

            var locationsForDisplayDto = TypeAdapter.Adapt<List<Location>, List<LocationForDisplayDto>>(locations); 

            return Ok(locationsForDisplayDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocation(int id)
        {            
            Location location = await _context.Locations.Include(x => x.Photo).FirstOrDefaultAsync(x => x.Id == id);

            if (location == null)
                return BadRequest();

            var locationForDisplayDto = location.Adapt<LocationForDisplayDto>();            

            return Ok(locationForDisplayDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddLocation([FromBody] LocationForCreationDto locationForCreationDto)
        {
            // note: use regex for street address abbreviations
            var locationExists = await _context.Locations
            .FirstOrDefaultAsync(x => x.Name.ToLower().Replace(" ", String.Empty) == locationForCreationDto.Name.ToLower().Replace(" ", String.Empty) &&
            x.Address.ToLower().Replace(" ", String.Empty) == locationForCreationDto.Address.ToLower().Replace(" ", String.Empty) &&
            x.City.ToLower().Replace(" ", String.Empty) == locationForCreationDto.City.ToLower().Replace(" ", String.Empty) &&
            x.State.ToLower().Replace(" ", String.Empty) == locationForCreationDto.State.ToLower().Replace(" ", String.Empty)
            );

            if (locationExists == null) {
                var location = locationForCreationDto.Adapt<Location>();
                
                location.AdminApproved = false;

                _context.Locations.Add(location);
                
                if (await _context.SaveChangesAsync() > 0)
                    return Ok(location);
            
                return BadRequest("something went wrong");
            }
            else {
                return BadRequest("Location already exists");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("approve/{locationId}")]
        public async Task<IActionResult> AdminApproveLocation(int locationId)
        {
            Location location = await _context.Locations.FirstOrDefaultAsync(x => x.Id == locationId);

            location.AdminApproved = true;

            if (await _context.SaveChangesAsync() > 0)
                return Ok(location);
            
            return BadRequest("something went wrong");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("edit/{locationId}")]
        public async Task<IActionResult> EditLocation([FromBody]LocationForCreationDto locationForCreationDto, int locationId)
        {
            Location location = await _context.Locations.Include(x => x.Photo).FirstOrDefaultAsync(x => x.Id == locationForCreationDto.Id);

            // Location location = locationForCreationDto.Adapt<Location>();

            location.Address = locationForCreationDto.Address;
            location.Name = locationForCreationDto.Name;
            location.City = locationForCreationDto.City;
            location.State = locationForCreationDto.State;
            location.Zip = locationForCreationDto.Zip;
            location.Country = locationForCreationDto.Country;
            location.Photo.Url = locationForCreationDto.PhotoUrl;
            location.Market = (Market)Enum.Parse(typeof(Market), locationForCreationDto.MarketType);

            if (await _context.SaveChangesAsync() > 0)
                return Ok(location);
            
            return BadRequest("something went wrong");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{locationId}")]
        public async Task<IActionResult> DeleteLocation(int locationId)
        {
            var location = await _context.Locations.FirstOrDefaultAsync(x => x.Id == locationId);

            _context.Remove(location);

            var locationProducts = await _context.LocationProducts.Where(x => x.LocationId == locationId).ToListAsync();

            _context.RemoveRange(locationProducts);

            if (await _context.SaveChangesAsync() > 0)
                return Ok(location);
            
            return BadRequest("something went wrong");
        }
    }
}