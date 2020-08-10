using Microsoft.AspNetCore.Mvc;
using ProcMgt_Reference_Services;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference.Extensions;
using ProcMgt_Reference_Services.Interfaces;
using ProcMgt_Reference_Core.GenericRepoInter;

namespace ProcMgt_Reference.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RoleMenuController : ControllerBase
    {
        private readonly IRoleMenuServices _roleMenuServices;
        private readonly IMapper _mapper;


        public RoleMenuController(IRoleMenuServices roleMenuService, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._roleMenuServices  = roleMenuService;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetAllRoleMenu")]
        public async Task<IEnumerable<RoleMenuResource>> GetAllRoleMenu()
        {
            var rolemenu = await _roleMenuServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<RoleMenu>, IEnumerable<RoleMenuResource>>(rolemenu);

            return resources;

        }

        //[HttpPost, Route("UpdateMakeAsync/{id}")]

        //public async Task<IActionResult> UpdateMakeAsync(string id, [FromBody] MakeResource resource)
        //{

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState.GetErrorMessages());

        //    var Make = _mapper.Map<MakeResource, Make>(resource);
        //    var result = await _makeServices.UpdateMakeAsync(id, Make);


        //    if (!result.Success)
        //        return BadRequest(result.Message);

        //    var makeresource = _mapper.Map<Make, MakeResource>(result.Obj);
        //    return Ok(makeresource);

        //}


        [HttpPost, Route("GetMenuList/{id}")]
        public async Task<IEnumerable<int>> GetMenuList(string id)
        {
            try
            {
                var menuLst = await _roleMenuServices.GetRoleMenuAsync(id);
                return menuLst;

            }
            catch (Exception ex)
            {
                return (null);
            }
        }

        //[HttpPost, Route("SaveRoleMenuAsync")]
        //public async Task<IActionResult> SaveRoleMenuAsync([FromBody] RoleMenuAlterResource resource)
        //{

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState.GetErrorMessages());

        //    // var roleMenu = _mapper.Map<RoleMenuResource, RoleMenu>(resource);
        //    foreach(int MenuID in resource.MenuID)
        //    {

        //        RoleMenuResource obj = new RoleMenuResource();
        //        obj.MenuID = MenuID;
        //        obj.UserRoleID = resource.UserRoleID;

        //        var rolemenu = _mapper.Map<RoleMenuResource, RoleMenu>(obj);
        //        var result = await _roleMenuServices.SaveRoleMenuAsync(rolemenu);

                

        //        if (!result.Success)
        //            return BadRequest(result.Message);

        //        var rolemenuresource = _mapper.Map<RoleMenu, RoleMenuResource>(result.Obj);
        //    }
        //    return Ok();
        //}


        [HttpPost, Route("DeleteAndSave")]
        public async Task<IActionResult> Delete([FromBody] RoleMenuAlterResource resource)
        {
            


            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState.GetErrorMessages());

                // var roleMenu = _mapper.Map<RoleMenuResource, RoleMenu>(resource);

                RoleMenuResource roleMenuobj = new RoleMenuResource();

                //roleMenuobj.MenuID = MenuID;
                roleMenuobj.UserRoleID = resource.UserRoleID;

                var rolemenu = _mapper.Map<RoleMenuResource, RoleMenu>(roleMenuobj);
                var result = await _roleMenuServices.Delete(rolemenu);

                //foreach (int MenuID in resource.MenuID)
                //    {

                    
                //        //RoleMenuResource roleMenuobj = new RoleMenuResource();
                //        roleMenuobj.MenuID = MenuID;
                //        roleMenuobj.UserRoleID = resource.UserRoleID;

                //        var rolemenu = _mapper.Map<RoleMenuResource, RoleMenu>(roleMenuobj);
                //        var result = await _roleMenuServices.Delete(rolemenu);
                    
                  

                //        if (!result.Success)
                //            return BadRequest(result.Message);

                //        var rolemenuresource = _mapper.Map<RoleMenu, RoleMenuResource>(result.Obj);
                //    break;
                //    }


                foreach (int MenuID in resource.MenuID)
                {

                    //RoleMenuResource obj = new RoleMenuResource();
                    roleMenuobj.MenuID = MenuID;
                    roleMenuobj.UserRoleID = resource.UserRoleID;

                    var rolemenu1 = _mapper.Map<RoleMenuResource, RoleMenu>(roleMenuobj);
                    var result1 = await _roleMenuServices.SaveRoleMenuAsync(rolemenu1);

                    if (!result.Success)
                        return BadRequest(result.Message);

                    var rolemenuresource = _mapper.Map<RoleMenu, RoleMenuResource>(result.Obj);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
