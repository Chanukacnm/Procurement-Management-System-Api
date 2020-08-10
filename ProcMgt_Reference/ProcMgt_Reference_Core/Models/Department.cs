using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class Department
    {
        public Department()
        {
            ItemRequest = new HashSet<ItemRequest>();
            User = new HashSet<User>();
        }

        [Column("DepartmentID")]
        public Guid DepartmentId { get; set; }
        [Column("CompanyID")]
        public Guid CompanyId { get; set; }
        [Required]
        [StringLength(50)]
        public string DepartmentName { get; set; }
        [Required]
        [StringLength(20)]
        public string Code { get; set; }
        [Required]
        public bool? IsActive { get; set; }

        [ForeignKey("CompanyId")]
        [InverseProperty("Department")]
        public virtual Company Company { get; set; }
        [InverseProperty("Department")]
        public virtual ICollection<ItemRequest> ItemRequest { get; set; }
        [InverseProperty("Department")]
        public virtual ICollection<User> User { get; set; }
    }
}
