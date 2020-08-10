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
    public class QuotationRequestHeaderController : BaseApiController
    {
        private readonly IQuotationRequestHeaderServices _quotationrequestheaderServices;
        private readonly IQuotationEnterServices _quotationenterServices;
        private readonly IQuotationRequestDetailsServices _quotationRequestdetailsServices;
        private readonly IMapper _mapper;

        public QuotationRequestHeaderController(IQuotationRequestHeaderServices quotationrequestheaderservice, IQuotationEnterServices quotationenterservice, IQuotationRequestDetailsServices quotationRequestdetailsService, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._quotationrequestheaderServices = quotationrequestheaderservice;
            this._quotationenterServices = quotationenterservice;
            this._quotationRequestdetailsServices = quotationRequestdetailsService;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetAllQuotationRequest")]
        public async Task<IEnumerable<QuotationRequestHeaderResource>> GetAllQuotationRequest()
        {
            var quotationRequest = await _quotationrequestheaderServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<QuotationRequestHeader>, IEnumerable<QuotationRequestHeaderResource>>(quotationRequest);

            return resources;

        }

        [HttpGet, Route("GetQuotationRequestDetailsGrid")]
        public async Task<DataGridTable> GetQuotationRequestDetailsGrid()
        {
            DataGridTable quotationRequestdetailsLst = await _quotationRequestdetailsServices.GetQuotationRequestDetailsGrid();
            return quotationRequestdetailsLst;

        }


        [HttpGet, Route("GetQuotationRequestGrid")]
        public async Task<DataGridTable> GetQuotationRequestGrid()
        {
            DataGridTable quotationReqLst = await _quotationrequestheaderServices.GetQuotationRequestHeaderGridAsync();
            return quotationReqLst;

        }

        //[HttpPost, Route("SaveQuotationRequestHeaderAsync")]
        //public async Task<IActionResult> SaveQuotationRequestHeaderAsync([FromBody] QuotationRequestHeaderResource resource)

        //{
        //    try {
        //        if (!ModelState.IsValid)
        //            return BadRequest(ModelState.GetErrorMessages());

        //        var quotationRequest = _mapper.Map<QuotationRequestHeaderResource, QuotationRequestHeader>(resource);
        //        var result = await _quotationrequestheaderServices.SaveQuotationRequestHeaderAsync(quotationRequest);


        //        if (!result.Success)
        //            return BadRequest(result.Message);

        //        var quotationRequestresource = _mapper.Map<QuotationRequestHeader, QuotationRequestHeaderResource>(result.Obj);

        //        return Ok();
        //        //return Ok(quotationRequestresource);

        //    }
        //    catch (Exception ex) {

        //        return null;
        //    }


        //}


        [HttpPost, Route("SaveQuotationRequestHeaderAsync")]
        public async Task<ResultResource> SaveQuotationRequestHeaderAsync([FromBody] QuotationRequestHeaderResource resource)

        {
            try
            {
                ResultResource result = new ResultResource { status = true };

                if (!ModelState.IsValid)
                {
                    result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                    result.status = false;
                    return result;
                }

                var quotationRequest = _mapper.Map<QuotationRequestHeaderResource, QuotationRequestHeader>(resource);
                var result2 = await _quotationrequestheaderServices.SaveQuotationRequestHeaderAsync(quotationRequest);


                if (!result2.Success)
                {
                    result.Message = result2.Message;
                    result.status = false;
                    return result;
                }


                var quotationRequestresource = _mapper.Map<QuotationRequestHeader, QuotationRequestHeaderResource>(result2.Obj);


                DataGridTable quotationReqLst = await _quotationrequestheaderServices.GetQuotationRequestHeaderGridAsync();
                result.ResultObject = quotationReqLst;
                return result;
                //return Ok(quotationRequestresource);

            }
            catch (Exception ex)
            {

                return null;
            }


        }

        [HttpPost, Route("UpdateQuotationRequestHeaderAsync/{id}")]
        public async Task<ResultResource> UpdateQuotationRequestHeaderAsync(string id, [FromBody] QuotationRequestHeaderResource resource)
        {
            try
            {
                ResultResource result = new ResultResource { status = true };

                if (!ModelState.IsValid)
                {
                    result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                    result.status = false;
                    return result;
                }

                var quotationRequest = _mapper.Map<QuotationRequestHeaderResource, QuotationRequestHeader>(resource);
                var result2 = await _quotationrequestheaderServices.UpdateQuotationRequestHeaderAsync(id, quotationRequest);

                if (!result2.Success)
                {
                    result.Message = result2.Message;
                    result.status = false;
                    return result;
                }


                var quotationRequestresource = _mapper.Map<QuotationRequestHeader, QuotationRequestHeaderResource>(result2.Obj);


                DataGridTable quotationReqLst = await _quotationrequestheaderServices.GetQuotationRequestHeaderGridAsync();
                result.ResultObject = quotationReqLst;
                return result;
            }

            catch (Exception ex)
            {
                return null;
            }
           
        }

        [HttpPost, Route("UpdateQuotationRequestDetailsrAsync/{id}")]
        public async Task<ResultResource> UpdateQuotationRequestDetailsrAsync(string id, [FromBody] QuotationRequestHeaderResource resource)
        {
            try
            {
                ResultResource result = new ResultResource { status = true };

                if (!ModelState.IsValid)
                {
                    result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                    result.status = false;
                    return result;
                }

                var quotationRequest = _mapper.Map<QuotationRequestHeaderResource, QuotationRequestHeader>(resource);
                var result2 = await _quotationrequestheaderServices.UpdateQuotationRequestDetailsrAsync(id, quotationRequest);

                if (!result2.Success)
                {
                    result.Message = result2.Message;
                    result.status = false;
                    return result;
                }

                var quotationRequestresource = _mapper.Map<QuotationRequestHeader, QuotationRequestHeaderResource>(result2.Obj);

                DataGridTable pendingQuotationRequestdetailsLst = await _quotationenterServices.GetPendingQuotationRequestDetailsGrid();
                result.ResultObject = pendingQuotationRequestdetailsLst;

                return result;
            }

            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
