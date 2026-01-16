using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;


namespace GIBS.Module.DesignRequest.Models
{
    [Table("GIBSDesignRequest_UserCredit")]
    public class UserCredit : IAuditable
    {
        [Key]
        public int UserCreditId { get; set; }
        public int ModuleId { get; set; }
        public int UserId { get; set; }
        public int CreditBalance { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}