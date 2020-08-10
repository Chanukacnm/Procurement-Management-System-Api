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
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
    public class DepartmentService: IDepartmentServices
    {
        private IGenericRepo<Department> _repository = null;
        private IUnitOfWorks _unitOfWork;
        private IGenericRepo<Company> _companyrepository = null;
        private IGenericRepo<User> _userrepository = null; //--- Add By Nipuna Francisku

        public DepartmentService(IGenericRepo<Department> repository, IGenericRepo<Company> companyrepository, IUnitOfWorks unitfwork, IGenericRepo<User> userrepository)
        {
            this._repository = repository;
            this._companyrepository = companyrepository;
            this._unitOfWork = unitfwork;
            this._userrepository = userrepository; //--- Add By Nipuna Francisku
        }
        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<IEnumerable<Department>> GetAllSpecAsync(string id, Department department)
        {
            var getDepartmentList = (await _repository.GetAll()).Select(c => new Department
            {
                DepartmentId = c.DepartmentId,
                DepartmentName = c.DepartmentName,
                Code = c.Code,
                IsActive = c.IsActive.HasValue ? c.IsActive.Value : false,
                CompanyId = c.CompanyId

            }).Where(d => d.IsActive == true && department.CompanyId == d.CompanyId).OrderBy(e => e.DepartmentName).ToList();
            return getDepartmentList;
            //return await _repository.GetAll();
        }

        public async Task<DataGridTable> GetDepartmentGridAsync()
        {
            var departmentList = (await _repository.GetAll()).Select(a => new DepartmentResource
            {
                DepartmentID = a.DepartmentId,
                CompanyID = a.CompanyId,
                DepartmentName = a.DepartmentName,
                Code = a.Code,
                IsActive = a.IsActive.HasValue ? a.IsActive.Value : false,
                Status = a.IsActive == true ? "Active" : "Inactive",
                CompanyName = _companyrepository.GetByIdAsync(a.CompanyId).Result.CompanyName.ToString(),

            }).OrderBy(b => b.DepartmentName).ToList();

            //--------- Add By Nipuna Francisku --------------------------------
            foreach(var q in departmentList)
            {
                var userList = (await _userrepository.GetAll()).Select(b => new UserResource()
                {
                    UserID = b.UserId,
                    DepartmentID = b.DepartmentId
                }).Where(d => d.DepartmentID == q.DepartmentID).ToList();

                if(userList.Count !=0)
                {
                    q.IsTansactions = true;
                }
                else
                {
                    q.IsTansactions = false;
                }
            }
            //-------------------------------------------------------------------

            //CompanyID = a.CompanyId.HasValue ? a.CompanyId.Value : Guid.Empty,

            //var departmentlstOrder = departmentList.OrderBy(a => a.DepartmentName);

            DataTable dtDepartment = CommonGenericService<DepartmentResource>.ToDataTable(departmentList);

            var dataTable = new DataGridTable
            {
                rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                enableSorting = true,
                enableColResize = false,
                DataGridColumns = GetDepartmentColumnsfromList(dtDepartment),
                DataGridRows = GetDeprtmentRowsFromList(dtDepartment)
            };

            return dataTable;
        }

        private List<DataGridColumn> GetDepartmentColumnsfromList(DataTable dataTable)
        {
            var DataGridColumns = new List<DataGridColumn>();

            foreach (DataColumn column in dataTable.Columns)
            {
                var dataTableColumn = new DataGridColumn
                {
                    field = column.ToString().Replace(" ", "_"),
                    headerName = column.ToString(),
                    
                };

                if (column.ToString().Equals("DepartmentName"))
                {
                    dataTableColumn.headerName = "Department Name";
                    //dataTableColumn.cellStyle = "background-color";
                }

                if (column.ToString().Equals("CompanyName"))
                {
                    dataTableColumn.headerName = "Company Name";
                    dataTableColumn.width = 350;
                }
                           
                if (column.ToString().Equals("Code"))
                {
                    dataTableColumn.headerName = "Code";
                }

                if (column.ToString().Equals("Status"))
                {
                    dataTableColumn.headerName = "Status";
                }

                if (!column.ToString().Equals("DepartmentID")
                    && !column.ToString().Equals("CompanyID")
                    && !column.ToString().Equals("IsActive")
                    && !column.ToString().Equals("Company")
                    && !column.ToString().Equals("User")
                    && !column.ToString().Equals("IsTansactions") //--------- Add By Nipuna Francisku 
                    && !column.ToString().Equals("ItemRequest"))
                {
                    dataTableColumn.hide = false;
                }
                else
                {
                    dataTableColumn.hide = true;
                }
              
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

        private static List<Object> GetDeprtmentRowsFromList(DataTable dataTable)
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


        public async Task<GenericSaveResponse<Department>> SaveDepartmentAsync(Department department)
        {
            try
            {
                if (department.DepartmentId == Guid.Empty)
                {
                    department.DepartmentId = Guid.NewGuid();
                }

                var departmentNameList = (await _repository.GetAll()).Where(d => d.CompanyId == department.CompanyId && d.DepartmentName == department.DepartmentName).ToList();

                if (departmentNameList.Count != 0)
                {
                    return new GenericSaveResponse<Department>($"Department Name record already exists. Please Reenter");
                }

                //var departmentCodeList = (await _repository.GetAll()).Where(d => d.CompanyId == department.CompanyId && d.DepartmentName == department.DepartmentName && d.Code == department.Code).ToList();

                //if (departmentCodeList.Count != 0)
                //{
                //    return new GenericSaveResponse<Department>($"Department Code already exists. Please Reenter");
                //}

                var departmentCodeList = (await _repository.GetAll()).Where(d => d.CompanyId == department.CompanyId && d.Code == department.Code).ToList();

                if (departmentCodeList.Count != 0)
                {
                    return new GenericSaveResponse<Department>($"Department Code already exists. Please Reenter");
                }



                await _repository.InsertAsync(department);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<Department>(true,"Successfully Saved.",department);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Department>($"An error occured when saving the Department:" + (ex.Message ?? ex.InnerException.Message));
            }
        }
        public async Task<GenericSaveResponse<Department>> UpdateDepartmentAsync(Department department)
        {
            try
            {
                Department existingDepartment = await _repository.GetByIdAsync(department.DepartmentId);

                if (existingDepartment == null)
                    return new GenericSaveResponse<Department>($"Department not found");

                var departmentNameList = (await _repository.GetAll()).Where(d => d.CompanyId == department.CompanyId && d.DepartmentName == department.DepartmentName && d.DepartmentId != existingDepartment.DepartmentId).ToList();

                if (departmentNameList.Count != 0)
                {
                    return new GenericSaveResponse<Department>($"Department Name record already exists. Please Reenter");
                }

                var departmentCodeList = (await _repository.GetAll()).Where(d => d.CompanyId == department.CompanyId && d.Code == department.Code && d.DepartmentId != existingDepartment.DepartmentId).ToList();

                if (departmentCodeList.Count != 0)
                {
                    return new GenericSaveResponse<Department>($"Department Code already exists. Please Reenter");
                }

                ResourceComparer<Department> Comparer = new ResourceComparer<Department>(department, existingDepartment);
                ResourceComparerResult<Department> CompareResult = Comparer.GetUpdatedObject();

                if (CompareResult.Updated)
                {
                    _repository.Update(CompareResult.Obj);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<Department>(department);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Department>($"An error occured when updating the Department :" + (ex.Message ?? ex.InnerException.Message));
            }




        }

        public async Task<GenericSaveResponse<Department>> DeleteDepartmentAsync(Department department, string id)
        {
            try
            {
                Department existingDepartment = await _repository.GetByIdAsync(department.DepartmentId);

                if (existingDepartment == null)
                    return new GenericSaveResponse<Department>($"Department not found");

                else

                    _repository.Delete(department.DepartmentId);
                await _unitOfWork.CompleteAsync();
                return new GenericSaveResponse<Department>(department);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Department>($"An error occured when deleting the Department: " + (ex.Message ?? ex.InnerException.Message));
            }
        }

    }
}
