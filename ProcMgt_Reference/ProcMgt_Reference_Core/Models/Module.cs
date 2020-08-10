using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class Module
    {
        public Module()
        {
            ModuleMenu = new HashSet<ModuleMenu>();
        }

        [Column("ModuleID")]
        public int ModuleId { get; set; }
        [StringLength(50)]
        public string ModuleName { get; set; }

        [InverseProperty("Module")]
        public virtual ICollection<ModuleMenu> ModuleMenu { get; set; }
    }
}
