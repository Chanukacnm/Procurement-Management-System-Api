using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class Menu
    {
        public Menu()
        {
            ModuleMenu = new HashSet<ModuleMenu>();
            RoleMenu = new HashSet<RoleMenu>();
        }

        [Column("MenuID")]
        public int MenuId { get; set; }
        [Required]
        [Column("MenuIDHTML")]
        [StringLength(50)]
        public string MenuIdhtml { get; set; }
        [Required]
        [StringLength(50)]
        public string MenuName { get; set; }
        [Required]
        [Column("URL")]
        [StringLength(200)]
        public string Url { get; set; }
        [StringLength(50)]
        public string Icon { get; set; }

        [InverseProperty("Menu")]
        public virtual ICollection<ModuleMenu> ModuleMenu { get; set; }
        [InverseProperty("Menu")]
        public virtual ICollection<RoleMenu> RoleMenu { get; set; }
    }
}
