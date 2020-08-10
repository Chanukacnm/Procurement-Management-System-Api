using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface IConatcDetailsServices
    {
        Task<IEnumerable<ContactDetails>> GetAllAsync();
        Task<DataGridTable> getContactList(string id, ContactDetails contactdetails);
        Task<GenericSaveResponse<ContactDetails>> SaveContactDetailsAsync(ContactDetails contactdetails);
        Task<GenericSaveResponse<ContactDetails>> UpdateContactDetailsAsync(string id, ContactDetails contactdetails);

        //Task<DataGridTable> GetContactDetailsGridAsync();

    }
}
