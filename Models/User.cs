using System;
using System.Collections.Generic;

namespace newNet.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username {get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NIN { get; set; }
        public double PhoneNumber  {get; set;}
        public string Role { get; set; }
        public double AccountBalance { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; }
    }
}