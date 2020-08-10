using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
  public  interface IRoleMenuServices
    {
        Task<IEnumerable<RoleMenu>> GetAllAsync();
        Task<GenericSaveResponse<RoleMenu>> SaveRoleMenuAsync(RoleMenu rolemenu);
        Task<IEnumerable<int>> GetRoleMenuAsync(string id);

        Task<GenericSaveResponse<RoleMenu>> Delete(RoleMenu rolemenu);

    }
}
