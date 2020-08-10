using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class BusinessUnits
    {
        public BusinessUnits()
        {
            DesignationBusinessUnit = new HashSet<DesignationBusinessUnit>();
        }

        [Column("BusinessUnitsID")]
        public Guid BusinessUnitsId { get; set; }
        [Required]
        [StringLength(50)]
        public string BusinessUnitsName { get; set; }
        [Column("BusinessUnitTypeID")]
        public Guid BusinessUnitTypeId { get; set; }
        public bool IsActive { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EntryDateTime { get; set; }

        [ForeignKey("BusinessUnitTypeId")]
        [InverseProperty("BusinessUnits")]
        public virtual BusinessUnitType BusinessUnitType { get; set; }
        [InverseProperty("BusinessUnits")]
        public virtual ICollection<DesignationBusinessUnit> DesignationBusinessUnit { get; set; }
    }
}
