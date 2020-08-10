using ProcMgt_Reference_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ProcMgt_Reference_Core.Resources;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface IAuthServices
    {
        Task<IEnumerable<User>> GetAllAsync();
        //Task<AuthenticationResult> LoginAsync(string username, string password);
        Task<AuthenticatorResource> Authenticate(string username, string password);
        Task<IEnumerable<User>> GetAll();
    }
}
