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
    public class ApprovalFlowManagementController : BaseApiController
    {
        private readonly IApprovalFlowManagementServices _approvalFlowManagementServices;
        private readonly IMapper _mapper;

        public ApprovalFlowManagementController(IApprovalFlowManagementServices approvalflowmanagementservice, IMapper mapper) // IGenericRepo<User> repo)
        {
            this._approvalFlowManagementServices = approvalflowmanagementservice;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetAllApprovalFlowManagement")]
        public async Task<IEnumerable<ApprovalFlowManagementResource>> GetAllApprovalFlowManagement()
        {
            var approvalflowmanagement = await _approvalFlowManagementServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<ApprovalFlowManagement>, IEnumerable<ApprovalFlowManagementResource>>(approvalflowmanagement);

            return resources;

        }

        [HttpGet, Route("GetApprovalFlowManagementGridAsync")]
        public async Task<DataGridTable> GetApprovalFlowManagementGridAsync()
        {
            DataGridTable approvalFlowManagementLst = await _approvalFlowManagementServices.GetApprovalFlowManagementGridAsync();
            return approvalFlowManagementLst;

        }

        [HttpPost, Route("SaveApprovalFlowManagementAsync")]
        public async Task<IActionResult> SaveApprovalFlowManagementAsync([FromBody] ApprovalFlowManagementResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var ApprovalFlowManagement = _mapper.Map<ApprovalFlowManagementResource, ApprovalFlowManagement>(resource);
            var result = await _approvalFlowManagementServices.SaveApprovalFlowManagementAsync(ApprovalFlowManagement);


            if (!result.Success)
                return BadRequest(result.Message);

            var approvalflowmanagementresource = _mapper.Map<ApprovalFlowManagement, ApprovalFlowManagementResource>(result.Obj);
            return Ok(approvalflowmanagementresource);

        }

        [HttpPost, Route("UpdateApprovalFlowManagementAsync/{id}")]
        public async Task<IActionResult> UpdateApprovalFlowManagementAsync(string id, [FromBody] ApprovalFlowManagementResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var ApprovalFlowManagement = _mapper.Map<ApprovalFlowManagementResource, ApprovalFlowManagement>(resource);
            var result = await _approvalFlowManagementServices.UpdateApprovalFlowManagementAsync(id, ApprovalFlowManagement);


            if (!result.Success)
                return BadRequest(result.Message);

            var approvalflowmanagementresource = _mapper.Map<ApprovalFlowManagement, ApprovalFlowManagementResource>(result.Obj);
            return Ok(approvalflowmanagementresource);

        }

        [HttpPost, Route("DeleteApprovalFlowManagementAsync/{id}")]
        public async Task<IActionResult> DeleteApprovalFlowManagementAsync(string id, [FromBody] ApprovalFlowManagementResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var ApprovalFlowManagement = _mapper.Map<ApprovalFlowManagementResource, ApprovalFlowManagement>(resource);
            var result = await _approvalFlowManagementServices.DeleteApprovalFlowManagementAsync(id, ApprovalFlowManagement);


            if (!result.Success)
                return BadRequest(result.Message);

            var approvalflowmanagementresource = _mapper.Map<ApprovalFlowManagement, ApprovalFlowManagementResource>(result.Obj);
            return Ok(approvalflowmanagementresource);

        }
    }
}
