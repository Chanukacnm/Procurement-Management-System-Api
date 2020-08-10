using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class Make
    {
        public Make()
        {
            ItemRequest = new HashSet<ItemRequest>();
            Model = new HashSet<Model>();
            QuotationRequestDetails = new HashSet<QuotationRequestDetails>();
        }

        [Column("MakeID")]
        public Guid MakeId { get; set; }
        [Column("ItemTypeID")]
        public Guid ItemTypeId { get; set; }
        [Required]
        [StringLength(20)]
        public string MakeCode { get; set; }
        [Required]
        [StringLength(100)]
        public string MakeName { get; set; }
        [Required]
        public bool? IsActive { get; set; }
        [Column("UserID")]
        public Guid? UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EntryDateTime { get; set; }

        [ForeignKey("ItemTypeId")]
        [InverseProperty("Make")]
        public virtual ItemType ItemType { get; set; }
        [InverseProperty("Make")]
        public virtual ICollection<ItemRequest> ItemRequest { get; set; }
        [InverseProperty("Make")]
        public virtual ICollection<Model> Model { get; set; }
        [InverseProperty("Make")]
        public virtual ICollection<QuotationRequestDetails> QuotationRequestDetails { get; set; }
    }
}
