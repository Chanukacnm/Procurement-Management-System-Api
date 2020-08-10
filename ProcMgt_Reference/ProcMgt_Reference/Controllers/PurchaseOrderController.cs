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

    public class PurchaseOrderController : BaseApiController
    {
        private readonly IPurchaseOrderServices  _purchaseOrderServices;
        
        private readonly IMapper _mapper;


        public PurchaseOrderController(IPurchaseOrderServices purchaseOrderServices, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._purchaseOrderServices = purchaseOrderServices;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetAllPoHeader")]
        public async Task<IEnumerable<PoHeaderResource>> GetAllPoHeader()
        {
            var poheader = await _purchaseOrderServices.GetAllPoHeaderAsync();
            var resources = _mapper.Map<IEnumerable<Poheader>, IEnumerable<PoHeaderResource>>(poheader);

            return resources;

        }

        [HttpGet, Route("GetAllPoDetails")]
        public async Task<IEnumerable<PoDetailResource>> GetAllPoDetails()
        {
            var podetails = await _purchaseOrderServices.GetAllPoDetailsAsync();
            var resources = _mapper.Map<IEnumerable<Podetail>, IEnumerable<PoDetailResource>>(podetails);

            return resources;

        }

        [HttpPost, Route("GetAllPoReportDetails/{id}")]
        public async Task<ResultResource> GetAllPoReportDetails(string id, [FromBody] PoHeaderResource resource)
        {
            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var poHeader = _mapper.Map<PoHeaderResource, Poheader>(resource);
            //var result2 = await _purchaseOrderServices.GetAllPoReportDetailsAsync(id, poHeader);
            var poReportdetails = await _purchaseOrderServices.GetAllPoReportDetailsAsync(id, poHeader);
            //var resources = _mapper.Map<IEnumerable<Poheader>, IEnumerable<PoHeaderResource>>(poReportdetails);


            //return poReportdetails;
            result.ResultObject = poReportdetails;

            return result;


        }

        //[HttpGet, Route("GetPoHeaderByID/{id}")]

        //public async Task<IEnumerable<PoHeaderResource>> GetPohHeaderByID(int id)
        //{
        //    var poheader = await _poHeaderServices.GetAllAsync();
        //    var resources = _mapper.Map<IEnumerable<Poheader>, IEnumerable<PoHeaderResource>>(poheader);

        //    return resources;

        //}

        [HttpGet, Route("GetQuotationListGrid")]
        public async Task<DataGridTable> GetQuotationListGrid()
        {
            DataGridTable quotationList = await _purchaseOrderServices.GetQuotationListGridAsync();
            return quotationList;

        }

        [HttpPost, Route("GetQuotatioDetailsGrid")]
        public async Task<DataGridTable> GetQuotatioDetailsGrid([FromBody] PoHeaderResource resource)
        {
            var poheader = _mapper.Map<PoHeaderResource, Poheader>(resource);
            DataGridTable podetailsList = await _purchaseOrderServices.GetQuotatioDetailsListGridAsync(poheader);
            return podetailsList;

        }


        [HttpPost, Route("SavePurchaseOrderAsync")]
        public async Task<ResultResource> SavePurchaseOrderAsync([FromBody] PoHeaderResource resource)
        {
            ResultResource result = new ResultResource { status = true };
            if (!ModelState.IsValid) { 
                    //return BadRequest(ModelState.GetErrorMessages());
                    result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                    result.status = false;
                    return result;
            }



            var poheader = _mapper.Map<PoHeaderResource, Poheader>(resource);
            var result2 = await _purchaseOrderServices.SavePurchaseOrderAsync(poheader);

            if (!result2.Success)
            {
                //return BadRequest(result2.Message);
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var poheaderresource = _mapper.Map<Poheader, PoHeaderResource>(result2.Obj);
            //return Ok();
            DataGridTable quatoationList = await _purchaseOrderServices.GetQuotationListGridAsync();

            result.Details = poheaderresource.PoheaderID.ToString();
            result.ResultObject = quatoationList;
            return result;
            //return Ok(poheaderresource);

        }

        








    }

   

}
