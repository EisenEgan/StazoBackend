namespace Stazo.API.Dtos
{
    public class LocationForCreationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zip { get; set; }
        public string Country { get; set; }
        public string PhotoUrl { get; set; }
        public string MarketType { get; set; }
    }
}