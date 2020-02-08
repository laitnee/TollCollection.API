using System.Collections.Generic;
using System.Threading.Tasks;
using newNet.Models;

namespace newNet.Data.Repository
{
    public interface ITollCollectionRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         void Update<T>(T entity) where T: class;
         Task<bool> SaveAll();
         Task<User> getUser(int userId);
         Task<IEnumerable<ChargeLog>> getLogs();
         Task<IEnumerable<ChargeLog>> getPlazaLogs(int plazaId);
         Task<IEnumerable<ChargeLog>> getUserLogs(int userId);
         Task<IEnumerable<Plaza>> getPlazas();
         Task<Plaza> GetPlaza(int plazaId);
         
         Task<IEnumerable<Vehicle>> getVehicles();
         Task<IEnumerable<Vehicle>> getUserVehicles(int userId);
         Task<Vehicle> getVehicleWithTag(string tagNumber);
         Task<Vehicle> getVehicleWithId(int id);
    }
}