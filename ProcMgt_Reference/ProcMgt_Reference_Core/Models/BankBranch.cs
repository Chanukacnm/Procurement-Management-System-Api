using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class BankBranch
    {
        public BankBranch()
        {
            Supplier = new HashSet<Supplier>();
        }

        [Column("BranchID")]
        public Guid BranchId { get; set; }
        [StringLength(50)]
        public string BranchName { get; set; }
        [Column("BankID")]
        public Guid BankId { get; set; }
        [Column("UserID")]
        public Guid? UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EntryDateTime { get; set; }

        [ForeignKey("BankId")]
        [InverseProperty("BankBranch")]
        public virtual Bank Bank { get; set; }
        [InverseProperty("Branch")]
        public virtual ICollection<Supplier> Supplier { get; set; }
    }
}
