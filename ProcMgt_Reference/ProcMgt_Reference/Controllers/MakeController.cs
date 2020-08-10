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
    public class MakeController : ControllerBase
    {

        private readonly IMakeServices _makeServices;
        private readonly IMapper _mapper;


        public MakeController(IMakeServices makeservice, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._makeServices = makeservice;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetMakeGrid")]

        public async Task<DataGridTable> GetMakeGrid()
        {
            DataGridTable makeLst = await _makeServices.GetMakeGridAsync();
            return makeLst;


        }

        [HttpGet, Route("GetAllMake")]
        public async Task<IEnumerable<MakeResource>> GetAllMake()
        {
            var make = await _makeServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<Make>, IEnumerable<MakeResource>>(make);

            return resources;

        }

        [HttpPost, Route("GetSpecAllMake/{id}")]
        public async Task<ResultResource> GetSpecAllMake(string id, [FromBody]MakeResource resource)
        {
            ResultResource result = new ResultResource { status = true };

            var make = _mapper.Map<MakeResource, Make>(resource);
            var result2 = await _makeServices.GetSpecMakeAllAsync(id, make);

            var resources = _mapper.Map<IEnumerable<Make>, IEnumerable<MakeResource>>(result2);
            result.ResultObject = resources;

            return result;
        }

        [HttpPost, Route("GetSpecListMake/{id}")]
        public async Task<ResultResource> GetSpecListMake(string id, [FromBody]ItemResource resource)
        {
            ResultResource result = new ResultResource { status = true };

            var make = _mapper.Map<ItemResource, Item>(resource);
            var result2 = await _makeServices.GetSpecMakeListAsync(id, make);

            //var resources = _mapper.Map<IEnumerable<MakeResource>, IEnumerable<Make>>(result2);
            result.ResultObject = result2;

            return result;
        }


        [HttpPost, Route("GetSpecSecondAllMake/{id}")]
        public async Task<ResultResource> GetSpecSecondAllMake(string id, [FromBody]MakeResource resource)
        {
            ResultResource result = new ResultResource { status = true };

            var make = _mapper.Map<MakeResource, Make>(resource);
            var result2 = await _makeServices.GetSpecMakeAllAsync(id, make);

            var resources = _mapper.Map<IEnumerable<Make>, IEnumerable<MakeResource>>(result2);
            result.ResultObject = resources;

            return result;
        }


        //[HttpGet, Route("GetMakeByID/{id}")]

        //public async Task<IEnumerable<MakeResource>> GetMakeByID(int id)
        //{
        //    var make = await _makeServices.GetAllAsync();
        //    var resources = _mapper.Map<IEnumerable<Make>, IEnumerable<MakeResource>>(make);

        //    return resources;

        //}

        [HttpPost, Route("SaveMakeAsync")]
        public async Task<ResultResource> SaveMakeAsync([FromBody] MakeResource resource)
        {

            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var make = _mapper.Map<MakeResource, Make>(resource);
            var result2 = await _makeServices.SaveMakeAsync(make);


            if (!result2.Success)
            {
                result.Message = result2.Message;
                result.status = false;
                return result;

            }

            var makeresource = _mapper.Map<Make, MakeResource>(result2.Obj);

            DataGridTable makeLst = await _makeServices.GetMakeGridAsync();
            
            result.ResultObject = makeLst;
            return result;

        }

        [HttpPost, Route("UpdateMakeAsync/{id}")]
        public async Task<ResultResource> UpdateMakeAsync(string id, [FromBody] MakeResource resource)
        {

            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var Make = _mapper.Map<MakeResource, Make>(resource);
            var result2 = await _makeServices.UpdateMakeAsync(id, Make);


            if (!result2.Success)
            {
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var makeresource = _mapper.Map<Make, MakeResource>(result2.Obj);
            DataGridTable makeLst = await _makeServices.GetMakeGridAsync();

            result.ResultObject = makeLst;
            return result;

        }

        [HttpPost, Route("DeleteMakeAsync/{id}")]

        public async Task<IActionResult> DeleteMakeAsync(string id, [FromBody] MakeResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var Make = _mapper.Map<MakeResource, Make>(resource);
            var result = await _makeServices.DeleteMakeAsync(Make, id);


            if (!result.Success)
                return BadRequest(result.Message);

            var makeresource = _mapper.Map<Make, MakeResource>(result.Obj);
            return Ok(makeresource = _mapper.Map<Make, MakeResource>(result.Obj));

        }
    }
}
