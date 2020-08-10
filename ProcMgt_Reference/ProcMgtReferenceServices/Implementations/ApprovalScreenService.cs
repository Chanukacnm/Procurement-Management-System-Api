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

namespace ProcMgt_Reference_Services
{
    public class ApprovalScreenService : IApprovalScreenServices
    {
        private IGenericRepo<ItemRequest> _repository = null;
        private IUnitOfWorks _unitOfWork;
        private IGenericRepo<Approver> _approverrepository = null;
        private IGenericRepo<Stock> _stockrepository = null;
        private IGenericRepo<Category> _categoryrepository = null;
        private IGenericRepo<Item> _itemrepository = null;
        private IGenericRepo<Priority> _priorityrepository = null;
        private IGenericRepo<ItemType> _itemtyperepository = null;
        private IGenericRepo<Model> _modelrepository = null;
        private IGenericRepo<Make> _makerepository = null;
        private IGenericRepo<User> _userrepository = null;
        private IGenericRepo<UserRole> _userrolerepository = null;
        private IGenericRepo<Department> _departmentrepository = null;

        public ApprovalScreenService(IGenericRepo<ItemRequest> repository, IGenericRepo<Approver> approverrepository, IGenericRepo<Stock> stockrepository,
               IGenericRepo<Category> categoryrepository,
               IGenericRepo<Item> itemrepository, IGenericRepo<UserRole> userrolerepository,
               IGenericRepo<Priority> priorityrepository,
               IGenericRepo<ItemType> itemtyperepository, IGenericRepo<Model> modelrepository,
               IGenericRepo<Make> makerepository, IGenericRepo<User> userrepository, IGenericRepo<Department> departmentrepository,
               IUnitOfWorks unitfwork)
        {
            this._repository = repository;
            this._unitOfWork = unitfwork;
            this._stockrepository = stockrepository;
            this._approverrepository = approverrepository;
            this._departmentrepository = departmentrepository;
            this._categoryrepository = categoryrepository;
            this._itemrepository = itemrepository;
            this._itemtyperepository = itemtyperepository;
            this._makerepository = makerepository;
            this._modelrepository = modelrepository;
            this._userrolerepository = userrolerepository;
            this._priorityrepository = priorityrepository;
            this._userrepository = userrepository;
        }

        public async Task<IEnumerable<ItemRequest>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<DataGridTable> GetApprovalScreenGridAsync(User user)

        {
            try
            {
                var approvalScreenList = (await _repository.GetAll()).Select(a => new ApprovalScreenResource()
                {


                    ItemRequestID = a.ItemRequestId,
                    RequestTitle = a.RequestTitle,
                    CategoryID = a.CategoryId,
                    MakeID = a.MakeId == null ? null : a.MakeId,
                    PriorityID = a.PriorityId,
                    IsReplaceble = a.IsReplaceble,
                    AssetCode = a.AssetCode,
                    Remark = a.Remark,
                    ItemTypeID = a.ItemTypeId,
                    ItemID = a.ItemId,
                    ModelID = a.ModelId == null ? null : a.ModelId,
                    RequiredDate = a.RequiredDate,
                    NoOfUnits = a.NoOfUnits,
                    ApproverID = a.ApproverId,
                    Attachment = a.Attachment,
                    IsApproved = a.IsApproved,
                    IsIssued = a.IsIssued,
                    IsRejected = a.IsRejected,
                    Status = a.IsIssued == true ? "Issued" : a.IsApproved == true ? "Approved" : a.IsRejected == true ? "Rejected" : "Pending",
                    ApprovedDateTime = a.ApprovedDateTime.HasValue ? a.ApprovedDateTime : Convert.ToDateTime("1900-01-01"),
                    ApprovalComment = a.ApprovalComment == null ? a.ApprovalComment : Convert.ToString(""),
                    RequestedDateTime = a.RequestedDateTime,
                    RequestedUserID = a.RequestedUserId, 
                    DepartmentID = a.DepartmentId,
                    ApprovedUserID = a.ApprovedUserId == null ? null : a.ApprovedUserId,
                    UpdatedRequestedDateTime = a.UpdatedRequestedDateTime.HasValue ? a.UpdatedRequestedDateTime : Convert.ToDateTime("1900-01-01"),

                    CategoryName = _categoryrepository.GetByIdAsync(a.CategoryId).Result.CategoryName.ToString(),
                    ItemDescription = _itemrepository.GetByIdAsync(a.ItemId).Result.ItemDescription.ToString(),
                    MakeName = a.MakeId == null ? "" : _makerepository.GetByIdAsync(a.MakeId).Result.MakeName.ToString(),
                    PriorityLevelName = _priorityrepository.GetByIdAsync(a.PriorityId).Result.PriorityLevelName.ToString(),
                    ItemTypeName = _itemtyperepository.GetByIdAsync(a.ItemTypeId).Result.ItemTypeName.ToString(),
                    ModelName = a.ModelId == null ? "" : _modelrepository.GetByIdAsync(a.ModelId).Result.ModelName.ToString(),
                    ApproverName = a.ApproverId == null ? "" : _approverrepository.GetByIdAsync(a.ApproverId).Result.ApproverName.ToString(),
                    ApprovedUserName = a.ApprovedUserId == null ? "" : _userrepository.GetByIdAsync(a.ApprovedUserId).Result.UserName.ToString(),
                    RequestedUserName = _userrepository.GetByIdAsync(a.RequestedUserId).Result.UserName.ToString(),
                    DepartmentName = _departmentrepository.GetByIdAsync(a.DepartmentId).Result.DepartmentName.ToString(),
                    Code = _userrolerepository.GetByIdAsync(user.UserRoleId).Result.Code.ToString(),
                    //BalancedQty = _itemrepository.GetByIdAsync(a.ItemId).Result.InitialQty.ToString()
                    //BalancedQty = 0
                    //BalancedQty = Convert.ToDouble(_itemrepository.GetByIdAsync(a.ItemId).Result.InitialQty)
                    //BalancedQty =  _stockrepository.GetByIdAsync(a.ItemId).Result.BalancedQty



                }).Where(x => (x.DepartmentID == user.DepartmentId) && ((x.IsApproved == false) && (x.IsRejected == false))
                &&((x.Code == "DIR") || (x.Code == "EXD") || (x.Code == "EX") || (x.Code == "SRE") || (x.Code == "CEO") || (x.Code == "ADM") || (x.Code == "SMA") || (x.Code == "MAN") ||
                 (x.Code == "PROH"))).OrderByDescending(h => h.RequestedDateTime).ToList();

                var qty = (await _stockrepository.GetAll()).ToList();

                //var dictionaryList2 = new List<object>();
                //List<ApprovalScreenResource> w = new List<ApprovalScreenResource>;
                //w = approvalScreenList;
                
                 foreach (var q in approvalScreenList)
                 {
               
                      var qty2 = (await _stockrepository.GetAll()).Select(c => new StockResource()
                    {
                        StockID = c.StockId,
                        ItemID = c.ItemId,
                        StockQty = c.StockQty,
                        BalancedQty = c.BalancedQty

                    }).Where(d => d.ItemID == q.ItemID).ToList();
                    q.BalancedQty = qty2.FirstOrDefault().BalancedQty;
                   // dictionaryList2.Add(qty2);
                }

                //foreach(var x in approvalScreenList)
                //{
                //    //var qty = (await _stockrepository.GetAll()).ToList();


                //      var qty = (await _stockrepository.GetAll()).Select(c => new StockResource()
                //      {
                //          StockID = c.StockId,
                //          ItemID = c.ItemId,
                //          StockQty = c.StockQty,
                //          BalancedQty = c.BalancedQty

                //      }).Where(d => d.ItemID == x.ItemID).ToList();

                //    approvalScreenList.Add(x);
                //}



                //approvalScreenList.Add(qty);
                //(user.UserRoleId == (Guid.Parse("BF9AF9F0-D3D0-4CC9-A53E-013F9BA9D5F0")) || user.UserRoleId == (Guid.Parse("34B80700-42A0-4851-BA99-2D92BD690DBC")) ||
                // user.UserRoleId == (Guid.Parse("32D0364F-987B-453C-A8E4-41272DBF2826")) || user.UserRoleId == (Guid.Parse("20AE09A8-C76A-4D7C-B8A3-B1A8ABBBFC7D")))

                DataTable dtApprovalScreen = CommonGenericService<ItemRequest>.ToDataTable(approvalScreenList);

                var dataTable = new DataGridTable
                {
                    rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                    enableSorting = true,
                    enableColResize = false,
                    DataGridColumns = GetApprovalScreenColumnsfromList(dtApprovalScreen),
                    DataGridRows = GetApprovalScreenRowsFromList(dtApprovalScreen)
                };

                return dataTable;
            }
            catch (Exception ex)
            {
                return null;
            }



        }

        private List<DataGridColumn> GetApprovalScreenColumnsfromList(DataTable dataTable)
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

                if (column.ToString().Equals("RequestTitle"))
                {
                    dataTableColumn.headerName = "Request Title";
                    dataTableColumn.width = 115;
                }
                if (column.ToString().Equals("RequiredDate"))
                {
                    dataTableColumn.headerName = "Required Date ";
                    dataTableColumn.width = 125;
                }
                if (column.ToString().Equals("BalancedQty"))
                {
                    dataTableColumn.headerName = "Balanced Qty ";
                    dataTableColumn.width = 115;
                }
                if (column.ToString().Equals("NoOfUnits"))
                {
                    dataTableColumn.headerName = "No of Units ";
                    dataTableColumn.width = 105;
                }
                if (column.ToString().Equals("RequestedDateTime"))
                {
                    dataTableColumn.headerName = "Requested Date Time";
                    dataTableColumn.width = 160;
                }
                if (column.ToString().Equals("CategoryName"))
                {
                    dataTableColumn.headerName = "Category Name";
                    dataTableColumn.width = 130;
                }
                if (column.ToString().Equals("ItemTypeName"))
                {
                    dataTableColumn.headerName = "Item Type Name";
                    dataTableColumn.width = 130;
                }
                if (column.ToString().Equals("ItemDescription"))
                {
                    dataTableColumn.headerName = "Item Description";
                    dataTableColumn.width = 135;
                }
                if (column.ToString().Equals("PriorityLevelName"))
                {
                    dataTableColumn.headerName = "Priority Level Name";
                    dataTableColumn.width = 150;
                }
                if (column.ToString().Equals("RequestedUserName"))
                {
                    dataTableColumn.headerName = "Requested User Name";
                    dataTableColumn.width = 165;
                }
                if (column.ToString().Equals("DepartmentName"))
                {
                    dataTableColumn.headerName = "Department Name";
                    dataTableColumn.width = 145;
                }
                if (column.ToString().Equals("Status"))
                {
                    dataTableColumn.headerName = "Status";
                    dataTableColumn.width = 90;
                }

                if (!column.ToString().Equals("ItemRequestID")
                    && !column.ToString().Equals("CategoryID")
                     && !column.ToString().Equals("MakeID")
                      && !column.ToString().Equals("MakeName")
                      && !column.ToString().Equals("DepartmentName")
                      && !column.ToString().Equals("PriorityID")
                       && !column.ToString().Equals("ItemTypeID")
                       && !column.ToString().Equals("Attachment")
                        && !column.ToString().Equals("ModelID")
                        && !column.ToString().Equals("ModelName")
                          && !column.ToString().Equals("ApproverName")
                          && !column.ToString().Equals("ReceivedQty")
                           && !column.ToString().Equals("ItemID")
                           && !column.ToString().Equals("ApproverID")
                           && !column.ToString().Equals("ApprovedUserID")
                           && !column.ToString().Equals("IsIssued")
                           && !column.ToString().Equals("ApprovedUserName")
                            && !column.ToString().Equals("IsReplaceble")
                            && !column.ToString().Equals("Code")
                             && !column.ToString().Equals("Remark")
                             && !column.ToString().Equals("IsApproved")
                             && !column.ToString().Equals("IsRejected")
                             && !column.ToString().Equals("AssetCode")
                              && !column.ToString().Equals("ApprovalComment")
                               && !column.ToString().Equals("RequestedUserID")
                                && !column.ToString().Equals("DepartmentID")
                                 && !column.ToString().Equals("UpdatedRequestedDateTime")
                                  && !column.ToString().Equals("ApprovedDateTime"))
                               

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

        private static List<Object> GetApprovalScreenRowsFromList(DataTable dataTable)
        {
            var dictionaryList = new List<object>();
            foreach (DataRow row in dataTable.Rows)
            {
                var dictionary = new Dictionary<string, string>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    var rowValue = row[column].ToString();
                    if (column.ToString().Equals("RequestedDateTime"))
                    {
                        rowValue = Convert.ToDateTime(rowValue).ToString();
                    }
                    else if (column.DataType.ToString() == "System.DateTime")
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
