using ProcMgt_Reference_Core;
using ProcMgt_Reference_Core.GenericRepoInter;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Services.Communication;
using ProcMgt_Reference_Services.Helpers;
using ProcMgt_Reference_Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Implementations
{
    public class AccountTypeService : IAccountTypeServices
    {
        private IGenericRepo<AccountType> _repository = null;
        private IUnitOfWorks _unitOfWork;

        public AccountTypeService(IGenericRepo<AccountType> repository, IUnitOfWorks unitfwork)
        {
            this._repository = repository;
            this._unitOfWork = unitfwork;
        }

        public async Task<IEnumerable<AccountType>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<GenericSaveResponse<AccountType>> SaveAccountTypeAsync(AccountType accounttype)
        {
            try
            {
                await _repository.InsertAsync(accounttype);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<AccountType>(accounttype);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<AccountType>($"An error occured when saving the AccountType :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<AccountType>> UpdateAccountTypeAsync(string id, AccountType accounttype)
        {
            try
            {
                AccountType existingAccountType = await _repository.GetByIdAsync(accounttype.AccountTypeId);

                if (existingAccountType == null)
                    return new GenericSaveResponse<AccountType>($"Company not found");

                ResourceComparer<AccountType> Comparer = new ResourceComparer<AccountType>(accounttype, existingAccountType);
                ResourceComparerResult<AccountType> CompareResult = Comparer.GetUpdatedObject();

                if (CompareResult.Updated)
                {
                    _repository.Update(CompareResult.Obj);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<AccountType>(accounttype);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<AccountType>($"An error occured when updating the AccountType :" + (ex.Message ?? ex.InnerException.Message));
            }


        }
    }

    
}
