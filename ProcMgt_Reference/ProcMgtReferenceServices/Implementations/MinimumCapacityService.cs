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
    public class MinimumCapacityService : IMinimumCapacityServices
    {
        private IGenericRepo<MinimumCapacity> _repository = null;
        private IUnitOfWorks _unitOfWork;

        public MinimumCapacityService(IGenericRepo<MinimumCapacity> repository, IUnitOfWorks unitfwork)
        {
            this._repository = repository;
            this._unitOfWork = unitfwork;
        }

        public async Task<IEnumerable<MinimumCapacity>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<GenericSaveResponse<MinimumCapacity>> SaveMinimumCapacityAsync(MinimumCapacity minimumcapacity)
        {
            try
            {
                await _repository.InsertAsync(minimumcapacity);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<MinimumCapacity>(minimumcapacity);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<MinimumCapacity>($"An error occured when saving the MinimumCapacity :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<MinimumCapacity>> UpdateMinimumCapacityAsync(string id, MinimumCapacity minimumcapacity)
        {
            try
            {
                MinimumCapacity existingMinimumCapacity = await _repository.GetByIdAsync(minimumcapacity.MinimumItemsCapacityId);

                if (existingMinimumCapacity == null)
                    return new GenericSaveResponse<MinimumCapacity>($"Company not found");

                ResourceComparer<MinimumCapacity> Comparer = new ResourceComparer<MinimumCapacity>(minimumcapacity, existingMinimumCapacity);
                ResourceComparerResult<MinimumCapacity> CompareResult = Comparer.GetUpdatedObject();

                if (CompareResult.Updated)
                {
                    _repository.Update(CompareResult.Obj);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<MinimumCapacity>(minimumcapacity);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<MinimumCapacity>($"An error occured when updating the MinimumCapacity :" + (ex.Message ?? ex.InnerException.Message));
            }


        }
    }
}
