using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProcMgt_Reference.Extensions;
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
    public class ContactDetailsController : BaseApiController
    {
        private readonly IConatcDetailsServices _contactDetailsServices;
        private readonly IMapper _mapper;


        public ContactDetailsController(IConatcDetailsServices contactDetailsService, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._contactDetailsServices = contactDetailsService;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetAllContactDetails")]
        public async Task<IEnumerable<ContactDetailsResource>> GetAllContactDetails()
        {
            var contactdetails = await _contactDetailsServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<ContactDetails>, IEnumerable<ContactDetailsResource>>(contactdetails);

            return resources;
             
        }

        //[HttpPost, Route("ContactDetailsList/{id}")]
        //public async Task<ResultResource> ContactDetailsList(string id, [FromBody]ContactDetailsResource resource)
        //{
        //    ResultResource result = new ResultResource { status = true };

        //    if (!ModelState.IsValid)
        //    {
        //        result.Message = ModelState.GetErrorMessages().FirstOrDefault();
        //        result.status = false;
        //        return result;
        //    }

        //    var contactList = _mapper.Map<ContactDetailsResource, ContactDetails>(resource);
        //    var result2 = await _contactDetailsServices.getContactList(id, contactList);

        //    var contactDetresource = _mapper.Map<IEnumerable<ContactDetails>, IEnumerable<ContactDetailsResource>>(result2);
        //    result.ResultObject = contactDetresource;
        //    return result;
        //}

        [HttpPost, Route("ContactDetailsList/{id}")]
        public async Task<DataGridTable> ContactDetailsList(string id, [FromBody] ContactDetailsResource resource)
        {
            var contactDetails = _mapper.Map<ContactDetailsResource, ContactDetails>(resource);
            DataGridTable contactDetailsList = await _contactDetailsServices.getContactList(id, contactDetails);
            return contactDetailsList;

        }

        //[HttpGet, Route("GetContactDetailsGrid")]
        //public async Task<DataGridTable> GetContactDetailsGrid()
        //{
        //    DataGridTable contactdetailsLst = await _contactDetailsServices.GetContactDetailsGridAsync();
        //    return contactdetailsLst;

        //}

        [HttpPost, Route("SaveContactDetailsAsync")]
        public async Task<IActionResult> SaveContactDetailsAsync([FromBody] ContactDetailsResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var contactdetails = _mapper.Map<ContactDetailsResource, ContactDetails>(resource);
            var result = await _contactDetailsServices.SaveContactDetailsAsync(contactdetails);

            if (!result.Success)
                return BadRequest(result.Message);

            var contactdetaiklsrresource = _mapper.Map<ContactDetails, ContactDetailsResource>(result.Obj);
            return Ok(contactdetaiklsrresource);

        }

    }
}
