using System;
using System.ComponentModel.DataAnnotations;

namespace AcxiomCRM.Models
{
    public class Opportunity
    {
        public int OpportunityId { get; set; }

        [Required(ErrorMessage = "Opportunity name is required")]
        [StringLength(150)]
        public string OpportunityName { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Opportunity Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(50)]
        public string Stage { get; set; } = "Qualification";

        [Range(0, 100, ErrorMessage = "Probability must be between 0 and 100")]
        public int Probability { get; set; }

        [Required(ErrorMessage = "Expected close date is required")]
        public DateTime ExpectedCloseDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Open";

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [StringLength(450)]
        public string AssignedTo { get; set; }
    }
}