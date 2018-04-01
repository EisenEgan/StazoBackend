namespace Stazo.API.Dtos
{
    public class UserForDisplayInReviewDto
    {
        public int UserRole { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string AvatarUrl { get; set; }
        public int ReviewCount { get; set; }
    }
}