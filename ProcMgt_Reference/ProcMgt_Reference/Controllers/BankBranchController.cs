using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
    public class BankBranchController : ControllerBase
    {
        private readonly IBankBranchServices _bankbranchServices;
        private readonly IMapper _mapper;

        public BankBranchController(IBankBranchServices bankbranchService, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._bankbranchServices = bankbranchService;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetAllBankBranch")]
        public async Task<IEnumerable<BankBranchResource>> GetAllBankBranch()
        {


            var bankbranch = await _bankbranchServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<BankBranch>, IEnumerable<BankBranchResource>>(bankbranch);

            return resources;

        }
    }
       
}
