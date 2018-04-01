using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Stazo.API.Data;
using Stazo.API.Dtos;
using Stazo.API.Models;

namespace Stazo.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Email = userForRegisterDto.Email.ToLower();

            // Check whether socialauth user is registered and if so proceed to socialauth login

            if (await _repo.UserExists(userForRegisterDto.Username, userForRegisterDto.Email))
                ModelState.AddModelError("Username", "Username already exists");

            // validate request
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userToCreate = new User
            {
                Email = userForRegisterDto.Email,
                SocialProvider = userForRegisterDto.SocialProvider,
                UserRole = 1,
                LastActive = DateTime.Now
            };

            var createUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _repo.Login(userForLoginDto.EmailOrUsername, userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            // generate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value);
            
            var role = userFromRepo.UserRole == 0 ? "Admin" : "User";
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                    new Claim(ClaimTypes.Name, userFromRepo.Username),
                    new Claim(ClaimTypes.Role, role)
                    // new Claim("Role", userFromRepo.UserRole)
                    
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { tokenString });
        }

        [HttpGet("validate-username/{username}")]
        public async Task<IActionResult> ValidateUsername(string username)
        {
            if (await _repo.UsernameExists(username))
                return Ok(new { username_taken = true });
            else
                return Ok(new { username_taken = false });
        }

        public class EmailDto
        {
            public string Email { get; set; }
        }

        [HttpPost("validate-email")]
        public async Task<IActionResult> ValidateEmail([FromBody]EmailDto emailToTest)
        {
            if (await _repo.EmailExists(emailToTest.Email))
                return Ok(new { email_taken = true });
            else
                return Ok(new { email_taken = false });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("changerole/{id}")]
        public async Task<IActionResult> ChangeUserRole(int id)
        {
            var user = await _repo.ChangeUserPrivileges(id);

            return Ok(user);
        }
    }
}