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
    public class SupplierTypeService : ISupplierTypeService
    {
        private IGenericRepo<SupplierType> _repository = null;
        private IUnitOfWorks _unitOfWork;

        public SupplierTypeService(IGenericRepo<SupplierType> repository, IUnitOfWorks unitfwork)
        {
            this._repository = repository;
            this._unitOfWork = unitfwork;
        }

        public async Task<IEnumerable<SupplierType>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<GenericSaveResponse<SupplierType>> SaveSupplierTypeAsync(SupplierType suppliertype)
        {
            try
            {
                await _repository.InsertAsync(suppliertype);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<SupplierType>(suppliertype);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<SupplierType>($"An error occured when saving the Category Master :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<SupplierType>> UpdateSupplierTypeAsync(string id, SupplierType suppliertype)
        {
            try
            {
                SupplierType existingSupllierType = await _repository.GetByIdAsync(suppliertype.SupplierTypeId);

                if (existingSupllierType == null)
                    return new GenericSaveResponse<SupplierType>($"Measurement Units not found");

                ResourceComparer<SupplierType> Comparer = new ResourceComparer<SupplierType>(suppliertype, existingSupllierType);
                ResourceComparerResult<SupplierType> CompareResult = Comparer.GetUpdatedObject();

                if (CompareResult.Updated)
                {
                    _repository.Update(CompareResult.Obj);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<SupplierType>(suppliertype);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<SupplierType>($"An error occured when updating the Measurement Units :" + (ex.Message ?? ex.InnerException.Message));
            }


        }
    }
}
