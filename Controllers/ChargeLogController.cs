using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using newNet.Data.Repository;

namespace newNet.Controllers 
{
    [Authorize]
    [Route ("api/[controller]")]
    public class ChargeLogController : Controller 
    {
        private readonly ITollCollectionRepository _repo;
        public ChargeLogController (ITollCollectionRepository repo) 
        {
            _repo = repo;
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> getLogs(){
            return Ok(await _repo.getLogs());
        }
        [Authorize(Roles = "admin")]
        [HttpGet("plaza/{id}")]
        public async Task<IActionResult> getPlazaLogs(int id){
            return Ok(await _repo.getPlazaLogs(id));
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> getUserLogs(int id){
            return Ok(await _repo.getUserLogs(id));
        }

    }
}