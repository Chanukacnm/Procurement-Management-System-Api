using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Core.GenericRepoInter
{
    public interface IGenericRepo<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetByIdAsync(object id);
        Task<T> InsertAsync(T obj);
        void Update(T obj);
        void Delete(object id);
     }

}