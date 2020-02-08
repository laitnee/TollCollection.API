using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using newNet.Data.Repository;
using newNet.Models;

namespace newNet.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class VehicleController : Controller
    {
        private readonly ITollCollectionRepository _repo;
        public VehicleController(ITollCollectionRepository repo)
        {
            _repo = repo;
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> addVehicle([FromBody] Vehicle vehicle){
            if(!ModelState.IsValid) return BadRequest(ModelState);
            _repo.Add<Vehicle>(vehicle);
            return Ok(await _repo.SaveAll());
        }
        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> updateVehicle([FromBody] Vehicle vehicle){
            if(!ModelState.IsValid) return BadRequest(ModelState);
            _repo.Update<Vehicle>(vehicle);
            return Ok(await _repo.SaveAll());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> getVehicle(int id){
            return Ok(await _repo.getUserVehicles(id));
        }
        [HttpGet]
        public async Task<IActionResult> getVehicles(){
             return Ok(await _repo.getVehicles());
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> deleteVehicle(int id)
        {
            var vehicleRepo = await _repo.getVehicleWithId(id);            
            _repo.Delete<Vehicle>(vehicleRepo);
            return Ok(await _repo.SaveAll());
        }
    }
}