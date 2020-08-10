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
    public class ApprovalPatternTypeController : BaseApiController
    {
        private readonly IApprovalPatternTypeServices _approvalPatternTypeServices;
        private readonly IMapper _mapper;


        public ApprovalPatternTypeController(IApprovalPatternTypeServices approvalpatterntypeservice, IMapper mapper) // IGenericRepo<User> repo)
        {
            this._approvalPatternTypeServices = approvalpatterntypeservice;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetAllApprovalPatternType")]
        public async Task<IEnumerable<ApprovalPatternTypeResource>> GetAllApprovalPatternType()
        {
            var approvalpatterntype = await _approvalPatternTypeServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<ApprovalPatternType>, IEnumerable<ApprovalPatternTypeResource>>(approvalpatterntype);

            return resources;

        }

        [HttpGet, Route("GetApprovalPatternTypeGrid")]
        public async Task<DataGridTable> GetApprovalPatternTypeGrid()
        {
            DataGridTable approvalPatternTypeLst = await _approvalPatternTypeServices.GetApprovalPatternTypeGridAsync();
            return approvalPatternTypeLst;


        }

        [HttpPost, Route("SaveApprovalPatternTypeAsync")]
        public async Task<ResultResource> SaveApprovalPatternTypeAsync([FromBody] ApprovalPatternTypeResource resource)
        {

            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var ApprovalPatternType = _mapper.Map<ApprovalPatternTypeResource, ApprovalPatternType>(resource);
            var result2 = await _approvalPatternTypeServices.SaveApprovalPatternTypeAsync(ApprovalPatternType);


            if (!result2.Success)
            {
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var approvalpatterntyperesource = _mapper.Map<ApprovalPatternType, ApprovalPatternTypeResource>(result2.Obj);

            DataGridTable approvalPatternTypeLst = await _approvalPatternTypeServices.GetApprovalPatternTypeGridAsync();

            result.ResultObject = approvalPatternTypeLst;
            return result;

        }

        [HttpPost, Route("UpdateApprovalPatternTypeAsync/{id}")]
        public async Task<ResultResource> UpdateApprovalPatternTypeAsync(string id, [FromBody] ApprovalPatternTypeResource resource)
        {

            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var ApprovalPatternType = _mapper.Map<ApprovalPatternTypeResource, ApprovalPatternType>(resource);
            var result2 = await _approvalPatternTypeServices.UpdateApprovalPatternTypeAsync(id,ApprovalPatternType);


            if (!result2.Success)
            {
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var approvalpatterntyperesource = _mapper.Map<ApprovalPatternType, ApprovalPatternTypeResource>(result2.Obj);

            DataGridTable approvalPatternTypeLst = await _approvalPatternTypeServices.GetApprovalPatternTypeGridAsync();

            result.ResultObject = approvalPatternTypeLst;
            return result;

        }


        [HttpPost, Route("DeleteApprovalPatternTypeAsync/{id}")]
        public async Task<IActionResult> DeleteApprovalPatternTypeAsync(string id, [FromBody] ApprovalPatternTypeResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var ApprovalPatternType = _mapper.Map<ApprovalPatternTypeResource, ApprovalPatternType>(resource);
            var result = await _approvalPatternTypeServices.DeleteApprovalPatternTypeAsync(id, ApprovalPatternType);


            if (!result.Success)
                return BadRequest(result.Message);

            var approvalpatterntyperesource = _mapper.Map<ApprovalPatternType, ApprovalPatternTypeResource>(result.Obj);
            return Ok(approvalpatterntyperesource);

        }

    }
}
