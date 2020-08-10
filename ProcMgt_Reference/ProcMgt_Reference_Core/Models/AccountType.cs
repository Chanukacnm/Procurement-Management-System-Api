using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class AccountType
    {
        public AccountType()
        {
            Supplier = new HashSet<Supplier>();
        }

        [Column("AccountTypeID")]
        public Guid AccountTypeId { get; set; }
        [StringLength(50)]
        public string AccountTypeName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EntryDateTime { get; set; }
        [Column("UserID")]
        public Guid? UserId { get; set; }

        [InverseProperty("AccountType")]
        public virtual ICollection<Supplier> Supplier { get; set; }
    }
}
