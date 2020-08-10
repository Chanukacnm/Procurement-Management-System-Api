using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class Item
    {
        public Item()
        {
            Arndetail = new HashSet<Arndetail>();
            ItemRequest = new HashSet<ItemRequest>();
            Podetail = new HashSet<Podetail>();
            QuotationRequestDetails = new HashSet<QuotationRequestDetails>();
            Stock = new HashSet<Stock>();
        }

        [Column("ItemID")]
        public Guid ItemId { get; set; }
        [Column("ItemTypeID")]
        public Guid ItemTypeId { get; set; }
        [Required]
        [StringLength(500)]
        public string ItemDescription { get; set; }
        [Required]
        [StringLength(50)]
        public string ItemCode { get; set; }
        public double? CurrentQty { get; set; }
        [Required]
        public bool? IsActive { get; set; }
        public double? ReOrderQuantity { get; set; }
        [Column("initialQty")]
        public double InitialQty { get; set; }
        [Column("UserID")]
        public Guid? UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EntryDateTime { get; set; }

        [ForeignKey("ItemTypeId")]
        [InverseProperty("Item")]
        public virtual ItemType ItemType { get; set; }
        [InverseProperty("Item")]
        public virtual ICollection<Arndetail> Arndetail { get; set; }
        [InverseProperty("Item")]
        public virtual ICollection<ItemRequest> ItemRequest { get; set; }
        [InverseProperty("Item")]
        public virtual ICollection<Podetail> Podetail { get; set; }
        [InverseProperty("Item")]
        public virtual ICollection<QuotationRequestDetails> QuotationRequestDetails { get; set; }
        [InverseProperty("Item")]
        public virtual ICollection<Stock> Stock { get; set; }
    }
}
