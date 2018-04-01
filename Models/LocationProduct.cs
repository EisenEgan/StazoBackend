namespace Stazo.API.Models
{
    public class LocationProduct
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}