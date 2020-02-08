using System.Threading.Tasks;
using newNet.Models;

namespace newNet.Services
{
    public interface IChargeLogService
    {
         Task addLog(ChargeLog chargeLog);
    }
}