using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
   public interface IItemServices
    {
        Task<IEnumerable<Item>> GetAllAsync();
        Task<IEnumerable<Item>> GetSpecItemAllAsync(string id, Item item);
        

        //Task<IEnumerable<Stock>> GetStockAllAsync();
        //Task<GenericSaveResponse<Item>> SaveItemAsync(Item item);
        Task<GenericSaveResponse<Item>> SaveItemAsync(Item item , Stock stock);
        //Task<GenericSaveResponse<Item>> UpdateItemAsync(string id, Item item);
        Task<GenericSaveResponse<Item>> UpdateItemAsync(string id, Item item , Stock stock);
        Task<GenericSaveResponse<Item>> DeleteItemAsync(Item item, string id);
        Task<DataGridTable> GetItemGridAsync();
    }
}
