using ProcMgt_Reference_Core;
using ProcMgt_Reference_Core.GenericRepoInter;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Communication;
using ProcMgt_Reference_Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Implementations
{
    public class UploadFileService : IUploadFileServices
    {
        private IGenericRepo<UploadFile> _repository = null;
        private IUnitOfWorks _unitOfWork;

        public UploadFileService(IGenericRepo<UploadFile> repository,  IUnitOfWorks unitfwork)
        {
            this._repository = repository;
            this._unitOfWork = unitfwork;
        }

        public async Task<IEnumerable<UploadFile>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<IEnumerable<UploadFile>> GetImageAsync(string id, UploadFile uploadFile)
        {
            var uploadimage = (await _repository.GetAll()).Select(c => new UploadFile
            {
                UploadFileId = c.UploadFileId,
                UploadFileName = c.UploadFileName,
                UploadFilePath = c.UploadFilePath,
                IsDeleted = c.IsDeleted,
                FileExtension = c.FileExtension,
                DateTime = c.DateTime
            }).Where(d => d.UploadFileId == uploadFile.UploadFileId && d.IsDeleted == false).ToList();
            return uploadimage;
        }

        public async Task<GenericSaveResponse<UploadFile>> SaveUploadFileAsync(UploadFile uploadFile)
        {
            try
            {
                if (uploadFile.UploadFileId == Guid.Empty)
                {
                    uploadFile.UploadFileId = Guid.NewGuid();
                }

                await _repository.InsertAsync(uploadFile);
                await _unitOfWork.CompleteAsync();

                 
                return new GenericSaveResponse<UploadFile>(true, "Successfully Saved.", uploadFile);
            }
            catch(Exception ex)
            {
                return new GenericSaveResponse<UploadFile>($"An error occured when saving the Upload File :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<UploadFile>> UpdateUploadFileAsync(string id,UploadFile uploadFile)
        {
            try
            {
                UploadFile existingUploadFile = await _repository.GetByIdAsync(uploadFile.UploadFileId);
                if (existingUploadFile == null)
                    return new GenericSaveResponse<UploadFile>($"Upload File not found");

                existingUploadFile.IsDeleted = true;

                _repository.Update(existingUploadFile);
                await _unitOfWork.CompleteAsync();
                return new GenericSaveResponse<UploadFile>(true, "Successfully Saved.", existingUploadFile);
            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<UploadFile>($"An error occured when saving the Upload File :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

    }
}
