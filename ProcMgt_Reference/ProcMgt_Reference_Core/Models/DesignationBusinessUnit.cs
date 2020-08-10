using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class DesignationBusinessUnit
    {
        [Column("DesignationBusinessUnitID")]
        public Guid DesignationBusinessUnitId { get; set; }
        [Column("DesignationID")]
        public Guid DesignationId { get; set; }
        [Column("BusinessUnitTypeID")]
        public Guid BusinessUnitTypeId { get; set; }
        [Column("BusinessUnitsID")]
        public Guid? BusinessUnitsId { get; set; }
        [Column("UserID")]
        public Guid? UserId { get; set; }

        [ForeignKey("BusinessUnitTypeId")]
        [InverseProperty("DesignationBusinessUnit")]
        public virtual BusinessUnitType BusinessUnitType { get; set; }
        [ForeignKey("BusinessUnitsId")]
        [InverseProperty("DesignationBusinessUnit")]
        public virtual BusinessUnits BusinessUnits { get; set; }
        [ForeignKey("DesignationId")]
        [InverseProperty("DesignationBusinessUnit")]
        public virtual Designation Designation { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("DesignationBusinessUnit")]
        public virtual User User { get; set; }
    }
}
