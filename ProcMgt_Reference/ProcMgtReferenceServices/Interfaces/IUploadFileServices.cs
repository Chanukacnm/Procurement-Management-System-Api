using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface IUploadFileServices
    {
        Task<IEnumerable<UploadFile>> GetAllAsync();
        Task<GenericSaveResponse<UploadFile>> SaveUploadFileAsync(UploadFile uploadFile);
        Task<GenericSaveResponse<UploadFile>> UpdateUploadFileAsync(string id, UploadFile uploadFile);

        Task<IEnumerable<UploadFile>> GetImageAsync(string id, UploadFile uploadFile);
    }
}
