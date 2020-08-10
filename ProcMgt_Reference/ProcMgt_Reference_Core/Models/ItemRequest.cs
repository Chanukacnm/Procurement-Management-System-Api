using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class ItemRequest
    {
        [Column("ItemRequestID")]
        public Guid ItemRequestId { get; set; }
        [Required]
        [StringLength(100)]
        public string RequestTitle { get; set; }
        [Column("CategoryID")]
        public Guid CategoryId { get; set; }
        [Column("MakeID")]
        public Guid? MakeId { get; set; }
        [Column("ModelID")]
        public Guid? ModelId { get; set; }
        [Column("ItemTypeID")]
        public Guid ItemTypeId { get; set; }
        [Column("ItemID")]
        public Guid ItemId { get; set; }
        public int NoOfUnits { get; set; }
        [Column("PriorityID")]
        public Guid PriorityId { get; set; }
        public bool IsReplaceble { get; set; }
        [StringLength(50)]
        public string AssetCode { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }
        [Column(TypeName = "date")]
        public DateTime RequiredDate { get; set; }
        [Column("RequestedUserID")]
        public Guid RequestedUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime RequestedDateTime { get; set; }
        public bool IsApproved { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ApprovedDateTime { get; set; }
        [StringLength(100)]
        public string ApprovalComment { get; set; }
        [Column("ApproverID")]
        public Guid? ApproverId { get; set; }
        [Column("DepartmentID")]
        public Guid DepartmentId { get; set; }
        public bool IsRejected { get; set; }
        [StringLength(50)]
        public string Attachment { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedRequestedDateTime { get; set; }
        [Column("ApprovedUserID")]
        public Guid? ApprovedUserId { get; set; }
        public bool IsIssued { get; set; }
        [Column("IssuedUserID")]
        public Guid? IssuedUserId { get; set; }

        [ForeignKey("ApprovedUserId")]
        [InverseProperty("ItemRequestApprovedUser")]
        public virtual User ApprovedUser { get; set; }
        [ForeignKey("CategoryId")]
        [InverseProperty("ItemRequest")]
        public virtual Category Category { get; set; }
        [ForeignKey("DepartmentId")]
        [InverseProperty("ItemRequest")]
        public virtual Department Department { get; set; }
        [ForeignKey("IssuedUserId")]
        [InverseProperty("ItemRequestIssuedUser")]
        public virtual User IssuedUser { get; set; }
        [ForeignKey("ItemId")]
        [InverseProperty("ItemRequest")]
        public virtual Item Item { get; set; }
        [ForeignKey("ItemTypeId")]
        [InverseProperty("ItemRequest")]
        public virtual ItemType ItemType { get; set; }
        [ForeignKey("MakeId")]
        [InverseProperty("ItemRequest")]
        public virtual Make Make { get; set; }
        [ForeignKey("ModelId")]
        [InverseProperty("ItemRequest")]
        public virtual Model Model { get; set; }
        [ForeignKey("PriorityId")]
        [InverseProperty("ItemRequest")]
        public virtual Priority Priority { get; set; }
        [ForeignKey("RequestedUserId")]
        [InverseProperty("ItemRequestRequestedUser")]
        public virtual User RequestedUser { get; set; }
    }
}
