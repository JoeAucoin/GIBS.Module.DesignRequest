using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace GIBS.Module.DesignRequest.Models
{
    [Table("GIBSDesignRequest_CouponRedemption")]
    public class CouponRedemption : IAuditable
    {
        [Key]
        public int CouponRedemptionId { get; set; }

        public int CouponCodeId { get; set; }
        public int ModuleId { get; set; }
        public int UserId { get; set; }

        // Optional link to a payment (e.g., zero-dollar PaymentRecord) if you want the trace
        public int? PaymentRecordId { get; set; }

        // Credits granted on this redemption
        public int CreditsGranted { get; set; }

        public DateTime RedeemedOnUtc { get; set; }

        [StringLength(64)]
        public string IpAddress { get; set; }

        [StringLength(256)]
        public string Notes { get; set; }

        // Audit
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}