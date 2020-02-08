using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using newNet.Data.Repository;
using newNet.Models;
using newNet.Models.DTOs;

namespace newNet.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthController: Controller
    {
        private readonly IAuthenticationRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public AuthController(IAuthenticationRepository repo, IConfiguration config, IMapper mapper)
        {
            _mapper = mapper;
            _config = config;
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserRegisterDTO userRegisterDto)
        {
            if (!string.IsNullOrEmpty(userRegisterDto.Username))
                userRegisterDto.Username = userRegisterDto.Username.ToLower();

            if (await _repo.UserExists(userRegisterDto.Username))
                ModelState.AddModelError("Username", "Username already exists");

            // validate request
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userToCreate = _mapper.Map<User>(userRegisterDto);

            var createdUser = await _repo.Register(userToCreate, userRegisterDto.Password);

            var userToReturn = _mapper.Map<UserDetailsDTO>(createdUser);

            return CreatedAtRoute("GetUser", new {controller = "User", userId = createdUser.Id}, userToReturn);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginDTO userLoginDto)
        {
            Console.WriteLine(userLoginDto);
            var userFromRepo = await _repo.Login(userLoginDto.Username.ToLower(), userLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            // generate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                    new Claim(ClaimTypes.Name, userFromRepo.Username),
                    new Claim(ClaimTypes.Role, userFromRepo.Role)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var user = _mapper.Map<UserDetailsDTO>(userFromRepo);

            return Ok(new { tokenString, user });

        }
    }
}