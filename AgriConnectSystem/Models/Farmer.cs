using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AgriConnectSystem.Models
{
    public class Farmer
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        public string PhoneNumber { get; set; } = "N/A";

        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}

