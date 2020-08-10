using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface IDesignationServices
    {
        Task<IEnumerable<Designation>> GetAllAsync();
        Task<GenericSaveResponse<Designation>> SaveDesignationAsync(Designation designation );
        Task<GenericSaveResponse<Designation>> UpdateDesignationAsync(string id, Designation designation );
        Task<GenericSaveResponse<Designation>> UpdateBusinessUnitTypeAsync(string id, Designation designation);
        Task<DataGridTable> GetDesignationGridAsync();
    }
}
