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

    public class ItemController : ControllerBase
    {

        private readonly IItemServices _itemServices;
        private readonly IMapper _mapper;

        public ItemController(IItemServices itemService, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._itemServices = itemService;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetAllItem")]
        public async Task<IEnumerable<ItemResource>> GetAllItem()
        {


            var item = await _itemServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<Item>, IEnumerable<ItemResource>>(item);

            return resources;

        }

        //[HttpGet, Route("GetAllStock")]
        //public async Task<IEnumerable<StockResource>> GetAllStock()
        //{


        //    var stock = await _itemServices.GetStockAllAsync();
        //    var resources = _mapper.Map<IEnumerable<Stock>, IEnumerable<StockResource>>(stock);

        //    return resources;

        //}

        [HttpPost, Route("GetSpecItemAll/{id}")]
        public async Task<ResultResource> GetSpecItemAll(string id, [FromBody]ItemResource resource)
        {
            ResultResource result = new ResultResource { status = true };

            var item = _mapper.Map<ItemResource, Item>(resource);
            var result2 = await _itemServices.GetSpecItemAllAsync(id,item);

            var resources = _mapper.Map<IEnumerable<Item>, IEnumerable<ItemResource>>(result2);
            result.ResultObject = resources;

            return result;
        }


        [HttpGet, Route("GetItemGrid")]

        public async Task<DataGridTable> GetItemGrid()
        {
            DataGridTable itemLst = await _itemServices.GetItemGridAsync();
            return itemLst;


        }

        //[HttpPost, Route("SaveItemAsync")]
        //public async Task<ResultResource> SaveItemAsync([FromBody] ItemResource resource)
        //{

        //    ResultResource result = new ResultResource { status = true };

        //    if (!ModelState.IsValid)
        //    {
        //        result.Message = ModelState.GetErrorMessages().FirstOrDefault();
        //        result.status = false;
        //        return result;
        //    } 

        //    var item = _mapper.Map<ItemResource, Item>(resource);
        //    var result2 = await _itemServices.SaveItemAsync(item);


        //    if (!result2.Success)
        //    {
        //        result.Message =result2.Message;
        //        result.status = false;
        //        return result;
        //    }

        //    var itemresource = _mapper.Map<Item, ItemResource>(result2.Obj);

        //    DataGridTable itemLst = await _itemServices.GetItemGridAsync();

        //    result.ResultObject = itemLst;
        //    return result;

        //}


        [HttpPost, Route("SaveItemAsync")]
        public async Task<ResultResource> SaveItemAsync([FromBody] ItemViewResource resource)
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



                //foreach (Item itm in resource.Items)
                //{
                //    item2 = _mapper.Map<ItemViewResource, Item>(resource);

                //}

                //var item2 = _mapper.Map<ItemViewResource, Item>(resource);
                //var stock = _mapper.Map<ItemViewResource, Stock>(resource);
                var result2 = await _itemServices.SaveItemAsync(resource.Items, resource.Stock);


                if (!result2.Success)
                {
                    result.Message = result2.Message;
                    result.status = false;
                    return result;
                }

                var itemresource = _mapper.Map<Item, ItemResource>(result2.Obj);

                DataGridTable itemLst = await _itemServices.GetItemGridAsync();

                result.ResultObject = itemLst;
                return result;

            }
            catch (Exception ex)
            {
                return null;

            }



        }


        //[HttpPost, Route("UpdateItemAsync/{id}")]
        //public async Task<ResultResource> UpdateItemAsync(string id, [FromBody] ItemResource resource)
        //{

        //    ResultResource result = new ResultResource { status = true };

        //    if (!ModelState.IsValid)
        //    {
        //        result.Message = ModelState.GetErrorMessages().FirstOrDefault();
        //        result.status = false;
        //        return result;
        //    }

        //    var item = _mapper.Map<ItemResource, Item>(resource);
        //    var result2 = await _itemServices.UpdateItemAsync(id, item);


        //    if (!result2.Success)
        //    {
        //        result.Message =result2.Message;
        //        result.status = false;
        //        return result;
        //    }

        //    var itemresource = _mapper.Map<Item, ItemResource>(result2.Obj);

        //    DataGridTable itemLst = await _itemServices.GetItemGridAsync();

        //    result.ResultObject = itemLst;
        //    return result;

        //}

        [HttpPost, Route("UpdateItemAsync/{id}")]
        public async Task<ResultResource> UpdateItemAsync(string id, [FromBody] ItemViewResource resource)
        {

            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            //var item = _mapper.Map<ItemResource, Item>(resource);
            var result2 = await _itemServices.UpdateItemAsync(id, resource.Items , resource.Stock);


            if (!result2.Success)
            {
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var itemresource = _mapper.Map<Item, ItemResource>(result2.Obj);

            DataGridTable itemLst = await _itemServices.GetItemGridAsync();

            result.ResultObject = itemLst;
            return result;

        }

        [HttpPost, Route("DeleteItemAsync/{id}")]

        public async Task<IActionResult> DeleteItemAsync(string id, [FromBody] ItemResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var item = _mapper.Map<ItemResource, Item>(resource);
            var result = await _itemServices.DeleteItemAsync(item, id);


            if (!result.Success)
                return BadRequest(result.Message);

            var itemresource = _mapper.Map<Item, ItemResource>(result.Obj);
            return Ok(itemresource = _mapper.Map<Item, ItemResource>(result.Obj));

        }
    }
}
