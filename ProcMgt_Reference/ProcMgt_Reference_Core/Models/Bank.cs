using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class Bank
    {
        public Bank()
        {
            BankBranch = new HashSet<BankBranch>();
            Supplier = new HashSet<Supplier>();
        }

        [Column("BankID")]
        public Guid BankId { get; set; }
        [StringLength(50)]
        public string BankName { get; set; }
        [Column("UserID")]
        public Guid? UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EntryDateTime { get; set; }

        [InverseProperty("Bank")]
        public virtual ICollection<BankBranch> BankBranch { get; set; }
        [InverseProperty("Bank")]
        public virtual ICollection<Supplier> Supplier { get; set; }
    }
}
