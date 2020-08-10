using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface IMinimumCapacityServices
    {
        Task<IEnumerable<MinimumCapacity>> GetAllAsync();
        Task<GenericSaveResponse<MinimumCapacity>> SaveMinimumCapacityAsync(MinimumCapacity minimumcapacity);
        Task<GenericSaveResponse<MinimumCapacity>> UpdateMinimumCapacityAsync(string id, MinimumCapacity minimumcapacity);
    }
}
