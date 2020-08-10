using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Core
{
    public interface IUnitOfWorks
    {
        Task CompleteAsync();
    }
}
