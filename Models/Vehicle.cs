namespace newNet.Models
{
    public class Vehicle
    {
        public int VehicleId {get; set;}
        public string PlateNumber {get; set;}
        public string TagNumber {get; set;}
        public string VehicleName {get; set;}
        public string VehicleType {get; set;}


        public int VehicleOwnerId {get; set;}
        public User VehicleOwner {get; set;}
    }
}