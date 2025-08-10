using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace GIBS.Module.DesignRequest.Models
{
    [Table("GIBSDesignRequest_Appliance")]
    public class Appliance : IAuditable
    {
        [Key]
        public int ApplianceId { get; set; }
        public int ModuleId { get; set; }
        public string ApplianceName { get; set; }
        public string ApplianceDescription { get; set; }
        public string ApplianceIcon { get; set; }
        public string ApplianceCode { get; set; }
        public string ApplianceColor { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

       

        

    }
}
