using Oqtane.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIBS.Module.DesignRequest.Models
{
    [Table("GIBSDesignRequest")]
    public class DesignRequest : IAuditable
    {
        [Key]
        public int DesignRequestId { get; set; }
        public int ModuleId { get; set; }
        public string ContactName { get; set; }
        public string Company { get; set; }
        public string ProjectName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string QuestionComments { get; set; }
        public string Interest { get; set; }
        public DateTime InstallationDate { get; set; }
        public string OverallSpaceDimensions { get; set; }
        public string CeilingHeight { get; set; }
        public string LengthOfKitchen { get; set; }
        public string SlopeOfPatio { get; set; }
        public string ShapeConfiguration { get; set; }
        public string DoorStyle { get; set; }
        public string Color { get; set; }
        public string CountertopThickness { get; set; }
        public string CounterDepth { get; set; }
        public string CounterHeight { get; set; }
        public string Status { get; set; }
        public string IP_Address { get; set; }
        public int AssignedToUserId { get; set; } // User ID of the person assigned to this request

        [NotMapped]
        public string Fax { get; set; } // Honeypot field

        [NotMapped]
        public string SendToEmail { get; set; } // Setting field
        [NotMapped]
        public string SendToName { get; set; } // Setting field

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }


        public virtual ICollection<NoteToRequest> Notes { get; set; }
        public virtual ICollection<ApplianceToRequest> Appliances { get; set; }

        public virtual ICollection<FileToRequest> Files { get; set; }
        public virtual ICollection<DetailToRequest> Details { get; set; }

        public DesignRequest()
        {
            Notes = new HashSet<NoteToRequest>();
            Appliances = new HashSet<ApplianceToRequest>();
            Files = new HashSet<FileToRequest>();
            Details = new HashSet<DetailToRequest>();
        }

    }   
       
}
