using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class Model
    {
        public Model()
        {
            ItemRequest = new HashSet<ItemRequest>();
            QuotationRequestDetails = new HashSet<QuotationRequestDetails>();
        }

        [Column("ModelID")]
        public Guid ModelId { get; set; }
        [Required]
        [StringLength(20)]
        public string ModelCode { get; set; }
        [Column("ItemTypeID")]
        public Guid ItemTypeId { get; set; }
        [Column("MakeID")]
        public Guid MakeId { get; set; }
        [Required]
        [StringLength(100)]
        public string ModelName { get; set; }
        [Required]
        public bool? IsActive { get; set; }
        [Column("UploadFileID")]
        public Guid? UploadFileId { get; set; }
        public string Image { get; set; }
        [Column("UserID")]
        public Guid? UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EntryDateTime { get; set; }

        [ForeignKey("ItemTypeId")]
        [InverseProperty("Model")]
        public virtual ItemType ItemType { get; set; }
        [ForeignKey("MakeId")]
        [InverseProperty("Model")]
        public virtual Make Make { get; set; }
        [ForeignKey("UploadFileId")]
        [InverseProperty("Model")]
        public virtual UploadFile UploadFile { get; set; }
        [InverseProperty("Model")]
        public virtual ICollection<ItemRequest> ItemRequest { get; set; }
        [InverseProperty("Model")]
        public virtual ICollection<QuotationRequestDetails> QuotationRequestDetails { get; set; }
    }
}
