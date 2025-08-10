using GIBS.Module.DesignRequest.Models;
using Oqtane.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GIBS.Module.DesignRequest.Models
{
    [Table("GIBSDesignRequest_DetailToRequest")]
    public class DetailToRequest : IAuditable
    {
        [Key]
        public int DetailToRequestId { get; set; }
        public int DesignRequestId { get; set; }
        public int DetailId { get; set; }
        public string DetailModel { get; set; }
        public string DetailSize { get; set; }
        public string DetailColor { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        [ForeignKey("DesignRequestId")]
        [JsonIgnore] // Keep this to prevent cycles, but allows details to be returned in DesignRequest JSON
        public virtual DesignRequest DesignRequest { get; set; }

        [ForeignKey("DetailId")]
        public virtual Detail Detail { get; set; }
    }
}