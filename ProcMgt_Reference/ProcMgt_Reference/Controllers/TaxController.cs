using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProcMgt_Reference.Extensions;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Communication;
using ProcMgt_Reference_Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcMgt_Reference.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TaxController : ControllerBase
    {
        private readonly ITaxServices _taxService;
        private readonly IMapper _mapper;


        public TaxController(ITaxServices taxservice, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._taxService = taxservice;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetUserRoleByID/{id}")]

        public async Task<IEnumerable<TaxResource>> GetUserRoleByID(int id)
        {
            var tax = await _taxService.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<Tax>, IEnumerable<TaxResource>>(tax);

            return resources;

        }

        [HttpGet, Route("GetAllTax")]

        public async Task<IEnumerable<TaxResource>> GetAllTax()
        {
            var tax = await _taxService.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<Tax>, IEnumerable<TaxResource>>(tax);

            return resources;

        }

        [HttpGet, Route("GetTaxGrid")]

        public async Task<DataGridTable> GetTaxGrid()
        {
            DataGridTable taxList = await _taxService.GetTaxGridAsync();
            return taxList;


        }

        [HttpPost, Route("SaveTaxAsync")]

        public async Task<ResultResource> SaveTaxAsync([FromBody] TaxResource resource)
        {

            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var tax = _mapper.Map<TaxResource, Tax>(resource);
            var result2 = await _taxService.SaveTaxAsync(tax);


            if (!result2.Success)
            {
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var taxresource = _mapper.Map<Tax, TaxResource>(result2.Obj);
            DataGridTable taxList = await _taxService.GetTaxGridAsync();

            result.ResultObject = taxList;
            return result;

        }

        [HttpPost, Route("UpdateTaxAsync/{id}")]

        public async Task<ResultResource> UpdateTaxAsync(string id, [FromBody] TaxResource resource)
        {

            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var Tax = _mapper.Map<TaxResource, Tax>(resource);
            var result2 = await _taxService.UpdateTaxAsync(id, Tax);


            if (!result2.Success)
            {
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var tax = _mapper.Map<Tax, TaxResource>(result2.Obj);
            DataGridTable taxList = await _taxService.GetTaxGridAsync();

            result.ResultObject = taxList;
            return result;


        }

        [HttpPost, Route("DeleteTaxAsync/{id}")]

        public async Task<IActionResult> DeleteTaxAsync(string id, [FromBody] TaxResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var tax = _mapper.Map<TaxResource, Tax>(resource);
            var result = await _taxService.DeleteTaxAsync(id, tax);


            if (!result.Success)
                return BadRequest(result.Message);

            var taxresource = _mapper.Map<Tax, TaxResource>(result.Obj);
            return Ok(taxresource = _mapper.Map<Tax, TaxResource>(result.Obj));

        }

    }
}
