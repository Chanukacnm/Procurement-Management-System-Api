using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProcMgt_Reference.Extensions;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProcMgt_Reference_Services.Interfaces;

namespace ProcMgt_Reference.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementUnitController : ControllerBase
    {
       
        private readonly IMeasurementUnitServices _measurementUnitService;
        private readonly IMapper _mapper;


        public MeasurementUnitController(IMeasurementUnitServices measurementunitservice, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._measurementUnitService = measurementunitservice;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetAllMeasurementUnits")]
        public async Task<IEnumerable<MeasurementUnitResource>> GetAllMeasurementUnits()
        {
            var measurementunits = await _measurementUnitService.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<MeasurementUnits>, IEnumerable<MeasurementUnitResource>>(measurementunits);

            return resources;

        }

        [HttpGet, Route("GetMeasurementUnitGrid")]

        public async Task<DataGridTable> GetMeasurementUnitGrid()
        {
            DataGridTable measurementUnitLst = await _measurementUnitService.GetMeasurementUnitGridAsync();
            return measurementUnitLst;


        }

        [HttpPost, Route("SaveMeasurementUnitsAsync")]

        public async Task<ResultResource> SaveMeasurementUnitsAsync([FromBody] MeasurementUnitResource resource)
        {
            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var MeasurementUnits = _mapper.Map<MeasurementUnitResource, MeasurementUnits>(resource);
            var result2 = await _measurementUnitService.SaveMeasurementUnitsAsync(MeasurementUnits);


            if (!result2.Success)
            {
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var measurementunitresource = _mapper.Map<MeasurementUnits, MeasurementUnitResource>(result2.Obj);

            DataGridTable measurementUnitLst = await _measurementUnitService.GetMeasurementUnitGridAsync();

            result.ResultObject = measurementUnitLst;
            return result;

        }

        [HttpPost, Route("UpdateMeasurementUnitsAsync/{id}")]
        public async Task<ResultResource> UpdateMeasurementUnitsAsync(string id, [FromBody] MeasurementUnitResource resource)
        {

            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var MeasurementUnits = _mapper.Map<MeasurementUnitResource, MeasurementUnits>(resource);
            var result2 = await _measurementUnitService.UpdateMeasurementUnitsAsync(id, MeasurementUnits);


            if (!result2.Success)
            {
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var measurementunitresource = _mapper.Map<MeasurementUnits, MeasurementUnitResource>(result2.Obj);

            DataGridTable measurementUnitLst = await _measurementUnitService.GetMeasurementUnitGridAsync();

            result.ResultObject = measurementUnitLst;
            return result;

        }

        [HttpPost, Route("DeleteMeasurementUnitsAsync/{id}")]

        public async Task<IActionResult> DeleteMeasurementUnitsAsync(string id, [FromBody] MeasurementUnitResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var measurementnits = _mapper.Map<MeasurementUnitResource, MeasurementUnits>(resource);
            var result = await _measurementUnitService.DeleteMeasurementUnitsAsync(id, measurementnits);


            if (!result.Success)
                return BadRequest(result.Message);

            var taxresource = _mapper.Map<MeasurementUnits, MeasurementUnitResource>(result.Obj);
            return Ok(taxresource = _mapper.Map<MeasurementUnits, MeasurementUnitResource>(result.Obj));

        }
    }
}
