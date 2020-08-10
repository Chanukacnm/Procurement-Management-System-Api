using System;
using System.Collections.Generic;
using System.Text;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Services.Communication;
using System.Threading.Tasks;
using ProcMgt_Reference_Core.Resources;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface IModelServices
    {
        Task<IEnumerable<Model>> GetAllAsync();
        Task<IEnumerable<Model>> GetSpecModelAllAsync(string id, Model model);

        Task<GenericSaveResponse<Model>> SaveModelAsync(Model model);
        Task<GenericSaveResponse<Model>> UpdateModelAsync(string id, Model model);
        Task<GenericSaveResponse<Model>> DeleteModelAsync(Model model, string id);
        Task<DataGridTable> GetModelGridAsync();
    }
}
