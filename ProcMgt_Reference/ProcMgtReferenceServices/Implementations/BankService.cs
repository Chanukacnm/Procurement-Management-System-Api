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
    public class BankService : IBankServices
    {
        private IGenericRepo<Bank> _repository = null;
        private IUnitOfWorks _unitOfWork;

        public BankService(IGenericRepo<Bank> repository, IUnitOfWorks unitfwork)
        {
            this._repository = repository;
            this._unitOfWork = unitfwork;
        }

        public async Task<IEnumerable<Bank>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<GenericSaveResponse<Bank>> SaveBankAsync(Bank bank)
        {
            try
            {
                await _repository.InsertAsync(bank);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<Bank>(bank);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Bank>($"An error occured when saving the Bank :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<Bank>> UpdateBankAsync(string id, Bank bank)
        {
            try
            {
                Bank existingBank = await _repository.GetByIdAsync(bank.BankId);

                if (existingBank == null)
                    return new GenericSaveResponse<Bank>($"Bank not found");

                ResourceComparer<Bank> Comparer = new ResourceComparer<Bank>(bank, existingBank);
                ResourceComparerResult<Bank> CompareResult = Comparer.GetUpdatedObject();

                if (CompareResult.Updated)
                {
                    _repository.Update(CompareResult.Obj);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<Bank>(bank);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Bank>($"An error occured when updating the Bank :" + (ex.Message ?? ex.InnerException.Message));
            }


        }
    }
}
