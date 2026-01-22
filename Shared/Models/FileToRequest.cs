using Oqtane.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GIBS.Module.DesignRequest.Models
{
    [Table("GIBSDesignRequest_FileToRequest")]
    public class FileToRequest : IAuditable
    {
        [Key]
        public int FileToRequestId { get; set; }
        public int DesignRequestId { get; set; }
        public int FileId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; } 
  // bool ApprovedForClientReview
  // bool ApproveByClient
        public bool ApprovedForClientReview { get; set; }
        public bool ApproveByClient { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        [ForeignKey("DesignRequestId")]
        [JsonIgnore] // Keep this to prevent cycles, but allows details to be returned in DesignRequest JSON
        public virtual DesignRequest DesignRequest { get; set; }

    }
}