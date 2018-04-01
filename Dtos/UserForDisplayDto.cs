using System;
using System.Collections.Generic;

namespace Stazo.API.Dtos
{
    public class UserForDisplayDto
    {
        public int UserRole { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }

        public IEnumerable<ReviewForDisplayDto> Reviews { get; set; }        
        public string SocialProvider { get; set; }
        public string AvatarUrl { get; set; }
        public string BackgroundUrl { get; set; }
        public int ReviewCount { get; set; }
        public DateTime LastActive { get; set; }
    }
}