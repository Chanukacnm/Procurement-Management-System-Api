using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
   public interface IApproverServices
    {
        Task<IEnumerable<Approver>> GetAllAsync();
        Task<GenericSaveResponse<Approver>> SaveApproverAsync(Approver approver);
        Task<GenericSaveResponse<Approver>> UpdateApproverAsync(Approver approver);
        Task<GenericSaveResponse<Approver>> DeleteApproverAsync(Approver approver, string id);
       
    }
}
