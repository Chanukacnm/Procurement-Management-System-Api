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
    public class QuotationEnterController : BaseApiController
    {
        private readonly IQuotationEnterServices _quotationenterServices;
        private readonly IMapper _mapper;

        public QuotationEnterController(IQuotationEnterServices quotationenterservice,  IMapper mapper)// IGenericRepo<User> repo)
        {
            this._quotationenterServices = quotationenterservice;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetAllQuotationEnter")]
        public async Task<IEnumerable<QuotationRequestHeaderResource>> GetAllQuotationRequest()
        {
            var quotationenter = await _quotationenterServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<QuotationRequestHeader>, IEnumerable<QuotationRequestHeaderResource>>(quotationenter);

            return resources;

        }


        [HttpGet, Route("GetPendingQuotationRequestDetailsGrid")]
        public async Task<DataGridTable> GetQuotationRequestDetailsGrid()
        {
            DataGridTable pendingQuotationRequestdetailsLst = await _quotationenterServices.GetPendingQuotationRequestDetailsGrid();
            return pendingQuotationRequestdetailsLst;

        }

        [HttpPost, Route("QuotationDetailsGrid")]
        public async Task<DataGridTable> QuotationDetailsGrid([FromBody] QuotationRequestDetailsResource resource)
        {
            var quotationDetails = _mapper.Map<QuotationRequestDetailsResource, QuotationRequestDetails>(resource);
            DataGridTable quotationDetailsList = await _quotationenterServices.QuotationDetailsGrid(quotationDetails);
            return quotationDetailsList;

        }

        [HttpPost, Route("UpdateQuotationEnterAsync/{id}")]

        public async Task<IActionResult> UpdateQuotationEnterAsync(string id, [FromBody] QuotationRequestDetailsResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var QuotationEnter = _mapper.Map<QuotationRequestDetailsResource, QuotationRequestDetails>(resource);
            var result = await _quotationenterServices.UpdateQuotationEnterAsync(id, QuotationEnter);


            if (!result.Success)
                return BadRequest(result.Message);

            var quotationenterresource = _mapper.Map<QuotationRequestDetails, QuotationRequestDetailsResource>(result.Obj);
            return Ok(quotationenterresource);

        }

    }
}
