using ProcMgt_Reference_Core;
using ProcMgt_Reference_Core.GenericRepoInter;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Common;
using ProcMgt_Reference_Services.Communication;
using ProcMgt_Reference_Services.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
    public class ApproverService: IApproverServices
    {
        private IGenericRepo<Approver> _repository = null;
        private IUnitOfWorks _unitOfWork;

        public ApproverService(IGenericRepo<Approver> repository, IUnitOfWorks unitfwork)
        {
            this._repository = repository;
            this._unitOfWork = unitfwork;
        }
        public async Task<IEnumerable<Approver>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<GenericSaveResponse<Approver>> SaveApproverAsync(Approver approver)
        {
            try
            {
                if (approver.ApproverId == Guid.Empty)
                {
                    approver.ApproverId = Guid.NewGuid();
                }

                await _repository.InsertAsync(approver);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<Approver>(true, "Successfully Saved.", approver);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Approver>($"An error occured when saving the Approver:" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<Approver>> UpdateApproverAsync(Approver approver)
        {
            try
            {
                Approver existingApprover = await _repository.GetByIdAsync(approver.ApproverId);

                if (existingApprover == null)
                    return new GenericSaveResponse<Approver>($"Approver not found");

                ResourceComparer<Approver> Comparer = new ResourceComparer<Approver>(approver, existingApprover);
                ResourceComparerResult<Approver> CompareResult = Comparer.GetUpdatedObject();

                if (CompareResult.Updated)
                {
                    _repository.Update(CompareResult.Obj);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<Approver>(approver);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Approver>($"An error occured when updating the Approver :" + (ex.Message ?? ex.InnerException.Message));
            }

        }

        public async Task<GenericSaveResponse<Approver>> DeleteApproverAsync(Approver approver, string id)
        {
            try
            {
                Approver existingApprover = await _repository.GetByIdAsync(approver.ApproverId);

                if (existingApprover == null)
                    return new GenericSaveResponse<Approver>($"Approver not found");

                else

                    _repository.Delete(approver.ApproverId);
                await _unitOfWork.CompleteAsync();
                return new GenericSaveResponse<Approver>(approver);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Approver>($"An error occured when deleting the Approver: " + (ex.Message ?? ex.InnerException.Message));
            }
        }
    }
}
