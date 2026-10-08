using System;
using System.ComponentModel.DataAnnotations;

namespace AcxiomCRM.Models
{
    public class FollowUp
    {
        public int FollowUpId { get; set; }
        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int? LeadId { get; set; }

        [Required(ErrorMessage = "Follow-up date is required")]
        public DateTime FollowUpDate { get; set; }

        [Required(ErrorMessage = "Follow-up type is required")]
        [StringLength(50)]
        public string FollowUpType { get; set; }

        public string Remarks { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Planned";

        [Required]
        [StringLength(450)]
        public string AssignedTo { get; set; }
    }
}