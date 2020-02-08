using System;
using System.Threading.Tasks;
using newNet.Data.Repository;
using newNet.Models;

namespace newNet.Services
{
    public class ProcessTransaction : IProcessTransaction
    {
        private ITollCollectionRepository _repo;
        private IChargeLogService _chargeService;
        private ISMSMessagingService _sms;

        public ProcessTransaction(ITollCollectionRepository repo, ISMSMessagingService sms)
        {
            _repo = repo;
            _sms = sms;
            _chargeService = new ChargeLogService(_repo);
        }
        private async Task logTransaction(ChargeLog chargeLog)
        {
            await _chargeService.addLog(chargeLog);
        }

        public async Task<bool> processTransaction(string transactionCode)
        {
            string[] codeArray = transactionCode.Split('/');
            string tagNumber = codeArray[0];
            string plazaId = codeArray[1];

            double amount = 0;

            Vehicle vehicle = await _repo.getVehicleWithTag(tagNumber);
            Plaza plaza = await _repo.GetPlaza(Int32.Parse(plazaId));

            switch (vehicle.VehicleType)
            {
                case "jeep":
                    amount = 30 * plaza.Amount;
                    break;
                case "car":
                    amount = 20 * plaza.Amount;
                    break;
                case "tricycle":
                    amount = 10 * plaza.Amount;
                    break;
                case "motorcycle":
                    amount = 5 * plaza.Amount;
                    break;
                case "trailer":
                    amount = 50 * plaza.Amount;
                    break;
                case "lorry":
                    amount = 40 * plaza.Amount;
                    break;
            }

            if (!(await chargeUserAccount(vehicle.VehicleOwnerId, amount)))
                return false;

            await logTransaction(new ChargeLog
            {
                Amount = amount,
                VehicleId = vehicle.VehicleId,
                UserId = vehicle.VehicleOwnerId,
                PlazaId = plaza.PlazaId
            });
            return true;

        }
        private async Task<bool> chargeUserAccount(int userId, double amount)
        {
            User userToCharge = await _repo.getUser(userId);
            userToCharge.AccountBalance -= amount;

            _repo.Update<User>(userToCharge);
            if (await _repo.SaveAll())
            {
                if (userToCharge.AccountBalance < 0)
                {
                    _sms.sendMessage($" Insufficient fund, your account balance is below zero, please Recharge as soon as possible  {amount}", $"{userToCharge.PhoneNumber}");
                    return false;
                }else {
                    _sms.sendMessage($" THANK YOU. your toll account was successfully charged {amount}", $"{userToCharge.PhoneNumber}");
                    return true;
                }
                
            }
            _sms.sendMessage($" An error occured charging your account please pay {amount}, manually", $"{userToCharge.PhoneNumber}");
            return false;
        }
    }
}