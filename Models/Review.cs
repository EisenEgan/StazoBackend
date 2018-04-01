using System;
using System.Collections.Generic;

namespace Stazo.API.Models
{
    public class Review
    {
        public Review()
        {
            CreatedAt = DateTime.Now;
        }
        
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LocationId { get; set; }
        public int ProductId { get; set; }
        public User User { get; set; }
        public Location Location { get; set; }
        public Product Product { get; set; }
        public string Text { get; set; }
        public int StarScore { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
    }
}