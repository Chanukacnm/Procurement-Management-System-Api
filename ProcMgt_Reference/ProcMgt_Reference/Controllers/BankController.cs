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
    public class BankController : ControllerBase
    {
        private readonly IBankServices _bankServices;
        private readonly IMapper _mapper;

        public BankController(IBankServices bankService, IMapper mapper)
        {
            this._bankServices = bankService;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetAllBank")]
        public async Task<IEnumerable<BankResource>> GetAllBank()
        {


            var bank = await _bankServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<Bank>, IEnumerable<BankResource>>(bank);

            return resources;

        }

        [HttpPost, Route("SaveBankAsync")]
        public async Task<IActionResult> SaveBankAsync([FromBody] BankResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var bank = _mapper.Map<BankResource, Bank>(resource);
            var result = await _bankServices.SaveBankAsync(bank);


            if (!result.Success)
                return BadRequest(result.Message);

            var bankresource = _mapper.Map<Bank, BankResource>(result.Obj);
            return Ok(bankresource);

        }

        [HttpPut, Route("UpdateBankAsync/{id}")]

        public async Task<IActionResult> UpdateBankAsync(string id, [FromBody] BankResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var bank = _mapper.Map<BankResource, Bank>(resource);
            var result = await _bankServices.UpdateBankAsync(id, bank);


            if (!result.Success)
                return BadRequest(result.Message);

            var bankresource = _mapper.Map<Bank, BankResource>(result.Obj);
            return Ok(bankresource);

        }

    }
}
