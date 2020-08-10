using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface IApprovalPatternTypeServices
    {
        Task<IEnumerable<ApprovalPatternType>> GetAllAsync();
        Task<GenericSaveResponse<ApprovalPatternType>> SaveApprovalPatternTypeAsync(ApprovalPatternType approvalpatterntype);
        Task<GenericSaveResponse<ApprovalPatternType>> UpdateApprovalPatternTypeAsync(string id, ApprovalPatternType approvalpatterntype);
        Task<GenericSaveResponse<ApprovalPatternType>> DeleteApprovalPatternTypeAsync(string id, ApprovalPatternType approvalpatterntype);

        Task<DataGridTable> GetApprovalPatternTypeGridAsync();
    }
}
