using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProcMgt_Reference.Extensions;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProcMgt_Reference_Services.Interfaces;

namespace ProcMgt_Reference.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : BaseApiController
    {
       
        private readonly IUserRoleServices _userRoleService;
        private readonly IMapper _mapper;


        public UserRoleController(IUserRoleServices userroleservice, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._userRoleService = userroleservice;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetUserRoleByID/{id}")]

        public async Task<IEnumerable<UserRoleResource>> GetUserRoleByID(int id)
        {
            var userrole = await _userRoleService.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<UserRole>, IEnumerable<UserRoleResource>>(userrole);

            return resources;

        }

        [HttpGet, Route("GetAllUserRole")]

        public async Task<IEnumerable<UserRoleResource>> GetAllUserRole()
        {
            try
            {
                var userrole = await _userRoleService.GetAllAsync();
                var resources = _mapper.Map<IEnumerable<UserRole>, IEnumerable<UserRoleResource>>(userrole);

                return resources;
            }
            catch(Exception ex) {

                return null;
            }
           

        }

        [HttpGet, Route("GetUserRoleGrid")]
        public async Task<DataGridTable> GetUserRoleGrid()
        {
            DataGridTable userRoleLst = await _userRoleService.GetUserRoleGridAsync();
            return userRoleLst;

        }


        [HttpPost, Route("SaveUserRoleAsync")]
        public async Task<ResultResource> SaveUserRoleAsync([FromBody] UserRoleResource resource)
        {

            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var UserRole = _mapper.Map<UserRoleResource, UserRole>(resource);
            var result2 = await _userRoleService.SaveUserRoleAsync(UserRole);


            if (!result2.Success)
            {
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var measurementunitresource = _mapper.Map<UserRole, UserRoleResource>(result2.Obj);

            DataGridTable userRoleLst = await _userRoleService.GetUserRoleGridAsync();

            result.ResultObject = userRoleLst;
            return result;

        }

        [HttpPost, Route("UpdateUserRoleAsync/{id}")]
        public async Task<ResultResource> UpdateUserRoleAsync(string id, [FromBody] UserRoleResource resource)
        {

            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var UserRole = _mapper.Map<UserRoleResource, UserRole>(resource);
            var result2 = await _userRoleService.UpdateUserRoleAsync(id, UserRole);


            if (!result2.Success)
            {
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var userroleresource = _mapper.Map<UserRole, UserRoleResource>(result2.Obj);

            DataGridTable userRoleLst = await _userRoleService.GetUserRoleGridAsync();

            result.ResultObject = userRoleLst;
            return result;

        }

        [HttpPost, Route("DeleteUserRoleAsync/{id}")]

        public async Task<IActionResult> DeleteUserRoleAsync(string id, [FromBody] UserRoleResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var UserRole = _mapper.Map<UserRoleResource, UserRole>(resource);
            var result = await _userRoleService.DeleteUserRoleAsync(id, UserRole);


            if (!result.Success)
                return BadRequest(result.Message);

            var userroleresource = _mapper.Map<UserRole, UserRoleResource>(result.Obj);
            return Ok(userroleresource);

        }

    }
}
