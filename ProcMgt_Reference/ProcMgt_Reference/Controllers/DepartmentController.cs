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
using System.Net;
using System.Net.Http;

namespace ProcMgt_Reference.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : BaseApiController 
    {
        private readonly IDepartmentServices _departmentServices;
        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentServices departmentService, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._departmentServices = departmentService;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetDepartmentGrid")]

        public async Task<DataGridTable> GetDepartmentGrid()
        {
            DataGridTable departmentLst = await _departmentServices.GetDepartmentGridAsync();
            return departmentLst;
          

        }

        [HttpGet, Route("GetAllDepartment")]
        public async Task<IEnumerable<DepartmentResource>> GetAllDepartment()
        {
            var department = await _departmentServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentResource>>(department);

            return resources;

        }

        [HttpPost, Route("GetSpecDepartment/{id}")]
        public async Task<ResultResource> GetSpecDepartment(string id, [FromBody]DepartmentResource resource)
        {
            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var department = _mapper.Map<DepartmentResource, Department>(resource);
            var result2 = await _departmentServices.GetAllSpecAsync(id , department);

            //if (!result2.Success)
            //{
            //    result.Message = "Password is Incorrect!";
            //    result.status = false;
            //    return result;
            //}

            var resources =  _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentResource>>(result2);
            result.ResultObject = resources;

            return result;
        }


        [HttpPost, Route("SaveDepartmentAsync")]

        public async Task<ResultResource> SaveDepartmentAsync([FromBody] DepartmentResource resource)
        {

            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var department = _mapper.Map<DepartmentResource, Department>(resource);
            var result2 = await _departmentServices.SaveDepartmentAsync(department);


            if (!result2.Success)
            {
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var departmentresource = _mapper.Map<Department, DepartmentResource>(result2.Obj);

            DataGridTable departmentLst = await _departmentServices.GetDepartmentGridAsync();
            result.ResultObject = departmentLst;
            return result;

        }

        [HttpPost, Route("UpdateDepartmentAsync/{id}")]
        public async Task<ResultResource> UpdateDepartmentAsync([FromBody] DepartmentResource resource)
        {
            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var Department = _mapper.Map<DepartmentResource, Department>(resource);
            var result2 = await _departmentServices.UpdateDepartmentAsync(Department);


            if (!result2.Success)
            {
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var departmentresource = _mapper.Map<Department, DepartmentResource>(result2.Obj);

            DataGridTable departmentLst = await _departmentServices.GetDepartmentGridAsync();

            result.ResultObject = departmentLst;
            return result;

        }

        [HttpPost, Route("DeleteDepartmentAsync/{id}")]


        public async Task<IActionResult> DeleteDepartmentAsync(string id, [FromBody] DepartmentResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var Department = _mapper.Map<DepartmentResource, Department>(resource);
            var result = await _departmentServices.DeleteDepartmentAsync(Department, id);


            if (!result.Success)
                return BadRequest(result.Message);

            var departmentresource = _mapper.Map<Department, DepartmentResource>(result.Obj);
            return Ok(departmentresource = _mapper.Map<Department, DepartmentResource>(result.Obj));

        }

    }
}
