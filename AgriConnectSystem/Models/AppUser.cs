using System.ComponentModel.DataAnnotations;

namespace AgriConnectSystem.Models
{
    public class AppUser
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } // "Farmer" or "Employee"
    }
}
