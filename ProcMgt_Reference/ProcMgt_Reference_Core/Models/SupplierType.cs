using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class SupplierType
    {
        public SupplierType()
        {
            Supplier = new HashSet<Supplier>();
        }

        [Column("SupplierTypeID")]
        public Guid SupplierTypeId { get; set; }
        [StringLength(50)]
        public string SupplierTypeName { get; set; }
        [Column("UserID")]
        public Guid? UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EntryDateTime { get; set; }

        [InverseProperty("SupplierType")]
        public virtual ICollection<Supplier> Supplier { get; set; }
    }
}
