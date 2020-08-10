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
    public class SupplierRegisteredItemsController : BaseApiController
    {
        private readonly ISupplierRegisteredItemsServices _supplierregistereditemsServices;
        private readonly IMapper _mapper;

        public SupplierRegisteredItemsController(ISupplierRegisteredItemsServices supplierregistereditemsService, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._supplierregistereditemsServices = supplierregistereditemsService;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetAllSupplierRegisteredItems")]
        public async Task<IEnumerable<SupplierRegisteredItemsResource>> GetAllSupplierRegisteredItems()
        {


            var supplierRegisteredItems = await _supplierregistereditemsServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<SupplierRegisteredItems>, IEnumerable<SupplierRegisteredItemsResource>>(supplierRegisteredItems);

            return resources;

        }

        [HttpPost, Route("GetItemsDescription/{id}")]
        public async Task<ResultResource> GetItemsDescription(string id, [FromBody]SupplierRegisteredItemsResource resource)
        {
            ResultResource result = new ResultResource { status = true };

            var supplierregistereditems = _mapper.Map<SupplierRegisteredItemsResource, SupplierRegisteredItems>(resource);
            var result2 = await _supplierregistereditemsServices.GetItemsDescription(id, supplierregistereditems);

            var resources = _mapper.Map<IEnumerable<ItemResource>, IEnumerable<Item>>(result2);
            result.ResultObject = result2;

            return result;
        }


        [HttpPost, Route("GetSupplierRegisteredItemsGrid/{id}")]
        public async Task<DataGridTable> GetSupplierRegisteredItemsGrid(string id, [FromBody] SupplierRegisteredItemsResource resource)
        {
            var supplierRegisteredItems = _mapper.Map<SupplierRegisteredItemsResource, SupplierRegisteredItems>(resource);
            DataGridTable SupplierRegItemList = await _supplierregistereditemsServices.getSupRegItemsList(id, supplierRegisteredItems);
            return SupplierRegItemList;
        }
        //[HttpGet, Route("GetSupplierRegisteredItemsGrid")]
        //public async Task<DataGridTable> GetSupplierRegisteredItemsGrid()
        //{
        //    DataGridTable categoryLst = await _supplierregistereditemsServices.GetSupplierRegisteredItemsGrid();
        //    return categoryLst;

            //}

        [HttpPost, Route("SaveSupplierRegisteredItemsAsync")]
        public async Task<IActionResult> SaveSupplierRegisteredItemsAsync([FromBody] SupplierRegisteredItemsResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var supplierRegisteredItems = _mapper.Map<SupplierRegisteredItemsResource, SupplierRegisteredItems>(resource);
            var result = await _supplierregistereditemsServices.SaveSupplierRegisteredItemsAsync(supplierRegisteredItems);


            if (!result.Success)
                return BadRequest(result.Message);

            var supplierregistereditemsresource = _mapper.Map<SupplierRegisteredItems, SupplierRegisteredItemsResource>(result.Obj);
            return Ok(supplierregistereditemsresource);

        }

        [HttpPut, Route("UpdateSupplierRegisteredItemsAsync/{id}")]
        public async Task<IActionResult> UpdateSupplierRegisteredItemsAsync(string id, [FromBody] SupplierRegisteredItemsResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var supplierRegisteredItems = _mapper.Map<SupplierRegisteredItemsResource, SupplierRegisteredItems>(resource);
            var result = await _supplierregistereditemsServices.UpdateSupplierRegisteredItemsAsync(id, supplierRegisteredItems);


            if (!result.Success)
                return BadRequest(result.Message);

            var supplierregistereditemsresource = _mapper.Map<SupplierRegisteredItems, SupplierRegisteredItemsResource>(result.Obj);
            return Ok(supplierregistereditemsresource);

        }

    }
}
