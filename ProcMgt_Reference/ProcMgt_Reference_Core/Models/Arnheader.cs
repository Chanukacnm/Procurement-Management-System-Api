using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    [Table("ARNHeader")]
    public partial class Arnheader
    {
        public Arnheader()
        {
            Arndetail = new HashSet<Arndetail>();
        }

        [Column("ARNHeaderID")]
        public Guid ArnheaderId { get; set; }
        [Column("POHeaderID")]
        public Guid PoheaderId { get; set; }
        [Required]
        [Column("ARNNumber")]
        [StringLength(50)]
        public string Arnnumber { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime RecivedDate { get; set; }
        [StringLength(100)]
        public string InvoiceNo { get; set; }
        public string InvoiceAtt { get; set; }
        [Column("ARNRemark")]
        public string Arnremark { get; set; }

        [ForeignKey("PoheaderId")]
        [InverseProperty("Arnheader")]
        public virtual Poheader Poheader { get; set; }
        [InverseProperty("Arnheader")]
        public virtual ICollection<Arndetail> Arndetail { get; set; }
    }
}
