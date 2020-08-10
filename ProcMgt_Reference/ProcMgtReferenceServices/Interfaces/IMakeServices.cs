using System;
using System.Collections.Generic;
using System.Text;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Services.Communication;
using System.Threading.Tasks;
using ProcMgt_Reference_Core.Resources;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface IMakeServices
    {
        Task<IEnumerable<Make>> GetAllAsync(); 
        Task<IEnumerable<Make>> GetSpecMakeAllAsync(string id, Make make);
        Task<IEnumerable<Make>> GetSpecMakeListAllAsync(string id, Make make);

        Task<IEnumerable<MakeResource>> GetSpecMakeListAsync(string id, Item item);

        Task<GenericSaveResponse<Make>> SaveMakeAsync(Make make);
        Task<GenericSaveResponse<Make>> UpdateMakeAsync(string id, Make make);
        Task<GenericSaveResponse<Make>> DeleteMakeAsync(Make make, string id);
        Task<DataGridTable> GetMakeGridAsync();
    }
}
