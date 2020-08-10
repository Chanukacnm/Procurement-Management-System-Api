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
    public class BusinessUnitsController : BaseApiController
    {
        private readonly IBusinessUnitsService _businessUnitsServices;
        private readonly IMapper _mapper;

        public BusinessUnitsController(IBusinessUnitsService businessUnitsServices, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._businessUnitsServices = businessUnitsServices;
            this._mapper = mapper;
        }

        //[HttpGet, Route("GetAllBusinessUnits")]
        //public async Task<IEnumerable<BusinessUnitsResource>> GetAllBusinessUnits()
        //{


        //    var businessUnits = await _businessUnitsServices.GetAllBusinessUnitsAsync();
        //    var resources = _mapper.Map<IEnumerable<BusinessUnits>, IEnumerable<BusinessUnitsResource>>(businessUnits);

        //    return resources;

        //}

        [HttpPost, Route("GetAllBusinessUnits/{id}")]
        public async Task<ResultResource> GetAllBusinessUnits(string id, [FromBody]BusinessUnitsResource resource)
        {
            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }


            var businessUnits = _mapper.Map<BusinessUnitsResource, BusinessUnits>(resource);
            var result2 = await _businessUnitsServices.GetAllBusinessUnitsAsync(id, businessUnits);

            //if (!result2.Success)
            //{
            //    result.Message = "Password is Incorrect!";
            //    result.status = false;
            //    return result;
            //}

            var resources = _mapper.Map<IEnumerable<BusinessUnits>, IEnumerable<BusinessUnitsResource>>(result2);
            result.ResultObject = resources;

            return result;
        }

    }
}
