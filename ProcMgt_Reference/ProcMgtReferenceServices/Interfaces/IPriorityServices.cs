using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface IPriorityServices
    {
        Task<IEnumerable<Priority>> GetAllAsync();
        Task<GenericSaveResponse<Priority>> SavePriorityAsync(Priority priority);
        Task<GenericSaveResponse<Priority>> UpdatePriorityAsync(string id, Priority priority);
    }
}
