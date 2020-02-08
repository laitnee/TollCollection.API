using System.Threading.Tasks;
using newNet.Models;

namespace newNet.Services
{
    public interface IProcessTransaction
    {
         Task<bool> processTransaction(string transactionCode);
         
    }
}