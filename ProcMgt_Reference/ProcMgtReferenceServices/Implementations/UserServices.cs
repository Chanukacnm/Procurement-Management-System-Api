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
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace ProcMgt_Reference_Services.Implementations
{
    public class UserServices : IUserServices
    {
        private IGenericRepo<User> _repository = null;
        private IGenericRepo<Company> _companytrepository = null;
        private IGenericRepo<Department> _departmentrepository = null;
        private IGenericRepo<Designation> _designationrepository = null;
        private IGenericRepo<BusinessUnitType> _businessunittyperepository = null;
        private IGenericRepo<BusinessUnits> _businessunitsrepository = null;
        private IGenericRepo<UserRole> _userrolerepository = null;
        private IGenericRepo<DesignationBusinessUnit> _designationBusinessUnitrepository = null;
        private IUnitOfWorks _unitOfWork;
        private IGenericRepo<ItemRequest> _itemrequestrepository = null; //--- Add By Nipuna Francisku
        private IGenericRepo<QuotationRequestHeader> _quotationrequestheaderrepository = null; //--- Add By Nipuna Francisku
        private IGenericRepo<Poheader> _poheaderrepository = null; //--- Add By Nipuna Francisku

        public UserServices(IGenericRepo<User> repository, IGenericRepo<Company> companyrepository, IGenericRepo<Department> departmentrepository,
                            IGenericRepo<Designation> designationrepository, IGenericRepo<UserRole> userrolerepository,
                            IUnitOfWorks unitfwork, IGenericRepo<BusinessUnits> businessunitsrepository,  IGenericRepo<BusinessUnitType> businessunittyperepository , IGenericRepo<DesignationBusinessUnit> designationBusinessUnitrepository,  IGenericRepo<ItemRequest> itemrequestrepository, IGenericRepo<QuotationRequestHeader> quotationrequestheaderrepository, IGenericRepo<Poheader> poheaderrepository)
        {
            this._repository = repository;
            this._unitOfWork = unitfwork;
            this._companytrepository = companyrepository;
            this._departmentrepository = departmentrepository;
            this._businessunittyperepository = businessunittyperepository;
            this._businessunitsrepository = businessunitsrepository;
            this._designationrepository = designationrepository;
            this._userrolerepository = userrolerepository;
            this._designationBusinessUnitrepository = designationBusinessUnitrepository;
            this._itemrequestrepository = itemrequestrepository; //--- Add By Nipuna Francisku
            this._quotationrequestheaderrepository = quotationrequestheaderrepository; //--- Add By Nipuna Francisku
            this._poheaderrepository = poheaderrepository; //--- Add By Nipuna Francisku
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<DataGridTable> GetUserGridAsync()
        {
            //var userList = await _repository.GetAll();
            try
            {
                var userList = (await _repository.GetAll()).Select(a => new UserResource()
                {
                    UserID = a.UserId,
                    UserName = a.UserName,
                    Password = a.Password,
                    EmployeeNo = a.EmployeeNo,
                    Name = a.Name,
                    CompanyID = a.CompanyId,
                    DepartmentID = a.DepartmentId,
                    Email = a.Email,
                    DesignationID = a.DesignationId,
                    UserRoleID = a.UserRoleId,
                    IsActive = a.IsActive.HasValue ? a.IsActive.Value : false,
                    Status = a.IsActive == true ? "Active" : "Inactive",
                    DateTime = a.DateTime,

                    UserRoleName = _userrolerepository.GetByIdAsync(a.UserRoleId).Result.UserRoleName.ToString(),
                    CompanyName = _companytrepository.GetByIdAsync(a.CompanyId).Result.CompanyName.ToString(),
                    DepartmentName = _departmentrepository.GetByIdAsync(a.DepartmentId).Result.DepartmentName.ToString(),
                    DesignationName = _designationrepository.GetByIdAsync(a.DesignationId).Result.DesignationName.ToString()

                }).OrderBy(b => b.Name).ToList();

                //--------- Add By Nipuna Francisku --------------------------------
                foreach (var q in userList)
                {
                    var ItemReqList = (await _itemrequestrepository.GetAll()).Select(b => new ItemRequest() //-- Check ItemRequest
                    {
                        RequestedUserId = b.RequestedUserId,
                        ItemId = b.ItemId
                    }).Where(d => d.RequestedUserId == q.UserID).ToList();

                    var QuotationReqList = (await _quotationrequestheaderrepository.GetAll()).Select(b => new QuotationRequestHeader() //-- Check QuotationRequestHeader
                    {
                        UserId = b.UserId,
                        QuotationRequestHeaderId = b.QuotationRequestHeaderId

                    }).Where(d => d.UserId == q.UserID).ToList();

                    var PoheaderList = (await _poheaderrepository.GetAll()).Select(b => new Poheader() //-- Check Poheader
                    {
                        UserId = b.UserId,
                        PoheaderId = b.PoheaderId

                    }).Where(d => d.UserId == q.UserID).ToList();

                    if (ItemReqList.Count != 0 || QuotationReqList.Count != 0 || PoheaderList.Count != 0)
                    {
                        q.IsTansactions = true;
                    }
                    else
                    {
                        q.IsTansactions = false;
                    }
                }
                //-------------------------------------------------------------------

                DataTable dtUser = CommonGenericService<User>.ToDataTable(userList);

                var dataTable = new DataGridTable
                {
                    rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                    enableSorting = true,
                    enableColResize = false,
                    suppressSizeToFit = true,
                    DataGridColumns = GetUserColumnsfromList(dtUser),
                    DataGridRows = GetUserRowsFromList(dtUser)
                };
                return dataTable;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //private string GetCompany(Obe id)
        //{
        //    //var Companyname= (await _repository.GetByIdAsync(user.CompanyId)).Select(b => )
        //    return null;
        //}

        private List<DataGridColumn> GetUserColumnsfromList(DataTable dataTable)
        {
            var DataGridColumns = new List<DataGridColumn>();

            foreach (DataColumn column in dataTable.Columns)
            {
                var dataTableColumn = new DataGridColumn
                {
                    field = column.ToString().Replace(" ", "_"),
                    headerName = column.ToString(),
                    width = 140,
                    //visible = 

                    //halign = DataAlignEnum.Center.ToString().ToLower()
                };
                if (column.ToString().Equals("EmployeeNo"))
                {
                    dataTableColumn.headerName = "Employee No";
                    dataTableColumn.width = 115;
                }
                if (column.ToString().Equals("CompanyName"))
                {
                    dataTableColumn.headerName = "Company Name";
                    dataTableColumn.width = 130;
                }
                if (column.ToString().Equals("DepartmentName"))
                {
                    dataTableColumn.headerName = "Department Name";
                    dataTableColumn.width = 142;
                }
                if (column.ToString().Equals("DesignationName"))
                {
                    dataTableColumn.headerName = "Designation Name";
                    dataTableColumn.width = 142;
                }
                if (column.ToString().Equals("UserRoleName"))
                {
                    dataTableColumn.headerName = "User Role Name";
                    dataTableColumn.width = 128;
                }
                if (column.ToString().Equals("Status"))
                {
                    dataTableColumn.width = 75;
                }

                if (!column.ToString().Equals("UserID")
                   && !column.ToString().Equals("DepartmentID")
                   && !column.ToString().Equals("CompanyID")
                   && !column.ToString().Equals("DesignationID")
                   && !column.ToString().Equals("UserRoleID")
                   && !column.ToString().Equals("UserName")
                   && !column.ToString().Equals("IsActive")
                   && !column.ToString().Equals("DateTime")
                   && !column.ToString().Equals("IsApprovalUser")
                   && !column.ToString().Equals("ApprDesignationID")
                   && !column.ToString().Equals("IsTansactions") //--------- Add By Nipuna Francisku 
                   && !column.ToString().Equals("Password"))
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

        private static List<Object> GetUserRowsFromList(DataTable dataTable)
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

        public async Task<GenericSaveResponse<User>> SaveUserAsync(User user)
        {
            try
            {
                if (user.UserId == Guid.Empty)
                {
                    user.UserId = Guid.NewGuid();
                }

                var getallUserNames = (await _repository.GetAll()).Where(d => d.UserName == user.UserName).ToList();

                if (getallUserNames.Count != 0)
                {
                    return new GenericSaveResponse<User>($"The username already exists. Please use a different username");
                }

                var getallempNO = (await _repository.GetAll()).Where(d => d.EmployeeNo == user.EmployeeNo).ToList();
                if (getallempNO.Count != 0)
                {
                    return new GenericSaveResponse<User>($"The Employee No already exists. Please use a different Employee No");
                }

                var getallemails = (await _repository.GetAll()).Where(d => d.Email == user.Email).ToList();
                if (getallemails.Count != 0)
                {
                    return new GenericSaveResponse<User>($"The Email already exists. Please use a different Email");
                }

                await _repository.InsertAsync(user);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<User>(true, "Successfully Saved.", user);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<User>($"An error occured when saving the user :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<User>> UpdateUserAsync(string id, User user)
        {
            try
            {
                //var paswrd = await _repository.GetByIdAsync(user.Password);

                User existingUser = await _repository.GetByIdAsync(user.UserId);

                if (existingUser == null)
                {
                    return new GenericSaveResponse<User>($"User not found");
                }

                var getallempNO = (await _repository.GetAll()).Where(d => d.EmployeeNo == user.EmployeeNo && d.UserId != existingUser.UserId).ToList();
                if (getallempNO.Count != 0)
                {
                    return new GenericSaveResponse<User>($"The Employee No already exists. Please use a different Employee No");
                }

                var getallemails = (await _repository.GetAll()).Where(d => d.Email == user.Email && d.UserId != existingUser.UserId).ToList();
                if (getallemails.Count != 0)
                {
                    return new GenericSaveResponse<User>($"The Email already exists. Please use a different Email");
                }

                ResourceComparer<User> Comparer = new ResourceComparer<User>(user, existingUser);
                ResourceComparerResult<User> CompareResult = Comparer.GetUpdatedObject();

                if (CompareResult.Updated)
                {
                    _repository.Update(CompareResult.Obj);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<User>(user);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<User>($"An error occured when updating the user :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<User>> ChangePWAsync(string id, User user, string CurrentPw)
        {
            try
            {
                User existingUser = await _repository.GetByIdAsync(user.UserId);

                if (existingUser == null)
                {
                    return new GenericSaveResponse<User>($"User not found");
                }
                if (existingUser.Password != CurrentPw)
                {
                    return new GenericSaveResponse<User>($"Password is Incorrect!");
                }

                existingUser.Password = user.Password;
                _repository.Update(existingUser);
                await _unitOfWork.CompleteAsync();


                return new GenericSaveResponse<User>(existingUser);
            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<User>($"An error occured when updating the user Password :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<User>> DeleteUserAsync(string id, User user)
        {
            try
            {
                User existingUser = await _repository.GetByIdAsync(user.UserId);

                if (existingUser == null)
                {
                    return new GenericSaveResponse<User>($"User not found");
                }
                else
                {
                    _repository.Delete(user.UserId);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<User>(user);
            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<User>($"An error occured when deleting the User :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<DataGridTable> GetApprovalUserGridAsync(string id, DesignationBusinessUnit designationBusinessUnit)
        {
            var approvalUserList = (await _designationBusinessUnitrepository.GetAll()).Select(c => new DesignationBusinessUnitResource
            {
                DesignationID = c.DesignationId,
                UserId = c.UserId,
                BusinessUnitsID =c.BusinessUnitsId,
                BusinessUnitTypeID = c.BusinessUnitTypeId,
                DesignationBusinessUnitID = c.DesignationBusinessUnitId,
                DesignationName = _designationrepository.GetByIdAsync(c.DesignationId).Result.DesignationName.ToString(),
                BusinessUnitsName = _businessunitsrepository.GetByIdAsync(c.BusinessUnitsId).Result.BusinessUnitsName.ToString(),
                BusinessUnitTypeName = _businessunittyperepository.GetByIdAsync(c.BusinessUnitTypeId).Result.BusinessUnitTypeName.ToString()

            }).Where(d => d.UserId == designationBusinessUnit.UserId).ToList();

            DataTable dtApprovalUser = CommonGenericService<ContactDetails>.ToDataTable(approvalUserList);

            var dataTable = new DataGridTable
            {
                rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                enableSorting = true,
                enableColResize = false,
                suppressSizeToFit = true,
                DataGridColumns = GetApprovalUserColumnsfromList(dtApprovalUser),
                DataGridRows = GetApprovalUserRowsFromList(dtApprovalUser)
            };

            return dataTable;

        }

        private List<DataGridColumn> GetApprovalUserColumnsfromList(DataTable dataTable)
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

                if (column.ToString().Equals("BusinessUnitTypeName"))
                {
                    dataTableColumn.headerName = "Desiganation Level";
                    dataTableColumn.width = 135;
                }
                if (column.ToString().Equals("BusinessUnitsName"))
                {
                    dataTableColumn.headerName = "Business Units Name";
                    dataTableColumn.width = 135;
                }
                if (column.ToString().Equals("DesignationName"))
                {
                    dataTableColumn.headerName = "Designation Name";
                    dataTableColumn.width = 135;
                }

                if (!column.ToString().Equals("DesignationBusinessUnitID")
                    && !column.ToString().Equals("BusinessUnitTypeID")
                    && !column.ToString().Equals("DesignationID")
                    && !column.ToString().Equals("BusinessUnitsID")
                    && !column.ToString().Equals("UserId"))
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

        private static List<Object> GetApprovalUserRowsFromList(DataTable dataTable)
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

    }
}
