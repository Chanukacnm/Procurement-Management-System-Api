using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface ISupplierServices
    {
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task<GenericSaveResponse<Supplier>> SaveSupplierAsync(Supplier supplier);
        Task<GenericSaveResponse<Supplier>> UpdateSupplierAsync(string id, Supplier supplier);
        Task<GenericSaveResponse<Supplier>> DeleteSupplierAsync(string id, Supplier supplier);

        Task<DataGridTable> GetSupplierGridAsync();
        Task<DataGridTable> GetContactDetailsGridAsync(ContactDetails contactDetails);
    }
}
