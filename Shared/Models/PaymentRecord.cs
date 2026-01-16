using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace GIBS.Module.DesignRequest.Models
{
    [Table("GIBSDesignRequest_PaymentRecord")]
    public class PaymentRecord : IAuditable
    {
        [Key]
        public int PaymentId { get; set; }

        public int ModuleId { get; set; }
        public int UserId { get; set; }
        public string Provider { get; set; } // "PayPal"
        public string TransactionId { get; set; } // External ID
        public decimal Amount { get; set; }
        public string Status { get; set; } // "Completed", "Pending", "Failed"
        public string PP_PaymentId { get; set; }
        public string PP_Response { get; set; } // Full PayPal JSON response data
        
        public string PaypalPaymentState { get; set; } // "approved", "failed", etc.
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}