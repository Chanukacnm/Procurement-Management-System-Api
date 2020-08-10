using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface IUserRoleServices
    {
        Task<IEnumerable<UserRole>> GetAllAsync();
        Task<GenericSaveResponse<UserRole>> SaveUserRoleAsync(UserRole userrole);
        Task<GenericSaveResponse<UserRole>> UpdateUserRoleAsync(string id, UserRole userrole);
        Task<GenericSaveResponse<UserRole>> DeleteUserRoleAsync(string id, UserRole userrole);

        Task<DataGridTable> GetUserRoleGridAsync();
    }
}
