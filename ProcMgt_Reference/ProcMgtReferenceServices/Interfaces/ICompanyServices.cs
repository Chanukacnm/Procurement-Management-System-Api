using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services
{
    public interface ICompanyServices
    {
        Task<IEnumerable<Company>> GetAllAsync();
        Task<GenericSaveResponse<Company>> SaveCompanyAsync(Company company);
        Task<GenericSaveResponse<Company>> UpdateCompanyAsync(string id, Company company);
        Task<GenericSaveResponse<Company>> UpdateCompanyGroupCompanyAsync(string id, Company company);

        Task<DataGridTable> GetCompanyGridAsync();
        Task<DataGridTable> getGroupCompanyGridAsync(string id, Company company);
    }
}
