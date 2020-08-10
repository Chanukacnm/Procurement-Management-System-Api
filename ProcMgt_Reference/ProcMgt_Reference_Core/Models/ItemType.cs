using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class ItemType
    {
        public ItemType()
        {
            Item = new HashSet<Item>();
            ItemRequest = new HashSet<ItemRequest>();
            Make = new HashSet<Make>();
            Model = new HashSet<Model>();
            SupplierRegisteredItems = new HashSet<SupplierRegisteredItems>();
        }

        [Column("ItemTypeID")]
        public Guid ItemTypeId { get; set; }
        [Required]
        [StringLength(100)]
        public string ItemTypeName { get; set; }
        [Column("CategoryID")]
        public Guid CategoryId { get; set; }
        [Required]
        [StringLength(30)]
        public string ItemTypeCode { get; set; }
        public double? DepreciationRate { get; set; }
        [Column("MeasurementUnitID")]
        public Guid MeasurementUnitId { get; set; }
        [Column("ApprovalPatternTypeID")]
        public Guid ApprovalPatternTypeId { get; set; }
        [Required]
        public bool? IsDisposable { get; set; }
        [Required]
        public bool? IsActive { get; set; }
        [Column("UserID")]
        public Guid? UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EntryDateTime { get; set; }

        [ForeignKey("ApprovalPatternTypeId")]
        [InverseProperty("ItemType")]
        public virtual ApprovalPatternType ApprovalPatternType { get; set; }
        [ForeignKey("CategoryId")]
        [InverseProperty("ItemType")]
        public virtual Category Category { get; set; }
        [ForeignKey("MeasurementUnitId")]
        [InverseProperty("ItemType")]
        public virtual MeasurementUnits MeasurementUnit { get; set; }
        [InverseProperty("ItemType")]
        public virtual ICollection<Item> Item { get; set; }
        [InverseProperty("ItemType")]
        public virtual ICollection<ItemRequest> ItemRequest { get; set; }
        [InverseProperty("ItemType")]
        public virtual ICollection<Make> Make { get; set; }
        [InverseProperty("ItemType")]
        public virtual ICollection<Model> Model { get; set; }
        [InverseProperty("ItemType")]
        public virtual ICollection<SupplierRegisteredItems> SupplierRegisteredItems { get; set; }
    }
}
