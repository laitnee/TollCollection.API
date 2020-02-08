namespace newNet.Models.DTOs
{
    public class UserUpdateDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NIN { get; set; }    
        public double PhoneNumber  {get; set;}
    }
}