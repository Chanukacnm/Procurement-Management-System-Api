using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class CompanyGroupCompany
    {
        [Column("CompanyGroupCompanyID")]
        public Guid CompanyGroupCompanyId { get; set; }
        [Column("CompanyID")]
        public Guid CompanyId { get; set; }
        [Column("GroupCompanyID")]
        public Guid GroupCompanyId { get; set; }

        [ForeignKey("CompanyId")]
        [InverseProperty("CompanyGroupCompany")]
        public virtual Company Company { get; set; }
        [ForeignKey("GroupCompanyId")]
        [InverseProperty("CompanyGroupCompany")]
        public virtual GroupCompany GroupCompany { get; set; }
    }
}
