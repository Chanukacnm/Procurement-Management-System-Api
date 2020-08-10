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
    public class ItemRequestController : ControllerBase
    {
        private readonly IItemRequestServices _itemRequestServices;
        //private readonly IApprovalScreenServices _approvalscreenServices;
        private readonly IMapper _mapper;


        public ItemRequestController(IItemRequestServices itemrequestservice, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._itemRequestServices = itemrequestservice;
            this._mapper = mapper;
        }

        [HttpPost, Route("GetItemRequestGrid")]
        public async Task<DataGridTable> GetItemRequestGrid([FromBody] ItemRequestResource resource)
        {
            //var user = _mapper.Map<UserResource, User>(resource);
            DataGridTable itemRequestLst = await _itemRequestServices.GetItemRequestGridAsync(resource);
            return itemRequestLst;
        }


        //[HttpPost, Route("GetItemRequestGrid")]
        //public async Task<DataGridTable> GetItemRequestGrid([FromBody] UserResource resource)
        //{
        //    var user = _mapper.Map<UserResource, User>(resource);
        //    DataGridTable itemRequestLst = await _itemRequestServices.GetItemRequestGridAsync(user);
        //    return itemRequestLst;
        //}

        [HttpGet, Route("GetAllItemRequest")]
        public async Task<IEnumerable<ItemRequestResource>> GetAllItemRequest()
        {


            var itemRequest = await _itemRequestServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<ItemRequest>, IEnumerable<ItemRequestResource>>(itemRequest);

            return resources;

        }

        [HttpGet, Route("GetItemRequestByID/{id}")]

        public async Task<IEnumerable<ItemRequestResource>> GetItemRequestByID(int id)
        {
            var itemrequest = await _itemRequestServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<ItemRequest>, IEnumerable<ItemRequestResource>>(itemrequest);

            return resources;

        }

        //[HttpPost, Route("SaveItemRequestAsync")]

        //public async Task<IActionResult> SaveItemRequest([FromBody] ItemRequestResource resource)
        //{

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState.GetErrorMessages());

        //    var ItemRequest = _mapper.Map<ItemRequestResource, ItemRequest>(resource);
        //    var result = await _itemRequestServices.SaveItemRequestAsync(ItemRequest);


        //    if (!result.Success)
        //        return BadRequest(result.Message);

        //    var itemrequestresource = _mapper.Map<ItemRequest, ItemRequestResource>(result.Obj);
        //    return Ok(itemrequestresource);
        //}


        [HttpPost, Route("SaveItemRequestAsync")]
        public async Task<ResultResource> SaveItemRequest( ItemRequestResource resource )
        {
            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var ItemRequest = _mapper.Map<ItemRequestResource, ItemRequest>(resource);
            var result2 = await _itemRequestServices.SaveItemRequestAsync(ItemRequest);


            if (!result2.Success)
            {
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var itemrequestresource = _mapper.Map<ItemRequest, ItemRequestResource>(result2.Obj);

            DataGridTable itemRequestLst = await _itemRequestServices.GetItemRequestGridAsync(resource);
            result.ResultObject = itemRequestLst;

            return result;
        }


        //[HttpPost, Route("UpdateItemRequestAsync/{id}")]
        //public async Task<IActionResult> UpdateItemRequest(string id, [FromBody] ItemRequestResource resource)
        //{

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState.GetErrorMessages());

        //    var ItemRequest = _mapper.Map<ItemRequestResource, ItemRequest>(resource);
        //    var result = await _itemRequestServices.UpdateItemRequestAsync(id, ItemRequest);


        //    if (!result.Success)
        //        return BadRequest(result.Message);

        //    var itemrequestresource = _mapper.Map<ItemRequest, ItemRequestResource>(result.Obj);
        //    return Ok(itemrequestresource);

        //}

        [HttpPost, Route("UpdateItemRequestAsync/{id}")]
        public async Task<ResultResource> UpdateItemRequest(string id, [FromBody] ItemRequestResource resource)
        {
            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var ItemRequest = _mapper.Map<ItemRequestResource, ItemRequest>(resource);
            var result2 = await _itemRequestServices.UpdateItemRequestAsync(id, ItemRequest, resource.ReceivedQty , resource.BalancedQty);

            if (!result2.Success)
            {
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var itemrequestresource = _mapper.Map<ItemRequest, ItemRequestResource>(result2.Obj);

            //if(resource.IsApproved == true || resource.IsRejected == true)
            //{
            //    DataGridTable approvalscreen = await _approvalscreenServices.GetApprovalScreenGridAsync();
            //    result.ResultObject = approvalscreen;
            //}
            //else
            //{
                DataGridTable itemRequestLst = await _itemRequestServices.GetItemRequestGridAsync(resource);
                result.ResultObject = itemRequestLst;
            //}

            

            return result;

        }

        [HttpPost, Route("DeleteItemRequestAsync/{id}")]

        public async Task<IActionResult> DeleteItemRequest(string id, [FromBody] ItemRequestResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var ItemRequest = _mapper.Map<ItemRequestResource, ItemRequest>(resource);
            var result = await _itemRequestServices.DeleteItemRequestAsync( id, ItemRequest);


            if (!result.Success)
                return BadRequest(result.Message);

            var itemrequestresource = _mapper.Map<ItemRequest, ItemRequestResource>(result.Obj);
            return Ok(itemrequestresource = _mapper.Map<ItemRequest, ItemRequestResource>(result.Obj));

        }



    }
}
