using System.Collections.Generic;

namespace Stazo.API.Dtos
{
    public class ReviewForCreationDto
    {
        public int UserId { get; set; }
        public int LocationId { get; set; }
        public int ProductId { get; set; }
        public string Text { get; set; }
        public int Star { get; set; }
        public ICollection<string> PhotoUrls { get; set; }
    }
}