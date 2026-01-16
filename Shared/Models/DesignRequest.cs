using Oqtane.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace GIBS.Module.DesignRequest.Models
{
    [Table("GIBSDesignRequest")]
    public class DesignRequest : IAuditable
    {
        [Key]
        public int DesignRequestId { get; set; }
        public int ModuleId { get; set; }
        public int? UserId { get; set; } // Oqtane User
        public string ContactName { get; set; }
        public string Company { get; set; }
        public string ProjectName { get; set; }
        public string Address { get; set; }
        public string MailingAddress { get; set; } // Optional mailing address
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
        public int? ProjectManagerUserId { get; set; } // New field for Project Manager User ID
        public string ProjectStatus { get; set; } // New field for Project Status
        public string HandlePull { get; set; } // New field for Handle/Pull preference

        public int? RevisionsUsed { get; set; }
        public int? CreditsSpent { get; set; }


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

        [NotMapped]
        public bool FileUploadedRecently => Files != null && Files.Any(f => f.CreatedOn >= DateTime.UtcNow.AddDays(-3));

        [NotMapped]
        public bool NoteAddedRecently => Notes != null && Notes.Any(n => n.CreatedOn >= DateTime.UtcNow.AddDays(-3));

        public virtual ICollection<NoteToRequest> Notes { get; set; }
        public virtual ICollection<ApplianceToRequest> Appliances { get; set; }

        public virtual ICollection<FileToRequest> Files { get; set; }
        public virtual ICollection<DetailToRequest> Details { get; set; }

        public virtual ICollection<NotificationToRequest> Notifications { get; set; }

        public DesignRequest()
        {
            Notes = new HashSet<NoteToRequest>();
            Appliances = new HashSet<ApplianceToRequest>();
            Files = new HashSet<FileToRequest>();
            Details = new HashSet<DetailToRequest>();
            Notifications = new HashSet<NotificationToRequest>();
        }

    }

}