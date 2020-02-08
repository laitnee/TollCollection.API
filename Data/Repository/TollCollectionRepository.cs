using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using newNet.Models;

namespace newNet.Data.Repository
{
    public class TollCollectionRepository : ITollCollectionRepository 
    { 
        private DataContext _context;
        public TollCollectionRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public async Task<User> getUser(int userId) 
        {
            return await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
        }
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
        public async Task<IEnumerable<ChargeLog>> getLogs()
        {
            return await _context.ChargeLogs.ToListAsync();
        }

        public async Task<IEnumerable<ChargeLog>> getPlazaLogs(int plazaId)
        {
            return await _context.ChargeLogs.Where(x => x.PlazaId == plazaId).ToListAsync();
        }

        public async Task<IEnumerable<ChargeLog>> getUserLogs(int userId)
        {
            return await _context.ChargeLogs.Where(x => x.UserId == userId).ToListAsync();
        }


        public async Task<IEnumerable<Plaza>> getPlazas()
        {
            return await _context.Plazas.ToListAsync();
        }
        
        public async Task<IEnumerable<Vehicle>> getVehicles()
        {
            return await _context.Vehicles.ToListAsync();
        }
        public async Task<IEnumerable<Vehicle>> getUserVehicles(int userId)
        {
            return await _context.Vehicles.Where(x => x.VehicleOwnerId == userId).ToListAsync();
        }

        public async Task<Vehicle> getVehicleWithId(int id)
        {
            return await _context.Vehicles.Where(x => x.VehicleId == id).FirstOrDefaultAsync();
        }
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Vehicle> getVehicleWithTag(string tagNumber)
        {
            return await _context.Vehicles.Where(x => x.TagNumber == tagNumber)
            .FirstOrDefaultAsync();
        }

        public async Task<Plaza> GetPlaza(int plazaId)
        {
            return await _context.Plazas.Where(x => x.PlazaId == plazaId).FirstOrDefaultAsync();
        }
    }
}