using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface ICategoryMasterServices
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<GenericSaveResponse<Category>> SaveCategoryMasterAsync(Category categorymaster);
        Task<GenericSaveResponse<Category>> UpdateCategoryMasterAsync(string id, Category categorymaster);
        Task<GenericSaveResponse<Category>> DeleteCategoryMasterAsync(string id, Category categorymaster);

        Task<DataGridTable> GetCategoryGridAsync();
    }
}
