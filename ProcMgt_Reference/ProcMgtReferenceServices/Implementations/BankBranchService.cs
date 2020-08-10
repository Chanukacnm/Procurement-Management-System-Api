using ProcMgt_Reference_Core;
using ProcMgt_Reference_Core.GenericRepoInter;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Services;
using ProcMgt_Reference_Services.Communication;
using ProcMgt_Reference_Services.Helpers;
using ProcMgt_Reference_Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Implementations
{
    public class BankBranchService : IBankBranchServices
    {
        private IGenericRepo<BankBranch> _repository = null;
        private IUnitOfWorks _unitOfWork;

        public BankBranchService(IGenericRepo<BankBranch> repository, IUnitOfWorks unitfwork)
        {
            this._repository = repository;
            this._unitOfWork = unitfwork;
        }

        public async Task<IEnumerable<BankBranch>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<GenericSaveResponse<BankBranch>> SaveBankBranchAsync(BankBranch bankbranch)
        {
            try
            {
                await _repository.InsertAsync(bankbranch);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<BankBranch>(bankbranch);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<BankBranch>($"An error occured when saving the Branch :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<BankBranch>> UpdateBankBranchAsync(string id, BankBranch bankbranch)
        {
            try
            {
                BankBranch existingBankBranch = await _repository.GetByIdAsync(bankbranch.BranchId);

                if (existingBankBranch == null)
                    return new GenericSaveResponse<BankBranch>($"Company not found");

                ResourceComparer<BankBranch> Comparer = new ResourceComparer<BankBranch>(bankbranch, existingBankBranch);
                ResourceComparerResult<BankBranch> CompareResult = Comparer.GetUpdatedObject();

                if (CompareResult.Updated)
                {
                    _repository.Update(CompareResult.Obj);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<BankBranch>(bankbranch);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<BankBranch>($"An error occured when updating the Branch :" + (ex.Message ?? ex.InnerException.Message));
            }


        }
    }
}
