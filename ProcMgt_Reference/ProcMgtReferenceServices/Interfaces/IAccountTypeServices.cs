using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface IAccountTypeServices
    {
        Task<IEnumerable<AccountType>> GetAllAsync();
        Task<GenericSaveResponse<AccountType>> SaveAccountTypeAsync(AccountType accounttype);
        Task<GenericSaveResponse<AccountType>> UpdateAccountTypeAsync(string id, AccountType accounttype);
    }
}
