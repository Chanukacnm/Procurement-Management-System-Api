using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProcMgt_Reference.Extensions;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcMgt_Reference.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignationController : BaseApiController
    {
        private readonly IDesignationServices _designationServices;
        private readonly IMapper _mapper;

        public DesignationController(IDesignationServices designationService, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._designationServices = designationService;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetAllDesignation")]
        public async Task<IEnumerable<DesignationResource>> GetAllDesignation()
        {


            var designation = await _designationServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<Designation>, IEnumerable<DesignationResource>>(designation);

            return resources;

        }

        [HttpGet, Route("GetDesignationGrid")]
        public async Task<DataGridTable> GetDesignationGrid()
        {
            DataGridTable categoryLst = await _designationServices.GetDesignationGridAsync();
            return categoryLst;

        }

        [HttpPost, Route("SaveDesignationAsync")]
        public async Task<ResultResource> SaveDesignationAsync([FromBody] DesignationResource resource)
        {
            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }


            var designation = _mapper.Map<DesignationResource, Designation>(resource);
            var result2 = await _designationServices.SaveDesignationAsync(designation);

            if (!result2.Success)
            {
                // result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var designationresource = _mapper.Map<Designation, DesignationResource>(result2.Obj);

            DataGridTable designationLst = await _designationServices.GetDesignationGridAsync();

            result.ResultObject = designationLst;
            return result;

        }


        [HttpPost, Route("UpdateDesignationAsync/{id}")]
        public async Task<ResultResource> UpdateDesignationAsync(string id, [FromBody] DesignationResource resource)
        {
            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }


            var designation = _mapper.Map<DesignationResource, Designation>(resource);
            var result2 = await _designationServices.UpdateDesignationAsync(id , designation );

            if (!result2.Success)
            {
                // result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var result3 = await _designationServices.UpdateBusinessUnitTypeAsync(id, designation);

            if (!result3.Success)
            {
                // result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.Message = result3.Message;
                result.status = false;
                return result;
            }

            var designationresource = _mapper.Map<Designation, DesignationResource>(result2.Obj);

            DataGridTable designationLst = await _designationServices.GetDesignationGridAsync();

            result.ResultObject = designationLst;
            return result;

        }
    }
}
