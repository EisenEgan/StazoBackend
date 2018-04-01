using System;
using System.Collections.Generic;
using Stazo.API.Models;

namespace Stazo.API.Dtos
{
    public class ProductForDisplayDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public string Name { get; set; }
        // public ICollection<LocationForDisplayDto> Locations { get; set; }
        // change to reviewfordisplaydto
        // public ICollection<Review> Reviews { get; set; }
        public decimal StarRating { get; set; }
        public string PhotoUrl { get; set; }
        public string Market { get; set; }
        public bool AdminApproved { get; set; }
        public DateTime Created { get; set; }
    }
}