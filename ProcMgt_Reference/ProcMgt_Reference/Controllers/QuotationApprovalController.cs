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


    public class QuotationApprovalController : ControllerBase
    {
        private readonly IQuotationApprovalServices _quotApprovalServices;
        private readonly IMapper _mapper;

        public QuotationApprovalController(IQuotationApprovalServices quotationapproverService, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._quotApprovalServices = quotationapproverService;
            this._mapper = mapper;
        }


        [HttpGet, Route("GetQuotationApprovalGrid")]
        public async Task<DataGridTable> GetQuotationApprovalGrid()
        {
            try
            {
                DataGridTable quotationApprovalLst = await _quotApprovalServices.GetQuotationApprovalGridAsync();
                return quotationApprovalLst;
            }
            catch (Exception ex)
            {
                return null;
            }



        }

        [HttpGet, Route("GetAllQuotationApproval")]
        public async Task<IEnumerable<QuotationApprovalResource>> GetAllQuotationApproval()
        {


            var quotationApproval = await _quotApprovalServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<QuotationRequestHeader>, IEnumerable<QuotationApprovalResource>>(quotationApproval);

            return resources;

        }






    }



}
