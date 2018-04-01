namespace Stazo.API.Dtos
{
    public class ProductForCreationDto
    {
        public int CategoryId { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public string Market { get; set; }
    }
}