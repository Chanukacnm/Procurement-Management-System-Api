using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface IDepartmentServices
    {
        Task<IEnumerable<Department>> GetAllAsync();
        Task<IEnumerable<Department>> GetAllSpecAsync(string id, Department department);

        Task<GenericSaveResponse<Department>> SaveDepartmentAsync(Department department);
        Task<GenericSaveResponse<Department>> UpdateDepartmentAsync(Department department);
        Task<GenericSaveResponse<Department>> DeleteDepartmentAsync(Department department, string id);
        Task<DataGridTable> GetDepartmentGridAsync();
    }
}
