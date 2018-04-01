namespace Stazo.API.Dtos
{
    public class UserForLoginDto
    {
        public string EmailOrUsername { get; set; }
        
        public string Password { get; set; }
        public string SocialProvider { get; set; }
    }
}