using System;
using System.Collections.Generic;

namespace Stazo.API.Models
{
    public enum MarketType : byte
    {
        Grocery, Farmers, Both
    };
    

    public class Product
    {
        public Product()
        {
            Created = DateTime.Now;
        }
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public Brand Brand { get; set; }
        
        public string Name { get; set; }
        public IEnumerable<LocationProduct> LocationProducts { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public int ReviewCount { get; set; }
        public decimal StarRating { get; set; }
        public Photo Photo { get; set; }
        public MarketType MarketType { get; set; }
        public bool AdminApproved { get; set; }
        public DateTime Created { get; set; }
    }
}