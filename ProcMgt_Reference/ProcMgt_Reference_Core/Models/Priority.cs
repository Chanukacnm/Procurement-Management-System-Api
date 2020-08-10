using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class Priority
    {
        public Priority()
        {
            ItemRequest = new HashSet<ItemRequest>();
        }

        [Column("PriorityID")]
        public Guid PriorityId { get; set; }
        [Required]
        [StringLength(50)]
        public string PriorityLevelName { get; set; }
        [Column("UserID")]
        public Guid? UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EntryDateTime { get; set; }

        [InverseProperty("Priority")]
        public virtual ICollection<ItemRequest> ItemRequest { get; set; }
    }
}
