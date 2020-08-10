using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProcMgt_Reference.Extensions;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseApiController
    {
        private readonly IUserServices _userService;
        private readonly IMapper _mapper;

        public UserController(IUserServices userService, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._userService = userService;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetAllUser")]
        public async Task<IEnumerable<UserResource>> GetAllUser()
        {
            var user = await _userService.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<User>, IEnumerable<UserResource>>(user);

            return resources;

        }

        [HttpGet, Route("GetUserByID/{id}")]
        public async Task<IEnumerable<UserResource>> GetUserByID(int id)
        {
            var users = await _userService.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<User>, IEnumerable<UserResource>>(users);

            return resources;

        }

        [HttpGet, Route("GetUserGridAsync")]
        public async Task<DataGridTable> GetUserGridAsync()
        {
            DataGridTable userLst = await _userService.GetUserGridAsync();
            return userLst;

        }


        [HttpPost, Route("GetApprovalUserGridAsync/{id}")]
        public async Task<DataGridTable> GetApprovalUserGridAsync(string id, [FromBody] DesignationBusinessUnitResource resource)
        {
            var approvalUser = _mapper.Map<DesignationBusinessUnitResource, DesignationBusinessUnit>(resource);
            DataGridTable approvaluserLst = await _userService.GetApprovalUserGridAsync(id, approvalUser);
            return approvaluserLst;

        }




        [HttpPost, Route("SaveUserAsync")]

        public async Task<ResultResource> SaveUserAsync([FromBody] UserResource resource)
        {

            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }


            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMd5Hash(md5Hash, resource.Password);
                resource.Password = hash;
            }


            var User = _mapper.Map<UserResource, User>(resource);
            var result2 = await _userService.SaveUserAsync(User);


            if (!result2.Success)
            {
                //result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var userResource = _mapper.Map<User, UserResource>(result2.Obj);

            DataGridTable userLst = await _userService.GetUserGridAsync();

            result.ResultObject = userLst;
            result.Message = "User has been saved successfully";
            return result;

        }





        static string GetMd5Hash(MD5 md5Hash, string password)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        

        [HttpPost, Route("UpdateUserAsync/{id}")]
        public async Task<ResultResource> UpdateUserAsync(string id, [FromBody] UserResource resource)
        {

            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            //var checkpw = await _userService.UpdateUserAsync(id, User);
            if (resource.Password.Length < 20)
            {
                using (MD5 md5Hash = MD5.Create())
                {
                    string hash = GetMd5Hash(md5Hash, resource.Password);
                    resource.Password = hash;
                }
            }
            

            var User = _mapper.Map<UserResource, User>(resource);
            var result2 = await _userService.UpdateUserAsync(id, User);


            if (!result2.Success)
            {
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var userResource = _mapper.Map<User, UserResource>(result2.Obj);
            DataGridTable userLst = await _userService.GetUserGridAsync();

            result.ResultObject = userLst;
            result.Message = "User has been Update successfully";
            return result;

        }

        [HttpPost, Route("ChangePWAsync/{id}")]
        public async Task<ResultResource> ChangePWAsync(string id, [FromBody] ChangePwResource resource)
        {
            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMd5Hash(md5Hash, resource.CurrentPw);
                resource.CurrentPw = hash;

                string hash2 = GetMd5Hash(md5Hash, resource.Password);
                resource.Password = hash2;
            }
                       
             var PW = _mapper.Map<ChangePwResource, User>(resource);
             var result2 = await _userService.ChangePWAsync(id, PW, resource.CurrentPw);

             if (!result2.Success)
             {
                 result.Message = "Password is Incorrect!";
                 result.status = false;
                 return result;
             }

             var changepwResource = _mapper.Map<User, ChangePwResource>(result2.Obj);
             result.ResultObject = changepwResource;
             result.Message = "Your Password has been changed Successfully!";


             return result;
            

            
        }

        [HttpPost, Route("DeleteUserAsync/{id}")]
        public async Task<IActionResult> DeleteUserAsync(string id, [FromBody] UserResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var User = _mapper.Map<UserResource, User>(resource);
            var result = await _userService.DeleteUserAsync(id, User);


            if (!result.Success)
                return BadRequest(result.Message);

            var userResource = _mapper.Map<User, UserResource>(result.Obj);
            return Ok(userResource);

        }
    }
}
