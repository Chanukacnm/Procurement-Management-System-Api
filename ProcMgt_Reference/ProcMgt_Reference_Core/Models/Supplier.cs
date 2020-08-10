using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            ContactDetails = new HashSet<ContactDetails>();
            SupplierRegisteredItems = new HashSet<SupplierRegisteredItems>();
        }

        [Column("SupplierID")]
        public Guid SupplierId { get; set; }
        [Required]
        [StringLength(100)]
        public string SupplierName { get; set; }
        [Required]
        [StringLength(15)]
        public string BrNo { get; set; }
        [Required]
        [StringLength(100)]
        public string Address { get; set; }
        public double Telephone { get; set; }
        [Required]
        [StringLength(50)]
        public string BillingName { get; set; }
        [Required]
        [StringLength(100)]
        public string BillingAddress { get; set; }
        [Column("BankID")]
        public Guid BankId { get; set; }
        [Column("BranchID")]
        public Guid BranchId { get; set; }
        [Column("AccountTypeID")]
        public Guid AccountTypeId { get; set; }
        public double AccountNo { get; set; }
        [Required]
        [StringLength(50)]
        public string AccountName { get; set; }
        [Column("PaymentMethodID")]
        public Guid PaymentMethodId { get; set; }
        [Column("SupplierTypeID")]
        public Guid SupplierTypeId { get; set; }
        public bool IsActive { get; set; }
        [Column("UserID")]
        public Guid? UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EntryDateTime { get; set; }

        [ForeignKey("AccountTypeId")]
        [InverseProperty("Supplier")]
        public virtual AccountType AccountType { get; set; }
        [ForeignKey("BankId")]
        [InverseProperty("Supplier")]
        public virtual Bank Bank { get; set; }
        [ForeignKey("BranchId")]
        [InverseProperty("Supplier")]
        public virtual BankBranch Branch { get; set; }
        [ForeignKey("PaymentMethodId")]
        [InverseProperty("Supplier")]
        public virtual PaymentMethod PaymentMethod { get; set; }
        [ForeignKey("SupplierTypeId")]
        [InverseProperty("Supplier")]
        public virtual SupplierType SupplierType { get; set; }
        [InverseProperty("Supplier")]
        public virtual ICollection<ContactDetails> ContactDetails { get; set; }
        [InverseProperty("Supplier")]
        public virtual ICollection<SupplierRegisteredItems> SupplierRegisteredItems { get; set; }
    }
}
