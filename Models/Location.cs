using System.Collections.Generic;

namespace Stazo.API.Models
{
    public enum Market : byte
    {
        Grocery, Farmers
    };
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zip { get; set; }
        public string Country { get; set; }
        public ICollection<LocationProduct> LocationProducts { get; set; }
        public Photo Photo { get; set; }
        public Market Market { get; set; }
        public bool AdminApproved { get; set; }
    }
}