using System;
using System.Collections.Generic;

namespace Stazo.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public int UserRole { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string SocialProvider { get; set; }
        public string AvatarUrl { get; set; }
        public string BackgroundUrl { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public int ReviewCount { get; set; }
        public ICollection<Product> ProductsToTry { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime LastActive { get; set; }
    }
}