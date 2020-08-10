using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface IBankServices
    {
        Task<IEnumerable<Bank>> GetAllAsync();
        Task<GenericSaveResponse<Bank>> SaveBankAsync(Bank bank);
        Task<GenericSaveResponse<Bank>> UpdateBankAsync(string id, Bank bank);
    }
}
