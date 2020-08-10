     using ProcMgt_Reference_Core;
using ProcMgt_Reference_Core.GenericRepoInter;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Communication;
using ProcMgt_Reference_Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Implementations
{
    public class RoleMenuService : IRoleMenuServices
    {
        private IGenericRepo<RoleMenu> _repository = null;
        private IUnitOfWorks _unitOfWork;

        public RoleMenuService(IGenericRepo<RoleMenu> repository, IUnitOfWorks unitfwork)
        {
            this._repository = repository;
            this._unitOfWork = unitfwork;
        }

        public async Task<IEnumerable<RoleMenu>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<IEnumerable<int>> GetRoleMenuAsync(string id)

        {
            try
            {

                var gu = Guid.Parse(id);
                IEnumerable<int> menuIdslst = (await _repository.GetAll()).Where(a => a.UserRoleId == Guid.Parse(id)).Select(b => b.MenuId).ToList();

                return menuIdslst;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<GenericSaveResponse<RoleMenu>> SaveRoleMenuAsync(RoleMenu rolemenu)
        {

            try
            {
                //var rolemenus = (await _repository.GetAll()).Where(c => c.UserRoleId == rolemenu.UserRoleId).Select(b => b.RoleMenuId).ToList();

                //if (status == "exist")
                //{

                //    foreach (var delete in rolemenus)
                //    {
                //        _repository.Delete(delete);
                //        status = "existdelete";
                //    }

                //}

                await _repository.InsertAsync(rolemenu);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<RoleMenu>(true, "Successfully saved.", rolemenu);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<RoleMenu>($"An error occured when saving the RoleMenu:" + (ex.Message ?? ex.InnerException.Message));
            }
        }



        public async Task<GenericSaveResponse<RoleMenu>> Delete(RoleMenu rolemenu)
        {
            try
            {

                var rolemenus = (await _repository.GetAll()).Where(c => c.UserRoleId == rolemenu.UserRoleId).Select(b => b.RoleMenuId).ToList();

                if (rolemenus == null)
                    return new GenericSaveResponse<RoleMenu>($"RoleMenu not found");

                else

                    foreach (var delete in rolemenus)
                    {
                        _repository.Delete(delete);

                    }

                await _unitOfWork.CompleteAsync();
                return new GenericSaveResponse<RoleMenu>(true, "Successfully deleted existing records and saved", rolemenu);
            }

            catch (Exception ex)
            {

                return new GenericSaveResponse<RoleMenu>($"An error occured when delete the RoleMenu:" + (ex.Message ?? ex.InnerException.Message));

            }


        }

    }
}
