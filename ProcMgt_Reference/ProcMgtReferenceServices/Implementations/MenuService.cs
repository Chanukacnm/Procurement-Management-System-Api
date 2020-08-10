using ProcMgt_Reference_Core;
using ProcMgt_Reference_Core.GenericRepoInter;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Implementations
{
   public class MenuService : IMenuServices
    {

        private IGenericRepo<Menu> _menurepository = null;
        private IUnitOfWorks _unitOfWork;

        public MenuService(IGenericRepo<Menu> menurepository, IUnitOfWorks unitfwork)
        {
            this._menurepository = menurepository;
            this._unitOfWork = unitfwork;
        }

        public async Task<IEnumerable<Menu>> GetAllMenuAsync()
        {
            return await _menurepository.GetAll();
        }
    }
}
