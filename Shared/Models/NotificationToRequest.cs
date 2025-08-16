using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Oqtane.Models;

namespace GIBS.Module.DesignRequest.Models
{
    [Table("GIBSDesignRequest_NotificationToRequest")]
    public class NotificationToRequest : IAuditable
    {
        [Key]
        public int NotificationToRequestId { get; set; }
        public int DesignRequestId { get; set; }
        public int NotificationId { get; set; }
        public int FromUserId { get; set; }
        public string FromDisplayName { get; set; }
        public string FromEmail { get; set; }
        public int ToUserId { get; set; }
        public string ToDisplayName { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        [ForeignKey("DesignRequestId")]
        [JsonIgnore] // Keep this to prevent cycles, but allows details to be returned in DesignRequest JSON
        public virtual DesignRequest DesignRequest { get; set; }
    }
}