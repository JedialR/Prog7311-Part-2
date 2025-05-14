using System;
using System.ComponentModel.DataAnnotations;

namespace AgriConnectSystem.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Production date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Production Date")]
        public DateTime ProductionDate { get; set; }

        [Required(ErrorMessage = "A farmer must be assigned.")]
        [Display(Name = "Farmer")]
        public int FarmerId { get; set; }

        public Farmer Farmer { get; set; }
    }
}
