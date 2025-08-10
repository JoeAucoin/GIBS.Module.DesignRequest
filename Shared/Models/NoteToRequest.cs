using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Oqtane.Models;

namespace GIBS.Module.DesignRequest.Models
{
    [Table("GIBSDesignRequest_NoteToRequest")]
    public class NoteToRequest : IAuditable
    {
        [Key]
        public int NoteId { get; set; }
        public int DesignRequestId { get; set; }
        public string Note { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        [ForeignKey("DesignRequestId")]
        [JsonIgnore] // Keep this to prevent cycles, but allows details to be returned in DesignRequest JSON
        public virtual DesignRequest DesignRequest { get; set; }
    }
}