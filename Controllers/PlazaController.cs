using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using newNet.Data;
using newNet.Data.Repository;
using newNet.Models;

namespace newNet.Controllers 
{
    [Authorize]
    [Route ("api/[controller]")]
    public class PlazaController : Controller 
    {
        private readonly ITollCollectionRepository _repo;
        public PlazaController(ITollCollectionRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> getPlaza() {
            return Ok(await _repo.getPlazas());
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> addPlaza([FromBody] Plaza plaza) {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            _repo.Add<Plaza>(plaza);
            if(await _repo.SaveAll()) return Ok();
            return BadRequest("Failed to Add plaza");
        }
        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> updatePlaza([FromBody] Plaza plaza) {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            _repo.Update<Plaza>(plaza);
            if(await _repo.SaveAll()) return Ok();
            return BadRequest("Failed to Add plaza");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getPlaza(int id){
            var plazaRepo = await _repo.GetPlaza(id);
            return Ok(plazaRepo);
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task deletePlaza(int id){
            var plazaRepo = await _repo.GetPlaza(id);
            _repo.Delete<Plaza>(plazaRepo);
            await _repo.SaveAll() ;
        }

    }
}