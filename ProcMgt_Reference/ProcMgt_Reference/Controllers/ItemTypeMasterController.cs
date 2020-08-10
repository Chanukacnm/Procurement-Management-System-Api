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
    public class ItemTypeMasterController : ControllerBase
    {
        private readonly IItemTypeMasterServices _itemTypeMasterServices;
        private readonly IMapper _mapper;


        public ItemTypeMasterController(IItemTypeMasterServices itemtypemasterservice, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._itemTypeMasterServices = itemtypemasterservice;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetItemTypeMasterGrid")]

        public async Task<DataGridTable> GetItemTypeMasterGrid()
        {
            DataGridTable itemTypeLst = await _itemTypeMasterServices.GetItemTypeMasterGridAsync();
            return itemTypeLst;


        }

        [HttpGet, Route("GetAllItemTypeMaster")]
        public async Task<IEnumerable<ItemTypeMasterResource>> GetAllItemTypeMaster()
        {


            var itemtypemaster = await _itemTypeMasterServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<ItemType>, IEnumerable<ItemTypeMasterResource>>(itemtypemaster);

            return resources;

        }

        [HttpPost, Route("GetSpecItemTypeAll/{id}")]
        public async Task<ResultResource> GetSpecAllMake(string id, [FromBody]ItemTypeMasterResource resource)
        {
            ResultResource result = new ResultResource { status = true };

            var itemtype = _mapper.Map<ItemTypeMasterResource, ItemType>(resource);
            var result2 = await _itemTypeMasterServices.GetSpecItemTypeAllAsync(id, itemtype);

            var resources = _mapper.Map<IEnumerable<ItemType>, IEnumerable<ItemTypeMasterResource>>(result2);
            result.ResultObject = resources;

            return result;
        }

        [HttpGet, Route("GetItemTypeMasterByID/{id}")]

        public async Task<IEnumerable<ItemTypeMasterResource>> GetItemTypeMasterByID(int id)
        {
            var itemtypemaster = await _itemTypeMasterServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<ItemType>, IEnumerable<ItemTypeMasterResource>>(itemtypemaster);

            return resources;

        }

        [HttpPost, Route("SaveItemTypeMasterAsync")]

        public async Task<ResultResource> SaveItemTypeMasterAsync([FromBody] ItemTypeMasterResource resource)
        {

            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var ItemTypeMaster = _mapper.Map<ItemTypeMasterResource, ItemType>(resource);
            var result2 = await _itemTypeMasterServices.SaveItemTypeMasterAsync(ItemTypeMaster);


            if (!result2.Success)
            {
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var itemtypemasterresource = _mapper.Map<ItemType, ItemTypeMasterResource>(result2.Obj);

            DataGridTable itemTypeLst = await _itemTypeMasterServices.GetItemTypeMasterGridAsync();

            result.ResultObject = itemTypeLst;
            return result;
        }

        [HttpPost, Route("UpdateItemTypeMasterAsync/{id}")]

        public async Task<ResultResource> UpdateItemTypeMasterAsync(string id, [FromBody] ItemTypeMasterResource resource)
        {

            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var ItemTypeMaster = _mapper.Map<ItemTypeMasterResource, ItemType>(resource);
            var result2 = await _itemTypeMasterServices.UpdateItemTypeMasterAsync(id, ItemTypeMaster);


            if (!result2.Success)
            {
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var itemtypemasterresource = _mapper.Map<ItemType, ItemTypeMasterResource>(result2.Obj);


            DataGridTable itemTypeLst = await _itemTypeMasterServices.GetItemTypeMasterGridAsync();

            result.ResultObject = itemTypeLst;
            return result;

        }

        [HttpPost, Route("DeleteItemTypeMasterAsync/{id}")]

        public async Task<IActionResult> DeleteItemTypeMasterAsync(string id, [FromBody] ItemTypeMasterResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var ItemTypeMaster = _mapper.Map<ItemTypeMasterResource, ItemType>(resource);
            var result = await _itemTypeMasterServices.DeleteItemTypeMasterAsync(ItemTypeMaster, id);


            if (!result.Success)
                return BadRequest(result.Message);

            var itemtypemasterresource = _mapper.Map<ItemType, ItemTypeMasterResource>(result.Obj);
            return Ok(itemtypemasterresource = _mapper.Map<ItemType, ItemTypeMasterResource>(result.Obj));

        }



    }

}
