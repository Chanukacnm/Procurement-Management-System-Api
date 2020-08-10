using System;
using System.Collections.Generic;
using System.Text;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Services.Communication;
using System.Threading.Tasks;
using ProcMgt_Reference_Core.Resources;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface IPaymentMethodServices
    {
        Task<IEnumerable<PaymentMethod>> GetAllAsync();
        Task<GenericSaveResponse<PaymentMethod>> SavePaymentMethodAsync(PaymentMethod paymentmethod);
        Task<GenericSaveResponse<PaymentMethod>> UpdatePaymentMethodAsync(string id, PaymentMethod paymentmethod);
        Task<GenericSaveResponse<PaymentMethod>> DeletePaymentMethodAsync(PaymentMethod paymentmethod, string id);
        Task<DataGridTable> GetPaymentMethodGridAsync();
    }
}
