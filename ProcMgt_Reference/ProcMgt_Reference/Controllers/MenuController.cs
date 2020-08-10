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

    public class MenuController : ControllerBase
    {
        private readonly IMenuServices _menuServices;
        private readonly IMapper _mapper;

        public MenuController(IMenuServices menuServices, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._menuServices = menuServices ;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetAllMenuAsync")]
        public async Task<IEnumerable<MenuResource>> GetAllMenuAsync()
        {


            var menu = await _menuServices.GetAllMenuAsync();
            var resources = _mapper.Map<IEnumerable<Menu>, IEnumerable<MenuResource>>(menu);

            return resources;

        }
    }
}
