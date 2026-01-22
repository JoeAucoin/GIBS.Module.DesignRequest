using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace GIBS.Module.DesignRequest.Models
{
    [Table("GIBSDesignRequest_CouponCode")]
    public class CouponCode : IAuditable
    {
        [Key]
        public int CouponCodeId { get; set; }

        public int ModuleId { get; set; }

        [Required, StringLength(64)]
        public string Code { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        // Credits granted when redeemed
        public int Credits { get; set; }

        public DateTime? StartDateUtc { get; set; }
        public DateTime? EndDateUtc { get; set; }

        // Limits
        public int? MaxRedemptions { get; set; }
        public int? PerUserLimit { get; set; }

        public bool IsActive { get; set; } = true;

        // Audit
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}