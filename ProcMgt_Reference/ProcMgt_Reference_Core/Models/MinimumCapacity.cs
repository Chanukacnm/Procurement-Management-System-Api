using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class MinimumCapacity
    {
        [Key]
        public Guid MinimumItemsCapacityId { get; set; }
        [StringLength(50)]
        public string MinimumItemsCapacityName { get; set; }
    }
}
