using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
    public class MinimumCapacityController : ControllerBase
    {
        private readonly IMinimumCapacityServices _minimumcapacityServices;
        private readonly IMapper _mapper;

        public MinimumCapacityController(IMinimumCapacityServices minimumcapacityService, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._minimumcapacityServices = minimumcapacityService;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetAllMinimumCapacity")]
        public async Task<IEnumerable<MinimumCapacityResource>> GetAllMinimumCapacity()
        {


            var minimumcapacity = await _minimumcapacityServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<MinimumCapacity>, IEnumerable<MinimumCapacityResource>>(minimumcapacity);

            return resources;

        }
    }
}
