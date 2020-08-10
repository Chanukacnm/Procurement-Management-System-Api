using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class UserRole
    {
        public UserRole()
        {
            RoleMenu = new HashSet<RoleMenu>();
            User = new HashSet<User>();
        }

        [Column("UserRoleID")]
        public Guid UserRoleId { get; set; }
        [Required]
        [StringLength(30)]
        public string UserRoleName { get; set; }
        [Required]
        [StringLength(20)]
        public string Code { get; set; }
        [Required]
        public bool? IsActive { get; set; }

        [InverseProperty("UserRole")]
        public virtual ICollection<RoleMenu> RoleMenu { get; set; }
        [InverseProperty("UserRole")]
        public virtual ICollection<User> User { get; set; }
    }
}
