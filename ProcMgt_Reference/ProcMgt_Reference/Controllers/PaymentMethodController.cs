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
//  below temp code for report testing : Chathura 
using System.Collections.Generic;
//using Microsoft.Reporting.WebForms;
using System.Web;
using Microsoft.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting.Server;


namespace ProcMgt_Reference.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodServices _paymentMethodServices;
        private readonly IItemRequestServices _itemRequestServices;

        private readonly IMapper _mapper;


        // Reporting

        


        public PaymentMethodController(IPaymentMethodServices paymentmethodservice, IItemRequestServices itemRequestServices, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._paymentMethodServices = paymentmethodservice;
            this._itemRequestServices = itemRequestServices;
            this._mapper = mapper;
        }

        [HttpGet, Route("GetPaymentMethodGrid")]

        public async Task<DataGridTable> GetPaymentMethodGrid()
        {
            DataGridTable paymentmethodLst = await _paymentMethodServices.GetPaymentMethodGridAsync();
            return paymentmethodLst;


        }

        [HttpGet, Route("GetPaymentMethodByID/{id}")]

        public async Task<IEnumerable<PaymentMethodResource>> GetPaymentMethodByID(int id)
        {
            var paymentmethod = await _paymentMethodServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<PaymentMethod>, IEnumerable<PaymentMethodResource>>(paymentmethod);

            return resources;

        }

        [HttpGet, Route("GetAllPaymentMethod")]
        public async Task<IEnumerable<PaymentMethodResource>> GetAllPaymentMethod(int id)
        {
            var paymentmethod = await _paymentMethodServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<PaymentMethod>, IEnumerable<PaymentMethodResource>>(paymentmethod);

            return resources;

        }

        [HttpPost, Route("SavePaymentMethodAsync")]

        public async Task<ResultResource> SavePaymentMethodAsync([FromBody] PaymentMethodResource resource)
        {

            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var paymentmethod = _mapper.Map<PaymentMethodResource, PaymentMethod>(resource);
            var result2 = await _paymentMethodServices.SavePaymentMethodAsync(paymentmethod);


            if (!result2.Success)
            {
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var paymentmethodresource = _mapper.Map<PaymentMethod, PaymentMethodResource>(result2.Obj);

            DataGridTable paymentmethodLst = await _paymentMethodServices.GetPaymentMethodGridAsync();

            result.ResultObject = paymentmethodLst;
            return result;

        }

        [HttpPost, Route("UpdatePaymentMethodAsync/{id}")]

        public async Task<ResultResource> UpdatePaymentMethodAsync(string id, [FromBody] PaymentMethodResource resource)
        {

            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var paymentmethod = _mapper.Map<PaymentMethodResource, PaymentMethod>(resource);
            var result2 = await _paymentMethodServices.UpdatePaymentMethodAsync(id, paymentmethod);


            if (!result2.Success)
            {
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var paymentmethodresource = _mapper.Map<PaymentMethod, PaymentMethodResource>(result2.Obj);
            DataGridTable paymentmethodLst = await _paymentMethodServices.GetPaymentMethodGridAsync();

            result.ResultObject = paymentmethodLst;
            return result;
        }


        [HttpPost, Route("DeletePaymentMethodAsync/{id}")]

        public async Task<IActionResult> DeletePaymentMethodAsync(string id, [FromBody] PaymentMethodResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var paymentmethod = _mapper.Map<PaymentMethodResource, PaymentMethod>(resource);
            var result = await _paymentMethodServices.DeletePaymentMethodAsync(paymentmethod, id);


            if (!result.Success)
                return BadRequest(result.Message);

            var paymentmethodresource = _mapper.Map<PaymentMethod, PaymentMethodResource>(result.Obj);
            return Ok(paymentmethodresource = _mapper.Map<PaymentMethod, PaymentMethodResource>(result.Obj));

        }

    }
}
