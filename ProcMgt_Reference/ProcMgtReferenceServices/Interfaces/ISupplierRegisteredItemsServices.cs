using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface ISupplierRegisteredItemsServices
    {
        Task<IEnumerable<SupplierRegisteredItems>> GetAllAsync();
       Task<IEnumerable<ItemResource>> GetItemsDescription(string id, SupplierRegisteredItems supplierregistereditem);

        Task<DataGridTable> getSupRegItemsList(string id, SupplierRegisteredItems supplierRegisteredItems);
        Task<GenericSaveResponse<SupplierRegisteredItems>> SaveSupplierRegisteredItemsAsync(SupplierRegisteredItems supplierRegisteredItems);
        Task<GenericSaveResponse<SupplierRegisteredItems>> UpdateSupplierRegisteredItemsAsync(string id, SupplierRegisteredItems supplierRegisteredItems);
        //Task<DataGridTable> GetSupplierRegisteredItemsGrid();

    }
}
