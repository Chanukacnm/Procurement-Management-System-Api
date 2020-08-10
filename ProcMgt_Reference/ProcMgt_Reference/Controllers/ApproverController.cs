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
    public class ApproverController : BaseApiController
    {
        private readonly IApproverServices _approverServices;
        private readonly IMapper _mapper;

        public ApproverController(IApproverServices approverService, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._approverServices = approverService;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetAllApprover")]
        public async Task<IEnumerable<ApproverResource>> GetAllApprover()
        {
            var approver = await _approverServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<Approver>, IEnumerable<ApproverResource>>(approver);

            return resources;

        }

        [HttpPost, Route("SaveApproverAsync")]

        public async Task<IActionResult> SaveApproverAsync([FromBody] ApproverResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var approver = _mapper.Map<ApproverResource, Approver>(resource);
            var result = await _approverServices.SaveApproverAsync(approver);


            if (!result.Success)
                return BadRequest(result.Message);

            var approverresource = _mapper.Map<Approver, ApproverResource>(result.Obj);
            return Ok(approverresource);

        }

        [HttpPut, Route("UpdateApproverAsync/{id}")]

        public async Task<IActionResult> UpdateApproverAsync([FromBody] ApproverResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var Approver = _mapper.Map<ApproverResource, Approver>(resource);
            var result = await _approverServices.UpdateApproverAsync(Approver);


            if (!result.Success)
                return BadRequest(result.Message);

            var approverresource = _mapper.Map<Approver, ApproverResource>(result.Obj);
            return Ok(approverresource);

        }

        [HttpPost, Route("DeleteApproverAsync/{id}")]

        public async Task<IActionResult> DeleteApproverAsync(string id, [FromBody] ApproverResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var Approver = _mapper.Map<ApproverResource, Approver>(resource);
            var result = await _approverServices.DeleteApproverAsync(Approver, id);


            if (!result.Success)
                return BadRequest(result.Message);

            var approverresource = _mapper.Map<Approver, ApproverResource>(result.Obj);
            return Ok(approverresource = _mapper.Map<Approver, ApproverResource>(result.Obj));

        }
    }
}
