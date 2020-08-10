using ProcMgt_Reference_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface IBusinessUnitsService
    {
        Task<IEnumerable<BusinessUnits>> GetAllBusinessUnitsAsync(string id, BusinessUnits businessUnits);
    }
}
