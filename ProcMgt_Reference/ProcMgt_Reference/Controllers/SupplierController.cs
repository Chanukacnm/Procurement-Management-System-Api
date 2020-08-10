using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProcMgt_Reference.Extensions;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services;
using ProcMgt_Reference_Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcMgt_Reference.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : BaseApiController
    {
        private readonly ISupplierServices _supplierServices;
        private readonly IMapper _mapper;

        public SupplierController(ISupplierServices supplierservice, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._supplierServices = supplierservice;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetAllSupplier")]
        public async Task<IEnumerable<SupplierResource>> GetAllSupplier()
        {
            var supplier = await _supplierServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<Supplier>, IEnumerable<SupplierResource>>(supplier);

            return resources;

        }

        [HttpGet, Route("GetSupplierGrid")]
        public async Task<DataGridTable> GetSupplierGrid()
        {
            DataGridTable supplierLst = await _supplierServices.GetSupplierGridAsync();
            return supplierLst;

        }

        [HttpPost, Route("ContactDetailsGrid")]
        public async Task<DataGridTable> ContactDetailsGrid([FromBody] ContactDetailsResource resource)
        {
            var contactDetails = _mapper.Map<ContactDetailsResource, ContactDetails>(resource);
            DataGridTable contactDetailsList = await _supplierServices.GetContactDetailsGridAsync(contactDetails);
            return contactDetailsList;

        }

        [HttpPost, Route("SaveSupplierAsync")]
        public async Task<ResultResource> SaveSupplierAsync([FromBody] SupplierResource resource)

        {

            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var Supplier = _mapper.Map<SupplierResource, Supplier>(resource);
            var result2 = await _supplierServices.SaveSupplierAsync(Supplier);


            if (!result2.Success)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var supplierresource = _mapper.Map<Supplier, SupplierResource>(result2.Obj);

            DataGridTable supplierLst = await _supplierServices.GetSupplierGridAsync();

            result.ResultObject = supplierLst;
            return result;

        }

        [HttpPost, Route("UpdateSupplierAsync/{id}")]
        public async Task<ResultResource> UpdateSupplierAsync(string id, [FromBody] SupplierResource resource)
        {

            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var Supplier = _mapper.Map<SupplierResource, Supplier>(resource);
            var result2 = await _supplierServices.UpdateSupplierAsync(id, Supplier);


            if (!result2.Success)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var supplierresource = _mapper.Map<Supplier, SupplierResource>(result2.Obj);

            DataGridTable supplierLst = await _supplierServices.GetSupplierGridAsync();

            result.ResultObject = supplierLst;
            return result;

        }

        [HttpPost, Route("DeleteSupplierAsync/{id}")]
        public async Task<IActionResult> DeleteSupplierAsync(string id, [FromBody] SupplierResource resource)
        {
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var Supplier = _mapper.Map<SupplierResource, Supplier>(resource);
            var result = await _supplierServices.DeleteSupplierAsync(id, Supplier);


            if (!result.Success)
                return BadRequest(result.Message);

            var supplierresource = _mapper.Map<Supplier, SupplierResource>(result.Obj);
            return Ok(supplierresource);

        }

    }
}
