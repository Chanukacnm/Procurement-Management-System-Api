using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class User
    {
        public User()
        {
            Approver = new HashSet<Approver>();
            DesignationBusinessUnit = new HashSet<DesignationBusinessUnit>();
            IssueHeader = new HashSet<IssueHeader>();
            ItemRequestApprovedUser = new HashSet<ItemRequest>();
            ItemRequestIssuedUser = new HashSet<ItemRequest>();
            ItemRequestRequestedUser = new HashSet<ItemRequest>();
            Poheader = new HashSet<Poheader>();
        }

        [Column("UserID")]
        public Guid UserId { get; set; }
        [Required]
        [StringLength(100)]
        public string UserName { get; set; }
        [Required]
        [StringLength(50)]
        public string Password { get; set; }
        [StringLength(20)]
        public string EmployeeNo { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Column("CompanyID")]
        public Guid CompanyId { get; set; }
        [Column("DepartmentID")]
        public Guid DepartmentId { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        [Column("DesignationID")]
        public Guid DesignationId { get; set; }
        [Column("UserRoleID")]
        public Guid UserRoleId { get; set; }
        [Required]
        public bool? IsActive { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateTime { get; set; }
        public bool? IsApprovalUser { get; set; }

        [ForeignKey("DepartmentId")]
        [InverseProperty("User")]
        public virtual Department Department { get; set; }
        [ForeignKey("DesignationId")]
        [InverseProperty("User")]
        public virtual Designation Designation { get; set; }
        [ForeignKey("UserRoleId")]
        [InverseProperty("User")]
        public virtual UserRole UserRole { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Approver> Approver { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<DesignationBusinessUnit> DesignationBusinessUnit { get; set; }
        [InverseProperty("IssuedUser")]
        public virtual ICollection<IssueHeader> IssueHeader { get; set; }
        [InverseProperty("ApprovedUser")]
        public virtual ICollection<ItemRequest> ItemRequestApprovedUser { get; set; }
        [InverseProperty("IssuedUser")]
        public virtual ICollection<ItemRequest> ItemRequestIssuedUser { get; set; }
        [InverseProperty("RequestedUser")]
        public virtual ICollection<ItemRequest> ItemRequestRequestedUser { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Poheader> Poheader { get; set; }
    }
}
