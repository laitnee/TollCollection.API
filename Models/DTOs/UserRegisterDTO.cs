using System.ComponentModel.DataAnnotations;

namespace newNet.Models.DTOs
{
    public class UserRegisterDTO
    {
        [Required]
        public string Username {get; set; }
        
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string NIN { get; set; }

        [Required]
        public double PhoneNumber  {get; set;}

        [Required]
        public string Role { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify a password between 4 and 8 characters")]
        public string Password { get; set; }

    }
}