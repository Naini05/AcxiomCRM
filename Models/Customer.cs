using System;
using System.ComponentModel.DataAnnotations;

namespace AcxiomCRM.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Customer code is required")]
        [StringLength(50)]
        public string CustomerCode { get; set; }

        [Required(ErrorMessage = "Customer name is required")]
        [StringLength(100)]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(150)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone must be a valid 10-digit number")]
        public string Phone { get; set; }

        [StringLength(150)]
        public string CompanyName { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        [Required]
        public string Status { get; set; } = "Active";

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        [StringLength(450)]
        public string CreatedBy { get; set; }
    }
}