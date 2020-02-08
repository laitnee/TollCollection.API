namespace newNet.Models
{
    public class ChargeLog
    {
        public int ChargeLogId {get; set;}
        public double Amount {get; set;}
        public int VehicleId {get; set;}
        public int UserId {get; set;}
        public int PlazaId {get; set;}
    }
}