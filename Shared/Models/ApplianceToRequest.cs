using GIBS.Module.DesignRequest.Models;
using Oqtane.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GIBS.Module.DesignRequest.Models
{
    [Table("GIBSDesignRequest_ApplianceToRequest")]
    public class ApplianceToRequest : IAuditable
    {
        [Key]
        public int ApplianceToRequestId { get; set; }
        public int DesignRequestId { get; set; }
        public int ApplianceId { get; set; }
        public string BrandModel { get; set; }
        public string Size { get; set; }
        public string FuelType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        [ForeignKey("DesignRequestId")]
        [JsonIgnore] // Keep this to prevent cycles, but allows appliances to be returned in DesignRequest JSON
        public virtual DesignRequest DesignRequest { get; set; }

        [ForeignKey("ApplianceId")]
        public virtual Appliance Appliance { get; set; }
    }
}