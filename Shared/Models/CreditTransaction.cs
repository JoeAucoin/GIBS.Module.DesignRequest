using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace GIBS.Module.DesignRequest.Models
{
    [Table("GIBSDesignRequest_CreditTransaction")]
    public class CreditTransaction : IAuditable
    {
        [Key]
        public int TransactionId { get; set; }
        
        public int UserId { get; set; }
        public int Credits { get; set; }

        // Optional: Track the monetary value associated with this transaction
        // Only applicable for Purchases (Cost) or potentially Refunds
        public decimal Price { get; set; }
        public string TransactionType { get; set; } // "Purchase", "DesignRequest", "Revision", "Refund"
        public int? DesignRequestId { get; set; } // Optional link to a job
        public DateTime TransactionDate { get; set; }
        public string Notes { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        [NotMapped] 
        public string DisplayName { get; set; }

    }
}
