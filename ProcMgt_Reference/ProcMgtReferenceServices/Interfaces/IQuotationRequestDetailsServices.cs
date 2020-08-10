using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface IQuotationRequestDetailsServices
    {
        Task<IEnumerable<QuotationRequestDetails>> GetAllAsync();
        Task<DataGridTable> GetQuotationRequestDetailsGrid();
    }
}
