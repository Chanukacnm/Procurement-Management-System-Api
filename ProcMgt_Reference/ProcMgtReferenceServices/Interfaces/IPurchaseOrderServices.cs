using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
   public interface IPurchaseOrderServices  
   {
        Task<IEnumerable<Poheader>> GetAllPoHeaderAsync(); 

        Task<IEnumerable<PoHeaderResource>> GetAllPoReportDetailsAsync(string id, Poheader poheader); 

        Task<IEnumerable<Podetail>> GetAllPoDetailsAsync();

        Task<GenericSaveResponse<Poheader>> SavePurchaseOrderAsync(Poheader poheader);
       
        Task<DataGridTable> GetQuotationListGridAsync();

        Task<DataGridTable> GetQuotatioDetailsListGridAsync(Poheader poheader);




    }
}
  