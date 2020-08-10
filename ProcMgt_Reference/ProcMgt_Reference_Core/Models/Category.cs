using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class Category
    {
        public Category()
        {
            Approver = new HashSet<Approver>();
            ItemRequest = new HashSet<ItemRequest>();
            ItemType = new HashSet<ItemType>();
        }

        [Column("CategoryID")]
        public Guid CategoryId { get; set; }
        [Required]
        [StringLength(20)]
        public string CategoryCode { get; set; }
        [Required]
        [StringLength(30)]
        public string CategoryName { get; set; }
        [Required]
        public bool? IsActive { get; set; }
        [Column("UserID")]
        public Guid? UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EntryDateTime { get; set; }

        [InverseProperty("Category")]
        public virtual ICollection<Approver> Approver { get; set; }
        [InverseProperty("Category")]
        public virtual ICollection<ItemRequest> ItemRequest { get; set; }
        [InverseProperty("Category")]
        public virtual ICollection<ItemType> ItemType { get; set; }
    }
}
