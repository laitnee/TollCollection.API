using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using newNet.Data.Repository;
using newNet.Models;
using newNet.Models.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace newNet.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ITollCollectionRepository _repo;
        private IMapper _mapper;
        ILogger<UserController> _logger;
        public UserController(ITollCollectionRepository repo, IMapper  mapper, ILogger<UserController> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }

        [Authorize(Roles = "admin")]
        [HttpPut("makeAdmin/{id}")]
        public async Task<IActionResult> makeAdmin(int id)
        {
            User userRepo = await _repo.getUser(id);
            if(userRepo == null) return BadRequest("User not found");

            userRepo.Role = "admin";
            _repo.Update<User>(userRepo);
            return Ok(await _repo.SaveAll());
        }
        [HttpPut]
        public async Task<IActionResult> updateUser([FromBody] UserUpdateDTO user)
        {   
            if (user.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            User userRepo = await _repo.getUser(user.Id);
            if(userRepo == null) return BadRequest("User not found");
            _mapper.Map(user, userRepo);
            _repo.Update<User>(userRepo);
            return Ok(await _repo.SaveAll());
        }
        [HttpGet("{userId}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int userId)
        {
            User userRepo = await  _repo.getUser(userId);
            if(userRepo == null) return BadRequest("User not found");
            var userToReturn = _mapper.Map<UserDetailsDTO>(userRepo);
            
            return Ok(userToReturn);
        }

        [HttpPost("{userId}/{amount}")]
        public async Task<IActionResult> rechargeAccount(int userId, int amount)
        {
            User userRepo = await  _repo.getUser(userId);
            if(userRepo == null) return BadRequest("User not found");

            userRepo.AccountBalance += amount;
            _repo.Update<User>(userRepo);
            return Ok(await _repo.SaveAll());
        }
        
    }
}