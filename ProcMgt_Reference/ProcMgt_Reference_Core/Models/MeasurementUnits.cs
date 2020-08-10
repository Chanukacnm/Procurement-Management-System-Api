using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class MeasurementUnits
    {
        public MeasurementUnits()
        {
            ItemType = new HashSet<ItemType>();
            QuotationRequestDetails = new HashSet<QuotationRequestDetails>();
        }

        [Key]
        [Column("MeasurementUnitID")]
        public Guid MeasurementUnitId { get; set; }
        [Required]
        [StringLength(30)]
        public string MeasurementUnitName { get; set; }
        [Required]
        [StringLength(20)]
        public string Code { get; set; }
        [Required]
        public bool? IsActive { get; set; }
        [Column("UserID")]
        public Guid? UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EntryDateTime { get; set; }

        [InverseProperty("MeasurementUnit")]
        public virtual ICollection<ItemType> ItemType { get; set; }
        [InverseProperty("MeasurementUnit")]
        public virtual ICollection<QuotationRequestDetails> QuotationRequestDetails { get; set; }
    }
}
