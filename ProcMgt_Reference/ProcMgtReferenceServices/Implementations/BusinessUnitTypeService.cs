using ProcMgt_Reference_Core;
using ProcMgt_Reference_Core.GenericRepoInter;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Implementations
{
    public class BusinessUnitTypeService : IBusinessUnitTypeService
    {
        private IGenericRepo<BusinessUnitType> _repository = null;
        private IUnitOfWorks _unitOfWork;
   

        public BusinessUnitTypeService(IGenericRepo<BusinessUnitType> repository, IUnitOfWorks unitfwork)
        {
            this._repository = repository;
            this._unitOfWork = unitfwork;
        }

        public async Task<IEnumerable<BusinessUnitType>> GetAllBusinessUnitTypeAsync(string id, BusinessUnitType businessUnitType)
        {
            var designationlevellist = (await _repository.GetAll()).Select(a => new BusinessUnitType
            {
                BusinessUnitTypeId = a.BusinessUnitTypeId,
                BusinessUnitTypeName = a.BusinessUnitTypeName,
                IsActive = a.IsActive, 
                DesignationId = a.DesignationId

            }).Where(c => c.IsActive == true && businessUnitType.DesignationId == c.DesignationId).OrderBy(b => b.BusinessUnitTypeName).ToList();

            return designationlevellist;
            //return await _repository.GetAll();
        }

    }

}
