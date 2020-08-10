using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class Designation
    {
        public Designation()
        {
            ApprovalFlowManagement = new HashSet<ApprovalFlowManagement>();
            BusinessUnitType = new HashSet<BusinessUnitType>();
            DesignationBusinessUnit = new HashSet<DesignationBusinessUnit>();
            User = new HashSet<User>();
        }

        [Column("DesignationID")]
        public Guid DesignationId { get; set; }
        [Required]
        [StringLength(50)]
        public string DesignationName { get; set; }
        [Required]
        [StringLength(20)]
        public string DesignationCode { get; set; }
        [Required]
        public bool? IsActive { get; set; }
        [StringLength(50)]
        public string BusinessUnitTypeName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EntryDateTime { get; set; }

        [InverseProperty("Designation")]
        public virtual ICollection<ApprovalFlowManagement> ApprovalFlowManagement { get; set; }
        [InverseProperty("Designation")]
        public virtual ICollection<BusinessUnitType> BusinessUnitType { get; set; }
        [InverseProperty("Designation")]
        public virtual ICollection<DesignationBusinessUnit> DesignationBusinessUnit { get; set; }
        [InverseProperty("Designation")]
        public virtual ICollection<User> User { get; set; }
    }
}
