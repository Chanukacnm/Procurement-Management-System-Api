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
    public class BusinessUnitTypeController : BaseApiController
    {
        private readonly IBusinessUnitTypeService _businessUnitTypeServices;
        private readonly IMapper _mapper;

        public BusinessUnitTypeController(IBusinessUnitTypeService businessUnitTypeServices, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._businessUnitTypeServices = businessUnitTypeServices;
            this._mapper = mapper;
        }

        //[HttpGet, Route("GetAllDesignationLevel")]
        //public async Task<IEnumerable<BusinessUnitTypeResource>> GetAllDesignationLevel()
        //{


        //    var designationlevel = await _businessUnitTypeServices.GetAllBusinessUnitTypeAsync();
        //    var resources = _mapper.Map<IEnumerable<BusinessUnitType>, IEnumerable<BusinessUnitTypeResource>>(designationlevel);

        //    return resources;

        //}

        [HttpPost, Route("GetAllDesignationLevel/{id}")]
        public async Task<ResultResource> GetAllDesignationLevel(string id, [FromBody]BusinessUnitTypeResource resource)
        {
            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }


            var designationlevel = _mapper.Map<BusinessUnitTypeResource, BusinessUnitType>(resource);
            var result2 = await _businessUnitTypeServices.GetAllBusinessUnitTypeAsync(id, designationlevel);

            //if (!result2.Success)
            //{
            //    result.Message = "Password is Incorrect!";
            //    result.status = false;
            //    return result;
            //}

            var resources = _mapper.Map<IEnumerable<BusinessUnitType>, IEnumerable<BusinessUnitTypeResource>>(result2);
            result.ResultObject = resources;

            return result;
        }
    }
}
