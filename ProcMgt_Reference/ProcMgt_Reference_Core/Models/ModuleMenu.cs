using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class ModuleMenu
    {
        [Column("ModuleMenuID")]
        public int ModuleMenuId { get; set; }
        [Column("ModuleID")]
        public int ModuleId { get; set; }
        [Column("MenuID")]
        public int MenuId { get; set; }

        [ForeignKey("MenuId")]
        [InverseProperty("ModuleMenu")]
        public virtual Menu Menu { get; set; }
        [ForeignKey("ModuleId")]
        [InverseProperty("ModuleMenu")]
        public virtual Module Module { get; set; }
    }
}
