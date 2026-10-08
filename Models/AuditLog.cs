using System;

namespace AcxiomCRM.Models
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }
        public string UserId { get; set; }
        public string Action { get; set; }
        public string EntityName { get; set; }
        public int RecordId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string IpAddress { get; set; }
    }
}