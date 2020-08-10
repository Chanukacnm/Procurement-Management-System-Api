using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference.Extensions;
using ProcMgt_Reference_Services;
using ProcMgt_Reference_Services.Interfaces;

namespace ProcMgt_Reference.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyServices _companyServices;
        private readonly IMapper _mapper;

        public CompanyController(ICompanyServices companyService, IMapper mapper)// IGenericRepo<User> repo)
        {
            this._companyServices = companyService;
            this._mapper = mapper;
        }
       
        [HttpGet, Route("GetAllCompany")]
        public async Task<IEnumerable<CompanyResource>> GetAllCompany()
        {
           

            var company = await _companyServices.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<Company>, IEnumerable<CompanyResource>>(company);

            return resources;

        }

        [HttpGet, Route("GetCompanyrGrid")]
        public async Task<DataGridTable> GetCompanyrGrid()
        {
            DataGridTable companyLst = await _companyServices.GetCompanyGridAsync();
            return companyLst;

        }

        [HttpPost, Route("GetGroupCompanyrGrid/{id}")]
        public async Task<DataGridTable> GetGroupCompanyrGrid(string id, [FromBody] CompanyResource resource)
        {
            var grpCompanyDetails = _mapper.Map<CompanyResource, Company>(resource);
            DataGridTable grpCompanyLst = await _companyServices.getGroupCompanyGridAsync(id, grpCompanyDetails);
            return grpCompanyLst;

        }

        [HttpPost, Route("SaveCompanyAsync")]
        public async Task<ResultResource> SaveCompanyAsync([FromBody] CompanyResource resource)
        {

            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var company = _mapper.Map<CompanyResource, Company>(resource);
            var result2 = await _companyServices.SaveCompanyAsync(company);


            if (!result2.Success)
            {
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            var companyresource = _mapper.Map<Company, CompanyResource>(result2.Obj);

            DataGridTable companyLst = await _companyServices.GetCompanyGridAsync();

            result.ResultObject = companyLst;
            return result;

        }

        [HttpPost, Route("UpdateCompanyAsync/{id}")]

        public async Task<ResultResource> UpdateCompanyAsync(string id, [FromBody] CompanyResource resource)
        {

            ResultResource result = new ResultResource { status = true };

            if (!ModelState.IsValid)
            {
                result.Message = ModelState.GetErrorMessages().FirstOrDefault();
                result.status = false;
                return result;
            }

            var company = _mapper.Map<CompanyResource, Company>(resource);
            var result2 = await _companyServices.UpdateCompanyAsync(id, company);


            if (!result2.Success)
            {
                result.Message = result2.Message;
                result.status = false;
                return result;
            }

            if (resource.IsGroupofCompany == true)
            {
                var result3 = await _companyServices.UpdateCompanyGroupCompanyAsync(id, company);

                if (!result3.Success)
                {
                    result.Message = result3.Message;
                    result.status = false;
                    return result;
                }
            }

            var companyresource = _mapper.Map<Company, CompanyResource>(result2.Obj);

            DataGridTable companyLst = await _companyServices.GetCompanyGridAsync();

            result.ResultObject = companyLst;
            return result;

        }



    }
}

