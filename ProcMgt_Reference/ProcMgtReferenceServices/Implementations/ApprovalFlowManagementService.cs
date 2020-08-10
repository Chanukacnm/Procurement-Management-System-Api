using ProcMgt_Reference_Core;
using ProcMgt_Reference_Core.GenericRepoInter;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Common;
using ProcMgt_Reference_Services.Communication;
using ProcMgt_Reference_Services.Helpers;
using ProcMgt_Reference_Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Implementations
{
    public class ApprovalFlowManagementService : IApprovalFlowManagementServices
    {
        private IGenericRepo<ApprovalFlowManagement> _repository = null;
        private IGenericRepo<UserRole> _userrolerepository = null;
        private IGenericRepo<ApprovalPatternType> _approvalpatterntyperepository = null;
        private IGenericRepo<Designation> _designationrepository = null;
        private IUnitOfWorks _unitOfWork;
         
        public ApprovalFlowManagementService(IGenericRepo<ApprovalFlowManagement> repository, IGenericRepo<Designation> designationrepository, IGenericRepo<ApprovalPatternType> approvalpatterntyperepository, IGenericRepo<UserRole> userrolerepository, IUnitOfWorks unitfwork)
        {
            this._repository = repository;
            this._unitOfWork = unitfwork;
            this._designationrepository = designationrepository;
            this._approvalpatterntyperepository = approvalpatterntyperepository;
            this._userrolerepository = userrolerepository;
        }

        public async Task<IEnumerable<ApprovalFlowManagement>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<DataGridTable> GetApprovalFlowManagementGridAsync()
        {
            //var approvalFlowManagementList = await _repository.GetAll();

            var approvalFlowManagementList = (await _repository.GetAll()).Select(a => new ApprovalFlowManagementResource()
            {
                ApprovalFlowManagementID = a.ApprovalFlowManagementId,
                ApprovalPatternTypeID = a.ApprovalPatternTypeId,
                ApprovalSequenceNo = a.ApprovalSequenceNo,
                DesignationID = a.DesignationId, 

                ApprovalPatternName = _approvalpatterntyperepository.GetByIdAsync(a.ApprovalPatternTypeId).Result.PatternName.ToString(),
                DesignationName = _designationrepository.GetByIdAsync(a.DesignationId).Result.DesignationName.ToString(),
            }).OrderBy(d => d.ApprovalPatternName).ToList();

            DataTable dtApprovalFlowManagement = CommonGenericService<ApprovalFlowManagement>.ToDataTable(approvalFlowManagementList);

            var dataTable = new DataGridTable
            {
                rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                enableSorting = true,
                enableColResize = false,
                suppressSizeToFit = true,
                DataGridColumns = GetApprovalFlowManagementColumnsfromList(dtApprovalFlowManagement),
                DataGridRows = GetApprovalFlowManagementRowsFromList(dtApprovalFlowManagement)
            };

            return dataTable;
        }

        private List<DataGridColumn> GetApprovalFlowManagementColumnsfromList(DataTable dataTable)
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

                if (column.ToString().Equals("ApprovalSequenceNo"))
                {
                    dataTableColumn.width = 170;
                    dataTableColumn.headerName = "Approval Sequence No";
                }
                if (column.ToString().Equals("ApprovalPatternName"))
                {
                    dataTableColumn.width = 170;
                    dataTableColumn.headerName = "Approval Pattern Name";
                }
                if (column.ToString().Equals("DesignationName"))
                {
                    dataTableColumn.headerName = "Designation Name";
                    dataTableColumn.width = 145;
                }


                if (!column.ToString().Equals("ApprovalFlowManagementID")
                    && !column.ToString().Equals("ApprovalPatternTypeID")
                     && !column.ToString().Equals("DesignationID"))
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

        private static List<Object> GetApprovalFlowManagementRowsFromList(DataTable dataTable)
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

        public async Task<GenericSaveResponse<ApprovalFlowManagement>> SaveApprovalFlowManagementAsync(ApprovalFlowManagement approvalflowmanagement)
        {
            try
            {
                if (approvalflowmanagement.ApprovalFlowManagementId == Guid.Empty)
                {
                    approvalflowmanagement.ApprovalFlowManagementId = Guid.NewGuid();
                }

                await _repository.InsertAsync(approvalflowmanagement);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<ApprovalFlowManagement>(true, "Successfully Saved.", approvalflowmanagement);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<ApprovalFlowManagement>($"An error occured when saving the Approval Flow Management :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<ApprovalFlowManagement>> UpdateApprovalFlowManagementAsync(string id, ApprovalFlowManagement approvalflowmanagement)
        {
            try
            {
                ApprovalFlowManagement existingApprovalFlowManagement = await _repository.GetByIdAsync(approvalflowmanagement.ApprovalFlowManagementId);

                if (existingApprovalFlowManagement == null)
                    return new GenericSaveResponse<ApprovalFlowManagement>($"Approval Flow Management not found");

                ResourceComparer<ApprovalFlowManagement> Comparer = new ResourceComparer<ApprovalFlowManagement>(approvalflowmanagement, existingApprovalFlowManagement);
                ResourceComparerResult<ApprovalFlowManagement> CompareResult = Comparer.GetUpdatedObject();

                if (CompareResult.Updated)
                {
                    _repository.Update(CompareResult.Obj);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<ApprovalFlowManagement>(approvalflowmanagement);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<ApprovalFlowManagement>($"An error occured when updating the Approval Flow Management :" + (ex.Message ?? ex.InnerException.Message));
            }


        }

        public async Task<GenericSaveResponse<ApprovalFlowManagement>> DeleteApprovalFlowManagementAsync(string id, ApprovalFlowManagement approvalflowmanagement)
        {
            try
            {
                ApprovalFlowManagement existingApprovalFlowManagement = await _repository.GetByIdAsync(approvalflowmanagement.ApprovalFlowManagementId);

                if (existingApprovalFlowManagement == null)
                {
                    return new GenericSaveResponse<ApprovalFlowManagement>($"Approval Flow Management not found");
                }
                else
                {
                    _repository.Delete(approvalflowmanagement.ApprovalFlowManagementId);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<ApprovalFlowManagement>(approvalflowmanagement);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<ApprovalFlowManagement>($"An error occured when Deleting the Approval Flow Management :" + (ex.Message ?? ex.InnerException.Message));
            }


        }


    }
}
