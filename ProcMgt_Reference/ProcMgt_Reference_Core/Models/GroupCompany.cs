using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class GroupCompany
    {
        public GroupCompany()
        {
            CompanyGroupCompany = new HashSet<CompanyGroupCompany>();
        }

        [Column("GroupCompanyID")]
        public Guid GroupCompanyId { get; set; }
        [Required]
        [StringLength(100)]
        public string GroupCompanyName { get; set; }
        [Required]
        [StringLength(50)]
        public string GroupCompanyCode { get; set; }
        public bool IsActive { get; set; }
        [Column("GroupCompanyLogoID")]
        public Guid? GroupCompanyLogoId { get; set; }
        [StringLength(500)]
        public string GroupCompanyAddressLine1 { get; set; }
        [StringLength(500)]
        public string GroupCompanyAddressLine2 { get; set; }
        [StringLength(500)]
        public string GroupCompanyAddressLine3 { get; set; }
        [StringLength(500)]
        public string GroupCompanyAddressLine4 { get; set; }
        [Column("GCompanyTelephoneNo")]
        public double? GcompanyTelephoneNo { get; set; }
        [Column("GCompanyFax")]
        [StringLength(50)]
        public string GcompanyFax { get; set; }
        [Column("GCompanyEmail")]
        [StringLength(100)]
        public string GcompanyEmail { get; set; }
        [Column("GCompanyWeb")]
        [StringLength(100)]
        public string GcompanyWeb { get; set; }
        [Column("GCompanyRegistrationNo")]
        [StringLength(50)]
        public string GcompanyRegistrationNo { get; set; }
        [StringLength(50)]
        public string VatRegistrationNo { get; set; }
        [Column("CompanyID")]
        public Guid? CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        [InverseProperty("GroupCompany")]
        public virtual Company Company { get; set; }
        [InverseProperty("GroupCompany")]
        public virtual ICollection<CompanyGroupCompany> CompanyGroupCompany { get; set; }
    }
}
