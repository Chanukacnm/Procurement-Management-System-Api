using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class ContactDetails
    {
        [Column("ContactDetailsID")]
        public Guid ContactDetailsId { get; set; }
        [Required]
        [StringLength(100)]
        public string ContactName { get; set; }
        public double ContactMobile { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        public bool? IsDefault { get; set; }
        [Column("SupplierID")]
        public Guid SupplierId { get; set; }
        [Column("UserID")]
        public Guid? UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EntryDateTime { get; set; }

        [ForeignKey("SupplierId")]
        [InverseProperty("ContactDetails")]
        public virtual Supplier Supplier { get; set; }
    }
}
