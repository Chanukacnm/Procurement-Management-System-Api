using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
   public interface IItemRequestServices
    {
        Task<IEnumerable<ItemRequest>> GetAllAsync();
        Task<GenericSaveResponse<ItemRequest>> SaveItemRequestAsync(ItemRequest itemrequest);
        Task<GenericSaveResponse<ItemRequest>> UpdateItemRequestAsync(string id, ItemRequest itemrequest, double ReceivedQty, double BalancedQty);
        Task<GenericSaveResponse<ItemRequest>> DeleteItemRequestAsync(string id, ItemRequest itemrequest);

        Task<DataGridTable> GetItemRequestGridAsync(ItemRequestResource itemrequest);
    }
}
