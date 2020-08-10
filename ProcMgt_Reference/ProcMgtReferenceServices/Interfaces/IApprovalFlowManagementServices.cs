using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface IApprovalFlowManagementServices
    {
        Task<IEnumerable<ApprovalFlowManagement>> GetAllAsync();
        Task<GenericSaveResponse<ApprovalFlowManagement>> SaveApprovalFlowManagementAsync(ApprovalFlowManagement approvalflowmanagement);
        Task<GenericSaveResponse<ApprovalFlowManagement>> UpdateApprovalFlowManagementAsync(string id, ApprovalFlowManagement approvalflowmanagement);
        Task<GenericSaveResponse<ApprovalFlowManagement>> DeleteApprovalFlowManagementAsync(string id, ApprovalFlowManagement approvalflowmanagement);

        Task<DataGridTable> GetApprovalFlowManagementGridAsync();
    }
}
