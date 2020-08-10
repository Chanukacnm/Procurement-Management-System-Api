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
    public class IssueController : ControllerBase
    {
        private readonly IIssueServices _issueServices;
        private readonly IMapper _mapper;

        public IssueController(IIssueServices issueServices, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._issueServices = issueServices;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetIssueGrid")]
        public async Task<DataGridTable> GetIssueGrid()
        {
            DataGridTable issuetLst = await _issueServices.GetIssueGridAsync();
            return issuetLst;


        }

        [HttpPost, Route("SaveIssueAsync")]
        public async Task<ResultResource> SaveIssueAsync([FromBody] IssueHeaderResource resource)
        {
            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            if(resource.IssueDetails.FirstOrDefault().Qty > resource.ReceivedQty)
            {
                result.Message = "Received Quantity is not enough to issue.!";
                result.status = false;
                return result;

            }

            var issue = _mapper.Map<IssueHeaderResource, IssueHeader>(resource);
            var result2 = await _issueServices.SaveIssueAsync(issue);

            if (!result2.Success)
            {
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var issueresource = _mapper.Map<IssueHeader, IssueHeaderResource>(result2.Obj);
            DataGridTable issuetLst = await _issueServices.GetIssueGridAsync();
            result.ResultObject = issuetLst;
            return result;
        }
    }
}
