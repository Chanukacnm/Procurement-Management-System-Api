using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface IItemTypeMasterServices
    {
        Task<IEnumerable<ItemType>> GetAllAsync();
        Task<IEnumerable<ItemType>> GetSpecItemTypeAllAsync(string id, ItemType itemtypemaster);


        Task<GenericSaveResponse<ItemType>> SaveItemTypeMasterAsync(ItemType itemtypemaster);
        Task<GenericSaveResponse<ItemType>> UpdateItemTypeMasterAsync(string id, ItemType itemtypemaster);
        Task<GenericSaveResponse<ItemType>> DeleteItemTypeMasterAsync(ItemType itemtypemaster, string id);
        Task<DataGridTable> GetItemTypeMasterGridAsync();
    }
}
