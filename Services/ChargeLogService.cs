using System.Threading.Tasks;
using newNet.Data.Repository;
using newNet.Models;

namespace newNet.Services
{
    public class ChargeLogService : IChargeLogService
    {
        private ITollCollectionRepository _repo;
        public ChargeLogService(ITollCollectionRepository repo)
        {
            _repo = repo;
        }

        public async Task addLog(ChargeLog chargeLog)
        {
            _repo.Add(chargeLog);
            await _repo.SaveAll();
        }
    }
}