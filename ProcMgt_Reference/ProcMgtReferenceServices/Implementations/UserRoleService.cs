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
   public class UserRoleService : IUserRoleServices
    {
        private IGenericRepo<UserRole> _repository = null;
        private IUnitOfWorks _unitOfWork;
        private IGenericRepo<User> _userrepository = null; //--- Add By Nipuna Francisku

        public UserRoleService(IGenericRepo<UserRole> repository, IUnitOfWorks unitfwork, IGenericRepo<User> userrepository)
        {
            this._repository = repository;
            this._unitOfWork = unitfwork;
            this._userrepository = userrepository; //--- Add By Nipuna Francisku
        }

        public async Task<IEnumerable<UserRole>> GetAllAsync()
        {

            var isactiveuserrole = (await _repository.GetAll()).Where(a => a.IsActive.Equals(true)).ToList();
            return  isactiveuserrole;

    }

        public async Task<DataGridTable> GetUserRoleGridAsync()
        {
            //var userRoleList = await _repository.GetAll();
            var userRoleList = (await _repository.GetAll()).Select(a => new UserRoleResource
            {
                UserRoleID = a.UserRoleId,
                UserRoleName = a.UserRoleName,
                Code = a.Code,
                IsActive = a.IsActive.HasValue ? a.IsActive.Value : false,
                Status = a.IsActive == true ? "Active" : "Inactive"
            }).OrderBy(b => b.UserRoleName).ToList();

            //--------- Add By Nipuna Francisku --------------------------------
            foreach (var q in userRoleList)
            {
                var userList = (await _userrepository.GetAll()).Select(b => new UserResource()
                {
                    UserID = b.UserId,
                    UserRoleID = b.UserRoleId
                }).Where(d => d.UserRoleID == q.UserRoleID).ToList();

                if (userList.Count != 0)
                {
                    q.IsTansactions = true;
                }
                else
                {
                    q.IsTansactions = false;
                }
            }
            //-------------------------------------------------------------------

            DataTable dtUserRole = CommonGenericService<UserRole>.ToDataTable(userRoleList);

            var dataTable = new DataGridTable
            {
                rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                enableSorting = true,
                enableColResize = false,
                suppressSizeToFit = true,
                DataGridColumns = GetUserRoleColumnsfromList(dtUserRole),
                DataGridRows = GetUserRoleRowsFromList(dtUserRole)
            };

            return dataTable;
        }

        private List<DataGridColumn> GetUserRoleColumnsfromList(DataTable dataTable)
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
                

                if (column.ToString().Equals("UserRoleName"))
                {
                    dataTableColumn.headerName = "User Role Name";
                }
                if (column.ToString().Equals("Code"))
                {
                    dataTableColumn.headerName = "Code";
                }
                 
                 if (column.ToString().Equals("Status"))
                {
                    dataTableColumn.headerName = "Status";
                }

                if (!column.ToString().Equals("UserRoleID")
                   && !column.ToString().Equals("ApprovalFlowManagement")
                   && !column.ToString().Equals("User")
                    && !column.ToString().Equals("Accers")
                    && !column.ToString().Equals("IsActive")
                    && !column.ToString().Equals("RoleMenu")
                    && !column.ToString().Equals("IsTansactions") //--------- Add By Nipuna Francisku 
                    && !column.ToString().Equals("UserRoleAccers"))
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

        private static List<Object> GetUserRoleRowsFromList(DataTable dataTable)
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

        public async Task<GenericSaveResponse<UserRole>> SaveUserRoleAsync(UserRole userrole)
        {
            try
            {
                if (userrole.UserRoleId == Guid.Empty)
                {
                    userrole.UserRoleId = Guid.NewGuid();
                }

                var getallUserRole = (await _repository.GetAll()).Where(d => d.UserRoleName == userrole.UserRoleName).ToList();

                if (getallUserRole.Count != 0)
                {
                    return new GenericSaveResponse<UserRole>($"User Role Name already exists. Please Reenter");
                }

                var getallUserRoleCode = (await _repository.GetAll()).Where(d => d.Code == userrole.Code).ToList();

                if (getallUserRoleCode.Count != 0)
                {
                    return new GenericSaveResponse<UserRole>($"User Role Code already exists. Please Reenter");
                }

                await _repository.InsertAsync(userrole);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<UserRole>(true, "Successfully Saved.", userrole);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<UserRole>($"An error occured when saving the User Role :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<UserRole>> UpdateUserRoleAsync(string id, UserRole userrole)
        {
            try
            {
                UserRole existingUserRole = await _repository.GetByIdAsync(userrole.UserRoleId);

                if (existingUserRole == null)
                    return new GenericSaveResponse<UserRole>($"User Role not found");

                var getallUserRole = (await _repository.GetAll()).Where(d => d.UserRoleName == userrole.UserRoleName && d.UserRoleId != existingUserRole.UserRoleId).ToList();

                if (getallUserRole.Count != 0)
                {
                    return new GenericSaveResponse<UserRole>($"User Role Name already exists. Please Reenter");
                }

                var getallUserRoleCode = (await _repository.GetAll()).Where(d => d.Code == userrole.Code && d.UserRoleId != existingUserRole.UserRoleId).ToList();

                if (getallUserRoleCode.Count != 0)
                {
                    return new GenericSaveResponse<UserRole>($"User Role Code already exists. Please Reenter");
                }

                ResourceComparer<UserRole> Comparer = new ResourceComparer<UserRole>(userrole, existingUserRole);
                ResourceComparerResult<UserRole> CompareResult = Comparer.GetUpdatedObject();

                if (CompareResult.Updated)
                {
                    _repository.Update(CompareResult.Obj);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<UserRole>(userrole);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<UserRole>($"An error occured when updating the User Role :" + (ex.Message ?? ex.InnerException.Message));
            }


        }

        public async Task<GenericSaveResponse<UserRole>> DeleteUserRoleAsync(string id, UserRole userrole)
        {
            try
            {
                UserRole existingUserRole = await _repository.GetByIdAsync(userrole.UserRoleId);

                if (existingUserRole == null)
                {
                    return new GenericSaveResponse<UserRole>($"User Role not found");
                }
                else
                { 
                
                    _repository.Delete(userrole.UserRoleId);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<UserRole>(userrole);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<UserRole>($"An error occured when deleting the User Role :" + (ex.Message ?? ex.InnerException.Message));
            }


        }
    }
}
