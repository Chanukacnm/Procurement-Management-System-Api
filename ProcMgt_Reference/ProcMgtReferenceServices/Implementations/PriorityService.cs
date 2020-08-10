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
    public class PriorityService : IPriorityServices
    {
        private IGenericRepo<Priority> _repository = null;
        private IUnitOfWorks _unitOfWork;

        public PriorityService(IGenericRepo<Priority> repository, IUnitOfWorks unitfwork)
        {
            this._repository = repository;
            this._unitOfWork = unitfwork;
        }

        public async Task<IEnumerable<Priority>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<GenericSaveResponse<Priority>> SavePriorityAsync(Priority priority)
        {
            try
            {
                await _repository.InsertAsync(priority);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<Priority>(priority);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Priority>($"An error occured when saving the Priority :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<Priority>> UpdatePriorityAsync(string id, Priority priority)
        {
            try
            {
                Priority existingPriority = await _repository.GetByIdAsync(priority.PriorityId);

                if (existingPriority == null)
                    return new GenericSaveResponse<Priority>($"Company not found");

                ResourceComparer<Priority> Comparer = new ResourceComparer<Priority>(priority, existingPriority);
                ResourceComparerResult<Priority> CompareResult = Comparer.GetUpdatedObject();

                if (CompareResult.Updated)
                {
                    _repository.Update(CompareResult.Obj);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<Priority>(priority);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Priority>($"An error occured when updating the Priority :" + (ex.Message ?? ex.InnerException.Message));
            }


        }



    }
}
