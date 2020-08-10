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

namespace ProcMgt_Reference.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ApprovalScreenController : ControllerBase
    {
        private readonly IApprovalScreenServices _approvalScreenServices; 
        private readonly IMapper _mapper;

        public ApprovalScreenController(IApprovalScreenServices approvalscreenservice, IMapper mapper)
        {
            this._approvalScreenServices = approvalscreenservice;
            this._mapper = mapper;
        }

        //[HttpGet, Route("GetApprovalScreenGrid")]
        //public async Task<DataGridTable> GetApprovalScreenGrid()
        //{
        //    try
        //    {
        //        DataGridTable approvalScreenLst = await _approvalScreenServices.GetApprovalScreenGridAsync();
        //        return approvalScreenLst;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }



        //}

        [HttpPost, Route("GetApprovalScreenGrid")]
        public async Task<DataGridTable> GetApprovalScreenGrid([FromBody] UserResource resource)
        {
            var user = _mapper.Map<UserResource, User>(resource);
            DataGridTable approvalScreenLst = await _approvalScreenServices.GetApprovalScreenGridAsync(user);
            return approvalScreenLst;
        



        }

        [HttpGet, Route("GetAllApprovalScreen")]
        public async Task<IEnumerable<ApprovalScreenResource>> GetAllApprovalScreen()
        {


            var approvalscreen = await _approvalScreenServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<ItemRequest>, IEnumerable<ApprovalScreenResource>>(approvalscreen);

            return resources;

        }
    }
}
