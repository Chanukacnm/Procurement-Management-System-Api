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
    public class AccountTypeController : ControllerBase
    {
        private readonly IAccountTypeServices _accounttypeServices;
        private readonly IMapper _mapper;

        public AccountTypeController(IAccountTypeServices accounttypeService, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._accounttypeServices = accounttypeService;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetAllAccountType")]
        public async Task<IEnumerable<AccoutTypeResource>> GetAllAccountType()
        {


            var accounttype = await _accounttypeServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<AccountType>, IEnumerable<AccoutTypeResource>>(accounttype);

            return resources;

        }
    }
}
