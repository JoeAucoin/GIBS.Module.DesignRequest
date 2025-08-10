using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace GIBS.Module.DesignRequest.Models
{
    [Table("GIBSDesignRequest_Detail")]
    public class Detail : IAuditable
    {
        [Key]
        public int DetailId { get; set; }
        public int ModuleId { get; set; }
        public string DetailName { get; set; }
        public string DetailDescription { get; set; }
        public string DetailIcon { get; set; }
        public string DetailCode { get; set; }
        public string DetailColor { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
