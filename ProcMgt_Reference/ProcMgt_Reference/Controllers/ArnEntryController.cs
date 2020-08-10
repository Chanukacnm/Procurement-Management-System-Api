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
    public class ArnEntryController : BaseApiController
    {
        private readonly IArnEntryServices _arnentryServices;
        private readonly IMapper _mapper;


        public ArnEntryController(IArnEntryServices arnentryServices, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._arnentryServices = arnentryServices;
            this._mapper = mapper;
        }

        [HttpPost, Route("GetAllArnheader")]
        public async Task<Arnheader> GetAllArnheader([FromBody] ArnheaderResource resource)
        {
            //var arnheader = await _arnentryServices.GetAllArnheaderAsync(resource);

            var arn = _mapper.Map<ArnheaderResource, Arnheader>(resource);
            var result = await _arnentryServices.GetAllArnheaderAsync(arn);

             

            return result;

        }

        [HttpGet, Route("GetAllArndetail")]
        public async Task<IEnumerable<ArndetailResource>> GetAllArndetail()
        {
            var arndetails = await _arnentryServices.GetArndetailAsync();
            var resources = _mapper.Map<IEnumerable<Arndetail>, IEnumerable<ArndetailResource>>(arndetails);


            return resources;

        }


        [HttpPost, Route("GetArndetailGrid")]
        public async Task<DataGridTable> GetArndetailGrid([FromBody] ArnheaderResource resource)
        {
            var arnheader = _mapper.Map<ArnheaderResource, Arnheader>(resource);
            DataGridTable ArndetailList = await _arnentryServices.GetArndetailGridAsync(arnheader);
            return ArndetailList;

        }

        [HttpGet, Route("GetPOGrListGridAsync")]
        public async Task<DataGridTable> GetPOGrListdAsync()
        {
            DataGridTable POGrLis = await _arnentryServices.GetPOGrListGridAsync();
            return POGrLis;

        }

        [HttpPost, Route("SaveArnheader")]
        public async Task<ResultResource> SaveArnheader([FromBody] ArnheaderResource resource)

        {

            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;

            }
                

            var arn = _mapper.Map<ArnheaderResource, Arnheader>(resource);
            var result2 = await _arnentryServices.SaveArnheaderAsync(arn);


            if (!result2.Success)
            {

                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }
                

            var arnheaderResource = _mapper.Map<Arnheader, ArnheaderResource>(result2.Obj);

            DataGridTable poh = await _arnentryServices.GetPOGrListGridAsync();

            result.ResultObject = poh;
            result.Details = arnheaderResource.Arnnumber; //--- Add By Nipuna Francisku 
            return result;
           

        }



        [HttpPut, Route("UpdateArnheaderAsync/{id}")]
        public async Task<IActionResult> UpdateArnheaderAsync(string id, [FromBody] ArnheaderResource resource)
        {

            

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var arnheader = _mapper.Map<ArnheaderResource, Arnheader>(resource);
            var result = await _arnentryServices.UpdateArnheaderAsync( id,arnheader);


            if (!result.Success)
                return BadRequest(result.Message);

            var supplierresource = _mapper.Map<Arnheader, ArnheaderResource>(result.Obj);
            return Ok(supplierresource);

        }

        [HttpPost, Route("DeleteArnheaderAsync/{id}")]
        public async Task<IActionResult> DeleteArnheaderAsync(string id, [FromBody] ArnheaderResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var arnheader = _mapper.Map<ArnheaderResource, Arnheader>(resource);
            var result = await _arnentryServices.DeleteArnheaderAsync( id,arnheader);


            if (!result.Success)
                return BadRequest(result.Message);

            var supplierresource = _mapper.Map<Arnheader, ArnheaderResource>(result.Obj);
            return Ok(supplierresource);

        }

    }
}
