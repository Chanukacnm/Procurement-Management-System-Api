using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class Company
    {
        public Company()
        {
            CompanyGroupCompany = new HashSet<CompanyGroupCompany>();
            Department = new HashSet<Department>();
            GroupCompany = new HashSet<GroupCompany>();
        }

        [Column("CompanyID")]
        public Guid CompanyId { get; set; }
        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }
        [Required]
        [StringLength(50)]
        public string CompanyCode { get; set; }
        [Required]
        public bool? IsActive { get; set; }
        public bool? IsGroupofCompany { get; set; }
        [Column("CompanyLogoID")]
        public Guid? CompanyLogoId { get; set; }
        [StringLength(500)]
        public string CompanyAddressLine1 { get; set; }
        [StringLength(500)]
        public string CompanyAddressLine2 { get; set; }
        [StringLength(500)]
        public string CompanyAddressLine3 { get; set; }
        [StringLength(500)]
        public string CompanyAddressLine4 { get; set; }
        public double? CompanyTelephoneNo { get; set; }
        [StringLength(50)]
        public string CompanyFax { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(100)]
        public string CompanyWeb { get; set; }
        [StringLength(50)]
        public string CompanyRegistrationNo { get; set; }
        [StringLength(50)]
        public string VatRegistrationNo { get; set; }

        [InverseProperty("Company")]
        public virtual ICollection<CompanyGroupCompany> CompanyGroupCompany { get; set; }
        [InverseProperty("Company")]
        public virtual ICollection<Department> Department { get; set; }
        [InverseProperty("Company")]
        public virtual ICollection<GroupCompany> GroupCompany { get; set; }
    }
}
