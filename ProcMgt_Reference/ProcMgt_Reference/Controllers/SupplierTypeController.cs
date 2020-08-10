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
    public class SupplierTypeController : ControllerBase
    {
        private readonly ISupplierTypeService _supplierservices;
        private readonly IMapper _mapper;

        public SupplierTypeController(ISupplierTypeService supplierservices, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._supplierservices = supplierservices;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetAllSupplierType")]
        public async Task<IEnumerable<SupplierTypeResources>> GetAllSupplierType()
        {


            var suppliertype = await _supplierservices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<SupplierType>, IEnumerable<SupplierTypeResources>>(suppliertype);

            return resources;

        }

        [HttpPost, Route("SaveSupplierTypeAsync")]

        public async Task<IActionResult> SaveSupplierTypeAsync(SupplierTypeResources resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var suppliertype = _mapper.Map<SupplierTypeResources, SupplierType>(resource);
            var result = await _supplierservices.SaveSupplierTypeAsync(suppliertype);


            if (!result.Success)
                return BadRequest(result.Message);

            var suppliertyperesource = _mapper.Map<SupplierType, SupplierTypeResources>(result.Obj);
            return Ok(suppliertyperesource);

        }
        [HttpPut, Route("UpdateSupplierTypeAsync/{id}")]

        public async Task<IActionResult> UpdateSupplierTypeAsync(string id, [FromBody] SupplierTypeResources resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var suppliertyper = _mapper.Map<SupplierTypeResources, SupplierType>(resource);
            var result = await _supplierservices.UpdateSupplierTypeAsync(id, suppliertyper);


            if (!result.Success)
                return BadRequest(result.Message);

            var suppliertyperesource = _mapper.Map<SupplierType, SupplierTypeResources>(result.Obj);
            return Ok(suppliertyperesource);

        }
    }
}
