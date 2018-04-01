namespace Stazo.API.Dtos
{
    public class SimpleProductForDisplayDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public string Name { get; set; }
        public decimal StarRating { get; set; }
        public string PhotoUrl { get; set; }
        public string Market { get; set; }
        public bool AdminApproved { get; set; }
    }
}