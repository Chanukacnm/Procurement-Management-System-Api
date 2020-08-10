using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface IBankBranchServices
    {
        Task<IEnumerable<BankBranch>> GetAllAsync();
        Task<GenericSaveResponse<BankBranch>> SaveBankBranchAsync(BankBranch bankbranch);
        Task<GenericSaveResponse<BankBranch>> UpdateBankBranchAsync(string id, BankBranch bankbranch);
    }
}
