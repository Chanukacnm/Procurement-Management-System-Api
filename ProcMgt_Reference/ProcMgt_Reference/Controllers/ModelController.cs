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
    public class ModelController : ControllerBase
    {
        private readonly IModelServices _modelServices;
        private readonly IMapper _mapper;


        public ModelController(IModelServices modelservice, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._modelServices = modelservice;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetAllModel")]
        public async Task<IEnumerable<ModelResource>> GetAllModel()
        {
            var model = await _modelServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<Model>, IEnumerable<ModelResource>>(model);

            return resources;

        }


        [HttpPost, Route("GetSpecModelAll/{id}")]
        public async Task<ResultResource> GetSpecModelAll(string id, [FromBody]ModelResource resource)
        {
            ResultResource result = new ResultResource { status = true };

            var model = _mapper.Map<ModelResource, Model>(resource);
            var result2 = await _modelServices.GetSpecModelAllAsync(id, model);

            var resources = _mapper.Map<IEnumerable<Model>, IEnumerable<ModelResource>>(result2);
            result.ResultObject = resources;

            return result;
        }

        [HttpGet, Route("GetModelGrid")]

        public async Task<DataGridTable> GetModelGrid()
        {
            DataGridTable modelLst = await _modelServices.GetModelGridAsync();
            return modelLst;


        }

        [HttpGet, Route("GetModelByID/{id}")]

        public async Task<IEnumerable<ModelResource>> GetModelByID(int id)
        {
            var model = await _modelServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<Model>, IEnumerable<ModelResource>>(model);

            return resources;

        }
        [HttpPost, Route("SaveModelAsync")]

        public async Task<ResultResource> SaveModelAsync([FromBody] ModelResource resource)
        {

            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var Model = _mapper.Map<ModelResource, Model>(resource);
            var result2 = await _modelServices.SaveModelAsync(Model);


            if (!result2.Success)
            {
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var modelresource = _mapper.Map<Model, ModelResource>(result2.Obj);

            DataGridTable modelLst = await _modelServices.GetModelGridAsync();
            result.ResultObject = modelLst;
            result.Message = "Model has been Save successfully";
            return result;

        }
        [HttpPost, Route("UpdateModelAsync/{id}")]

        public async Task<ResultResource> UpdateModelAsync(string id, [FromBody] ModelResource resource)
        {
            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var Model = _mapper.Map<ModelResource, Model>(resource);
            var result2 = await _modelServices.UpdateModelAsync(id, Model);


            if (!result2.Success)
            {
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var modelresource = _mapper.Map<Model, ModelResource>(result2.Obj);
            DataGridTable modelLst = await _modelServices.GetModelGridAsync();
            result.ResultObject = modelLst;
            result.Message = "Model has been Update successfully";
            return result;

        }

        [HttpPost, Route("DeleteModelAsync/{id}")]

        public async Task<IActionResult> DeleteModelAsync(string id, [FromBody] ModelResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var Model = _mapper.Map<ModelResource, Model>(resource);
            var result = await _modelServices.DeleteModelAsync(Model, id);


            if (!result.Success)
                return BadRequest(result.Message);

            var modelresource = _mapper.Map<Model, ModelResource>(result.Obj);
            return Ok(modelresource = _mapper.Map<Model, ModelResource>(result.Obj));

        }

    }
}
