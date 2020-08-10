using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface ITaxServices
    {
        Task<IEnumerable<Tax>> GetAllAsync();
        Task<GenericSaveResponse<Tax>> SaveTaxAsync(Tax tax);
        Task<GenericSaveResponse<Tax>> UpdateTaxAsync(string id, Tax tax);

        Task<GenericSaveResponse<Tax>> DeleteTaxAsync( string id, Tax tax);

        Task<DataGridTable> GetTaxGridAsync();
    }

}
