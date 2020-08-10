using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Web;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference.Extensions;
using ProcMgt_Reference_Services.Interfaces;
using AutoMapper;
using ProcMgt_Reference_Core.Models;

namespace ProcMgt_Reference.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : BaseApiController
    {
        private IConfiguration _configuration;
        private readonly IUploadFileServices _uploadFileServices;
        private readonly IMapper _mapper;

        public FileUploadController(IUploadFileServices uploadFileServices, IConfiguration Configuration , IMapper mapper)
        {
            this._uploadFileServices = uploadFileServices;
            this._mapper = mapper;
            _configuration = Configuration;
        }

        [HttpPost, Route("UpadteFileUpload/{id}")]
        public async Task<ResultResource> UpadteFileUpload(string id, [FromBody] UploadFileResource resource)
        {
            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var uploadFile = _mapper.Map<UploadFileResource, UploadFile>(resource);
            var result2 = await _uploadFileServices.UpdateUploadFileAsync(id, uploadFile);

            if (!result2.Success)
            {
                result.Message = result2.Message;
                result.status = false;
                return result;
            }
            var fileUploadResource  = _mapper.Map<UploadFile, UploadFileResource>(result2.Obj);

            result.ResultObject = fileUploadResource;
            return result;

        }

        [HttpPost, Route("getuploadImage/{id}")]
        public async Task<ResultResource> getuploadImage(string id, [FromBody]UploadFileResource resource)
        {
            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var uploadFile = _mapper.Map<UploadFileResource, UploadFile>(resource);
            var result2 = await _uploadFileServices.GetImageAsync(id, uploadFile);

            var uploadresource = _mapper.Map<IEnumerable<UploadFile>, IEnumerable<UploadFileResource>>(result2);
            var objimg = uploadresource.FirstOrDefault();
            result.ResultObject = objimg;

            return result;
        }

        [HttpPost, Route("Upload")]
        public async Task<ResultResource> Upload()
        {
            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }
             

            var storageAccountName = _configuration["AzureConfigKeys:AzureStorageName"];
            var storageAccountKey = _configuration["AzureConfigKeys:AzureStorageKey"];

            
            try
            {
                var storageContainer = _configuration["AzureConfigKeys:ProkuraFileUpload"];

                StorageCredentials storageCred = new StorageCredentials(storageAccountName, storageAccountKey);
                CloudStorageAccount storageAcc = new CloudStorageAccount(storageCred, true);
                CloudBlobClient blobClient = storageAcc.CreateCloudBlobClient();
                CloudBlobContainer container_ = blobClient.GetContainerReference(storageContainer);
                await container_.CreateIfNotExistsAsync();

                BlobContainerPermissions containerPermission = await container_.GetPermissionsAsync();
                containerPermission.PublicAccess = BlobContainerPublicAccessType.Container;
                await container_.SetPermissionsAsync(containerPermission);

                var SelectedFiles = Request.Form.Files[0];
                string FullFileName = SelectedFiles.FileName;
                string Extention = Path.GetExtension(FullFileName);
                var contentType = SelectedFiles.ContentType;


                var UniqueFileName = "-"+ DateTime.Now.ToString("yyyyMMddHHmmss");
                var folderName = Path.Combine("Uploaded", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                

                if (SelectedFiles.Length > 0)
                {
                    //var fileName = ContentDispositionHeaderValue.Parse(SelectedFiles.ContentDisposition).FileName.Trim('"');
                    var fileName = Path.GetFileNameWithoutExtension(FullFileName);
                    string UniqFileName = string.Concat(string.Concat(fileName,UniqueFileName), Extention);
                    string FileNewName = UniqFileName.Replace(" ", "_");
                    var fullPath = Path.Combine(pathToSave, FileNewName);
                    var dbPath = Path.Combine(folderName, FileNewName);


                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        SelectedFiles.CopyTo(stream);
                    }

                    FileInfo _file = new FileInfo(fullPath);
                    if (!_file.Exists)//check file exsit or not
                    {
                        _file.Directory.Create();
                    }

                    CloudBlockBlob blockBlob = container_.GetBlockBlobReference(FileNewName);
                    using (var fileStream = System.IO.File.OpenRead(dbPath))
                    {
                        blockBlob.Properties.ContentType = contentType;
                        await blockBlob.UploadFromStreamAsync(fileStream);
                    }

                    FileInfo tempUploaded = new FileInfo(fullPath);
                    if (tempUploaded.Exists)
                    {
                        tempUploaded.Delete();
                    }

                    var UploadedUrl = Convert.ToString(blockBlob.SnapshotQualifiedUri.AbsoluteUri);

                    UploadFileResource upload = new UploadFileResource();
                    
                    upload.UploadFileName = FileNewName;
                    upload.UploadFilePath = UploadedUrl;
                    upload.FileExtension = Extention;
                    upload.DateTime = DateTime.Now;

                    var uploadFile = _mapper.Map<UploadFileResource, UploadFile>(upload);
                    var result2 = await _uploadFileServices.SaveUploadFileAsync(uploadFile);

                    if(!result2.Success)
                    {
                        result.Message = result2.Message;
                        result.status = false;
                        return result;
                    }

                    var uploadfileresource = _mapper.Map<UploadFile, UploadFileResource>(result2.Obj);

                    result.ResultObject = uploadfileresource;
                }

                
                result.Message = "Upload has been Saved Successfully!";
                


                return result;
                
            }
            catch (Exception ex)
            {
                result.Message = (ex.Message ?? ex.InnerException.Message);
                result.status = false;
                //return StatusCode(500, "Internal server error");
                return result;
            }
        }
    }
}
