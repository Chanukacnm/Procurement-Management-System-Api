using ProcMgt_Reference_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface IBusinessUnitTypeService
    {
        Task<IEnumerable<BusinessUnitType>> GetAllBusinessUnitTypeAsync(string id, BusinessUnitType businessUnitType);
    }
}
