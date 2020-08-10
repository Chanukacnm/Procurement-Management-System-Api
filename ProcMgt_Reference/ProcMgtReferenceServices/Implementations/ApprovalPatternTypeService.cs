using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcMgt_Reference_Core;
using ProcMgt_Reference_Core.GenericRepoInter;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Common;
using ProcMgt_Reference_Services.Communication;
using ProcMgt_Reference_Services.Helpers;
using ProcMgt_Reference_Services.Interfaces;

namespace ProcMgt_Reference_Services.Implementations
{
    public class ApprovalPatternTypeService : IApprovalPatternTypeServices
    {
        private IGenericRepo<ApprovalPatternType> _repository = null;
        private IUnitOfWorks _unitOfWork;

        public ApprovalPatternTypeService(IGenericRepo<ApprovalPatternType> repository, IUnitOfWorks unitfwork)
        {
            this._repository = repository;
            this._unitOfWork = unitfwork;
        }

        public async Task<IEnumerable<ApprovalPatternType>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<DataGridTable> GetApprovalPatternTypeGridAsync()
        {
            //var approvalPatternTypeList = await _repository.GetAll();

            var approvalPatternTypeList = (await _repository.GetAll()).Select(a => new ApprovalPatternTypeResource
            {
                ApprovalPatternTypeID = a.ApprovalPatternTypeId,
                Code = a.Code,
                PatternName = a.PatternName,
                IsActive = a.IsActive.HasValue ? a.IsActive.Value : false,
                Status = a.IsActive == true ? "Active" : "Inactive"
            }).OrderBy(b => b.PatternName);

            DataTable dtApprovalPatternType = CommonGenericService<ApprovalPatternType>.ToDataTable(approvalPatternTypeList);

            var dataTable = new DataGridTable
            {
                rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                enableSorting = true,
                enableColResize = false,
                suppressSizeToFit = true,
                DataGridColumns = GetApprovalPatternTypeColumnsfromList(dtApprovalPatternType),
                DataGridRows = GetApprovalPatternTypeRowsFromList(dtApprovalPatternType)
            };

            return dataTable;
        }

        private List<DataGridColumn> GetApprovalPatternTypeColumnsfromList(DataTable dataTable)
        {

            var DataGridColumns = new List<DataGridColumn>();

            //foreach (Department depItem in departmentList)
            // {
            //     DataGridColumn dataGridColumn = new DataGridColumn();
            //     dataGridColumn. nameof(depItem.CompanyId)
            // }
            foreach (DataColumn column in dataTable.Columns)
            {
                var dataTableColumn = new DataGridColumn
                {
                    field = column.ToString().Replace(" ", "_"),
                    headerName = column.ToString(),
                    //visible = 

                    //halign = DataAlignEnum.Center.ToString().ToLower()
                };

                if (column.ToString().Equals("PatternName"))
                {
                    dataTableColumn.headerName =  "Pattern Name";
                }

                if (column.ToString().Equals("Code"))
                {
                    dataTableColumn.headerName = "Code";
                }
                           
                  if (column.ToString().Equals("Status"))
                {
                    dataTableColumn.headerName = "Status";
                }

                if (!column.ToString().Equals("ApprovalPatternTypeID")
                    && !column.ToString().Equals("ItemType")
                    && !column.ToString().Equals("ApprovalFlowManagement")
                    && !column.ToString().Equals("IsActive")
                     && !column.ToString().Equals("Accers"))
                {
                    dataTableColumn.hide = false;
                }
                else
                {
                    dataTableColumn.hide = true;
                }

                // agNumberColumnFilter

                switch (column.DataType.ToString())
                {
                    case "System.Decimal":
                        dataTableColumn.type = "numericColumn";
                        dataTableColumn.filter = "agNumberColumnFilter";
                        break;
                    case "System.Int32":
                        dataTableColumn.type = "numericColumn";
                        dataTableColumn.filter = "agNumberColumnFilter";
                        break;
                    case "System.Int64":
                        dataTableColumn.type = "numericColumn";
                        dataTableColumn.filter = "agNumberColumnFilter";
                        break;
                    case "System.DateTime":
                        dataTableColumn.type = "dateColumn";
                        dataTableColumn.filter = "agDateColumnFilter";
                        break;
                    default:
                        //dataTableColumn.type = "agTextColumnFilter";
                        dataTableColumn.filter = "agTextColumnFilter";
                        break;
                }

                DataGridColumns.Add(dataTableColumn);
            }
            return DataGridColumns;
        }

        private static List<Object> GetApprovalPatternTypeRowsFromList(DataTable dataTable)
        {
            var dictionaryList = new List<object>();
            foreach (DataRow row in dataTable.Rows)
            {
                var dictionary = new Dictionary<string, string>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    var rowValue = row[column].ToString();
                    if (column.DataType.ToString() == "System.DateTime")
                    {
                        //rowValue = "<span style='display:none'>" + Convert.ToDateTime(rowValue).ToString("u", CultureInfo.CurrentCulture) + "</span>" + Convert.ToDateTime(rowValue).ToString("d", CultureInfo.CurrentCulture);
                        rowValue = Convert.ToDateTime(rowValue).ToString("d", CultureInfo.CurrentCulture);
                    }

                    dictionary.Add(column.ToString().Replace(" ", "_"), rowValue);
                }
                dictionaryList.Add(dictionary);
            }
            return dictionaryList;
        }

        public async Task<GenericSaveResponse<ApprovalPatternType>> SaveApprovalPatternTypeAsync(ApprovalPatternType approvalpatterntype)
        {
            try
            {
                if (approvalpatterntype.ApprovalPatternTypeId == Guid.Empty)
                {
                    approvalpatterntype.ApprovalPatternTypeId = Guid.NewGuid();
                }

                var getallApprovalPatternTypeName = (await _repository.GetAll()).Where(d => d.PatternName == approvalpatterntype.PatternName ).ToList();

                if (getallApprovalPatternTypeName.Count != 0)
                {
                    return new GenericSaveResponse<ApprovalPatternType>($"Pattern Name already exists. Please ReEnter");
                }
                var getallApprovalPatternTypeCode = (await _repository.GetAll()).Where(d => d.Code == approvalpatterntype.Code).ToList();

                if (getallApprovalPatternTypeCode.Count != 0)
                {
                    return new GenericSaveResponse<ApprovalPatternType>($"Pattern Name already exists. Please ReEnter");
                }


                await _repository.InsertAsync(approvalpatterntype);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<ApprovalPatternType>(true, "Successfully Saved.", approvalpatterntype);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<ApprovalPatternType>($"An error occured when saving the Approval Pattern Type :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<ApprovalPatternType>> UpdateApprovalPatternTypeAsync(string id, ApprovalPatternType approvalpatterntype)
        {
            try
            {
                ApprovalPatternType existingApprovalPatternType = await _repository.GetByIdAsync(approvalpatterntype.ApprovalPatternTypeId);

                if (existingApprovalPatternType == null)
                    return new GenericSaveResponse<ApprovalPatternType>($"Approval Pattern Type not found");

                var getallApprovalPatternTypeName = (await _repository.GetAll()).
                    Where(d => d.PatternName == approvalpatterntype.PatternName && approvalpatterntype.PatternName != existingApprovalPatternType.PatternName).ToList();

                if (getallApprovalPatternTypeName.Count != 0)
                {
                    return new GenericSaveResponse<ApprovalPatternType>($"Pattern Name already exists. Please Reenter");
                }

                var getallApprovalPatternTypeCode = (await _repository.GetAll()).
                    Where(d => d.Code == approvalpatterntype.Code && approvalpatterntype.Code != existingApprovalPatternType.Code).ToList();

                if (getallApprovalPatternTypeCode.Count != 0)
                {
                    return new GenericSaveResponse<ApprovalPatternType>($"Pattern Name already exists. Please Reenter");
                }

                ResourceComparer<ApprovalPatternType> Comparer = new ResourceComparer<ApprovalPatternType>(approvalpatterntype, existingApprovalPatternType);
                ResourceComparerResult<ApprovalPatternType> CompareResult = Comparer.GetUpdatedObject();

                if (CompareResult.Updated)
                {
                    _repository.Update(CompareResult.Obj);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<ApprovalPatternType>(approvalpatterntype);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<ApprovalPatternType>($"An error occured when updating the Approval Pattern Type :" + (ex.Message ?? ex.InnerException.Message));
            }


        }

        public async Task<GenericSaveResponse<ApprovalPatternType>> DeleteApprovalPatternTypeAsync(string id, ApprovalPatternType approvalpatterntype)
        {
            try
            {
                ApprovalPatternType existingApprovalPatternType = await _repository.GetByIdAsync(approvalpatterntype.ApprovalPatternTypeId);

                if (existingApprovalPatternType == null)
                { 
                    return new GenericSaveResponse<ApprovalPatternType>($"Approval Pattern Type not found");
                }
                else
                {
                    _repository.Delete(approvalpatterntype.ApprovalPatternTypeId);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<ApprovalPatternType>(approvalpatterntype);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<ApprovalPatternType>($"An error occured when Deleting the Approval Pattern Type :" + (ex.Message ?? ex.InnerException.Message));
            }


        }



    }
}
