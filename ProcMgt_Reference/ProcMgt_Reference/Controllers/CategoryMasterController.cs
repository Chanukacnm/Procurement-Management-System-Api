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
    public class CategoryMasterController : BaseApiController
    {
        private readonly ICategoryMasterServices _categoryMasterServices ;
        private readonly IMapper _mapper;
    

        public CategoryMasterController(ICategoryMasterServices categorymasterservice, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._categoryMasterServices = categorymasterservice;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetAllCategoryMaster")]        
        public async Task<IEnumerable<CategoryMasterResource>> GetAllCategoryMaster()
        {
            var categorymaster = await _categoryMasterServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryMasterResource>>(categorymaster);
            
            return resources;

        }

        [HttpGet, Route("GetCategoryMasterByID/{id}")]

        public async Task<IEnumerable<CategoryMasterResource>> GetCategoryMasterByID(int id)
        {
            var categorymaster = await _categoryMasterServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryMasterResource>>(categorymaster);

            return resources;

        }

        [HttpGet, Route("GetCategoryGrid")]
        public async Task<DataGridTable> GetCategoryGrid()
        {
            DataGridTable categoryLst = await _categoryMasterServices.GetCategoryGridAsync();
            return categoryLst;

        }

        [HttpPost, Route("SaveCategoryMasterAsync")]
        public async Task<ResultResource> SaveCategoryMasterAsync([FromBody] CategoryMasterResource resource)
        {
            ResultResource result = new ResultResource { status=true};

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }
               

            var CategoryMaster = _mapper.Map<CategoryMasterResource, Category>(resource);
            var result2 = await _categoryMasterServices.SaveCategoryMasterAsync(CategoryMaster);

            if (!result2.Success)
            {
                // result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.Message = result2.Message;
                result.status = false;
                return result;
            }                

            var categorymasterresource = _mapper.Map<Category, CategoryMasterResource>(result2.Obj);

            DataGridTable categoryLst = await _categoryMasterServices.GetCategoryGridAsync();

            result.ResultObject = categoryLst;
            return result;

        }

        [HttpPost, Route("UpdateCategoryMasterAsync/{id}")]
        public async Task<ResultResource> UpdateCategoryMasterAsync(string id, [FromBody] CategoryMasterResource resource)
        {
            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var CategoryMaster = _mapper.Map<CategoryMasterResource, Category>(resource);
            var result2 = await _categoryMasterServices.UpdateCategoryMasterAsync(id, CategoryMaster);


            if (!result2.Success)
            {
                //result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var categorymasterresource = _mapper.Map<Category, CategoryMasterResource>(result2.Obj);

            DataGridTable categoryLst = await _categoryMasterServices.GetCategoryGridAsync();

            result.ResultObject = categoryLst;
            
            return result;

        }

        [HttpPost, Route("DeleteCategoryMasterAsync/{id}")]
        public async Task<IActionResult> DeleteCategoryMasterAsync(string id, [FromBody] CategoryMasterResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());       

            var CategoryMaster = _mapper.Map<CategoryMasterResource, Category>(resource);
            var result = await _categoryMasterServices.DeleteCategoryMasterAsync(id, CategoryMaster);


            if (!result.Success)
                return BadRequest(result.Message);

            var categorymasterresource = _mapper.Map<Category, CategoryMasterResource>(result.Obj);
            return Ok(categorymasterresource);

        }

    }
}
