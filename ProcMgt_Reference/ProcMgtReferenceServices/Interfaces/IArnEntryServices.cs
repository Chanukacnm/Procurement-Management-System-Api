using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
   public interface IArnEntryServices
    {
        
        Task<Arnheader> GetAllArnheaderAsync(Arnheader arnheader);
        Task<IEnumerable<Arndetail>> GetArndetailAsync();
        Task<GenericSaveResponse<Arnheader>> SaveArnheaderAsync(Arnheader arnheader);
        Task<GenericSaveResponse<Arnheader>> UpdateArnheaderAsync(string id, Arnheader arnheader);
        Task<GenericSaveResponse<Arnheader>> DeleteArnheaderAsync(string id, Arnheader arnheader);

        Task<DataGridTable> GetArndetailGridAsync(Arnheader arnheader);
       
        Task<DataGridTable> GetPOGrListGridAsync();

    }
}
