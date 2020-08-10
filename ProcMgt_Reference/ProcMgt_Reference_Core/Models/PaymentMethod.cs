using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class PaymentMethod
    {
        public PaymentMethod()
        {
            Poheader = new HashSet<Poheader>();
            Supplier = new HashSet<Supplier>();
        }

        [Column("PaymentMethodID")]
        public Guid PaymentMethodId { get; set; }
        [Required]
        [StringLength(100)]
        public string PaymentMethodName { get; set; }
        [Required]
        public bool? IsActive { get; set; }
        [Required]
        [StringLength(20)]
        public string PaymentMethodCode { get; set; }
        [Column("UserID")]
        public Guid? UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EntryDateTime { get; set; }

        [InverseProperty("PaymentMethod")]
        public virtual ICollection<Poheader> Poheader { get; set; }
        [InverseProperty("PaymentMethod")]
        public virtual ICollection<Supplier> Supplier { get; set; }
    }
}
