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
    public class PriorityController : ControllerBase
    {
        private readonly IPriorityServices _priorityServices;
        private readonly IMapper _mapper;

        public PriorityController(IPriorityServices priorityService, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._priorityServices = priorityService;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetAllPriority")]
        public async Task<IEnumerable<PriorityResource>> GetAllPriority()
        {            
            var priority = await _priorityServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<Priority>, IEnumerable<PriorityResource>>(priority);

            return resources;

        }

        [HttpPost, Route("SavePriorityAsync")]
        public async Task<IActionResult> SavePriorityAsync([FromBody] PriorityResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var priority = _mapper.Map<PriorityResource, Priority>(resource);
            var result = await _priorityServices.SavePriorityAsync(priority);


            if (!result.Success)
                return BadRequest(result.Message);

            var companyresource = _mapper.Map<Priority, PriorityResource>(result.Obj);
            return Ok(companyresource);

        }

        [HttpPost, Route("UpdatePriorityAsync/{id}")]

        public async Task<IActionResult> UpdatePriorityAsync(string id, [FromBody] PriorityResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var priority = _mapper.Map<PriorityResource, Priority>(resource);
            var result = await _priorityServices.UpdatePriorityAsync(id, priority);


            if (!result.Success)
                return BadRequest(result.Message);

            var companyresource = _mapper.Map<Priority, PriorityResource>(result.Obj);
            return Ok(companyresource);

        }

    }
}
