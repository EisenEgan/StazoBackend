using System;
using System.Collections.Generic;
using Stazo.API.Models;

namespace Stazo.API.Dtos
{
    public class ReviewForDisplayDto
    {
        // public int UserId { get; set; }
        // public int LocationId { get; set; }
        // public int ProductId { get; set; }
        public UserForDisplayInReviewDto User { get; set; }
        public LocationForDisplayDto Location { get; set; }
        public SimpleProductForDisplayDto Product { get; set; }
        public string Text { get; set; }
        public int StarScore { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}