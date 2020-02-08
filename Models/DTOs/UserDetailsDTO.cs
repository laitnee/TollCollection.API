using System.Collections.Generic;

namespace newNet.Models.DTOs
{
    public class UserDetailsDTO
    {
        public int Id { get; set; }
        public string Username {get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NIN { get; set; }
        public string Role { get; set; }
        public double AccountBalance { get; set; }       
        public double PhoneNumber  {get; set;}
    }
}