using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface IUserServices
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<GenericSaveResponse<User>> SaveUserAsync(User user);
        Task<GenericSaveResponse<User>> UpdateUserAsync(string id, User user);
        Task<GenericSaveResponse<User>> DeleteUserAsync(string id, User user);

        Task<GenericSaveResponse<User>> ChangePWAsync(string id, User user, string CurrentPw);

        Task<DataGridTable> GetUserGridAsync();

        Task<DataGridTable> GetApprovalUserGridAsync(string id, DesignationBusinessUnit designationBusinessUnit);
    }
}
