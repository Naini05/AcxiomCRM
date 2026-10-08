using System;
using System.ComponentModel.DataAnnotations;

namespace AcxiomCRM.Models
{
    public class Lead
    {
        public int LeadId { get; set; }

        [Required(ErrorMessage = "Lead name is required")]
        [StringLength(100)]
        public string LeadName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(150)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone must be a valid 10-digit number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Lead source is required")]
        [StringLength(50)]
        public string LeadSource { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "New";

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [StringLength(450)]
        public string AssignedTo { get; set; }
    }
}