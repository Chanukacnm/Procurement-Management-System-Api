using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class BusinessUnitType
    {
        public BusinessUnitType()
        {
            BusinessUnits = new HashSet<BusinessUnits>();
            DesignationBusinessUnit = new HashSet<DesignationBusinessUnit>();
        }

        [Column("BusinessUnitTypeID")]
        public Guid BusinessUnitTypeId { get; set; }
        [Required]
        [StringLength(50)]
        public string BusinessUnitTypeName { get; set; }
        [Column("DesignationID")]
        public Guid DesignationId { get; set; }
        public bool IsActive { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EntryDateTime { get; set; }

        [ForeignKey("DesignationId")]
        [InverseProperty("BusinessUnitType")]
        public virtual Designation Designation { get; set; }
        [InverseProperty("BusinessUnitType")]
        public virtual ICollection<BusinessUnits> BusinessUnits { get; set; }
        [InverseProperty("BusinessUnitType")]
        public virtual ICollection<DesignationBusinessUnit> DesignationBusinessUnit { get; set; }
    }
}
