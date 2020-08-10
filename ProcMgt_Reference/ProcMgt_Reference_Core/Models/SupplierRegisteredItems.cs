using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class SupplierRegisteredItems
    {
        [Column("SupplierRegisteredItemsID")]
        public Guid SupplierRegisteredItemsId { get; set; }
        [Column("ItemTypeID")]
        public Guid ItemTypeId { get; set; }
        [Required]
        [StringLength(50)]
        public string MinimumItemCapacity { get; set; }
        public double SupplierLeadTime { get; set; }
        [Column("SupplierID")]
        public Guid SupplierId { get; set; }
        [Column("UserID")]
        public Guid? UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EntryDateTime { get; set; }

        [ForeignKey("ItemTypeId")]
        [InverseProperty("SupplierRegisteredItems")]
        public virtual ItemType ItemType { get; set; }
        [ForeignKey("SupplierId")]
        [InverseProperty("SupplierRegisteredItems")]
        public virtual Supplier Supplier { get; set; }
    }
}
