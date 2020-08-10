using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class RoleMenu
    {
        [Column("RoleMenuID")]
        public int RoleMenuId { get; set; }
        [Column("MenuID")]
        public int MenuId { get; set; }
        [Column("UserRoleID")]
        public Guid UserRoleId { get; set; }

        [ForeignKey("MenuId")]
        [InverseProperty("RoleMenu")]
        public virtual Menu Menu { get; set; }
        [ForeignKey("UserRoleId")]
        [InverseProperty("RoleMenu")]
        public virtual UserRole UserRole { get; set; }
    }
}
