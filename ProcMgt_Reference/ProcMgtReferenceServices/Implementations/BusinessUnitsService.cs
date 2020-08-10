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
    public class BusinessUnitsService: IBusinessUnitsService
    {
        private IGenericRepo<BusinessUnits> _repository = null;
        private IUnitOfWorks _unitOfWork;

        public BusinessUnitsService(IGenericRepo<BusinessUnits> repository, IUnitOfWorks unitfwork)
        {
            this._repository = repository;
            this._unitOfWork = unitfwork;
        }

        public async Task<IEnumerable<BusinessUnits>> GetAllBusinessUnitsAsync(string id, BusinessUnits businessUnits)
        {
            var businessunitslist = (await _repository.GetAll()).Select(a => new BusinessUnits
            {
                BusinessUnitsId = a.BusinessUnitsId,
                BusinessUnitsName = a.BusinessUnitsName,
                IsActive = a.IsActive,
                BusinessUnitTypeId = a.BusinessUnitTypeId

            }).Where(c => c.IsActive == true && businessUnits.BusinessUnitTypeId == c.BusinessUnitTypeId).OrderBy(b => b.BusinessUnitsName).ToList();

            return businessunitslist;
            //return await _repository.GetAll();
        }
    }
}
