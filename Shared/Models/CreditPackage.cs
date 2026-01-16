using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace GIBS.Module.DesignRequest.Models
{
    [Table("GIBSDesignRequest_CreditPackage")]
    // Defines the packages available for purchase
    public class CreditPackage : IAuditable
    {
        public int CreditPackageId { get; set; }
        public int ModuleId { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}