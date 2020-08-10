using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class Tax
    {
        [Column("TaxID")]
        public Guid TaxId { get; set; }
        [Required]
        [StringLength(50)]
        public string TaxCode { get; set; }
        [Required]
        [StringLength(50)]
        public string TaxName { get; set; }
        public double Percentage { get; set; }
        [Column("UserID")]
        public Guid? UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EntryDateTime { get; set; }
    }
}
