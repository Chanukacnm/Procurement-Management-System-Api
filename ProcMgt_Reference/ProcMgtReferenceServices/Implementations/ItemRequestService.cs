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
    public class ItemRequestService : IItemRequestServices
    {
        private IGenericRepo<ItemRequest> _repository = null;
        private IUnitOfWorks _unitOfWork;
        private IGenericRepo<Approver> _approverrepository = null;
        private IGenericRepo<Category> _categoryrepository = null;
        private IGenericRepo<Stock> _stockrepository = null;
        private IGenericRepo<Item> _itemrepository = null;
        private IGenericRepo<Priority> _priorityrepository = null;
        private IGenericRepo<ItemType> _itemtyperepository = null;
        private IGenericRepo<Model> _modelrepository = null;
        private IGenericRepo<Make> _makerepository = null;
        private IGenericRepo<User> _userrepository = null;
        private IGenericRepo<Department> _departmentrepository = null;
        private IGenericRepo<IssueHeader> _issuerepository = null;

        public ItemRequestService(IGenericRepo<ItemRequest> repository, IGenericRepo<Approver> approverrepository,
               IGenericRepo<Category> categoryrepository,
               IGenericRepo<Item> itemrepository, IGenericRepo<Stock> stockrepository,
               IGenericRepo<Priority> priorityrepository,
               IGenericRepo<ItemType> itemtyperepository, IGenericRepo<Model> modelrepository, 
               IGenericRepo<Make> makerepository, IGenericRepo<User> userrepository, IGenericRepo<Department> departmentrepository,
               IGenericRepo<IssueHeader> issuerepository, IUnitOfWorks unitfwork)
        {

            this._repository = repository;
            this._unitOfWork = unitfwork;
            this._issuerepository = issuerepository;
            this._stockrepository = stockrepository;
            this._approverrepository = approverrepository;
            this._departmentrepository = departmentrepository;
            this._categoryrepository = categoryrepository;
            this._itemrepository = itemrepository;
            this._itemtyperepository = itemtyperepository;
            this._makerepository = makerepository;
            this._modelrepository = modelrepository;
            this._priorityrepository = priorityrepository;
            this._userrepository = userrepository;
        }

        public async Task<IEnumerable<ItemRequest>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<DataGridTable> GetItemRequestGridAsync(ItemRequestResource itemrequest)
           
        {
            
            try{
                var itemRequestList = (await _repository.GetAll()).Select(a => new ItemRequestResource()
                {


                    ItemRequestID = a.ItemRequestId,
                    RequestTitle = a.RequestTitle,
                    CategoryID = a.CategoryId,
                    MakeID = a.MakeId == null ? null : a.MakeId,
                    ItemID =a.ItemId,
                    PriorityID = a.PriorityId,
                    IsReplaceble = a.IsReplaceble,
                    AssetCode = a.AssetCode,
                    Remark = a.Remark,
                    ItemTypeID = a.ItemTypeId,
                    ModelID = a.ModelId == null ? null : a.ModelId,
                    RequiredDate = a.RequiredDate,
                    NoOfUnits = a.NoOfUnits,
                    ApproverID = a.ApproverId,
                    Attachment = a.Attachment,
                    IsApproved = a.IsApproved,
                    IsIssued = a.IsIssued,
                    IsRejected = a.IsRejected,
                    Status = a.IsIssued ==true ? "Issued" : a.IsApproved ==true ? "Approved" : a.IsRejected == true ? "Rejected" : "Pending",
                    ApprovedDateTime = a.ApprovedDateTime.HasValue ? a.ApprovedDateTime : Convert.ToDateTime("1900-01-01"),
                    ApprovalComment = a.ApprovalComment == null ? a.ApprovalComment : Convert.ToString(""),
                    RequestedDateTime = a.RequestedDateTime,
                    RequestedUserID = a.RequestedUserId,
                    DepartmentID = a.DepartmentId,
                    IssuedUserId = a.IssuedUserId,
                    ApprovedUserID = a.ApprovedUserId == null ? null : a.ApprovedUserId, 
                    UpdatedRequestedDateTime = a.UpdatedRequestedDateTime.HasValue ? a.UpdatedRequestedDateTime: Convert.ToDateTime("1900-01-01"),

                    CategoryName = _categoryrepository.GetByIdAsync(a.CategoryId).Result.CategoryName.ToString(),
                    ItemDescription = _itemrepository.GetByIdAsync(a.ItemId).Result.ItemDescription.ToString(),
                    MakeName = a.MakeId == null ? "" : _makerepository.GetByIdAsync(a.MakeId).Result.MakeName.ToString(),
                    PriorityLevelName = _priorityrepository.GetByIdAsync(a.PriorityId).Result.PriorityLevelName.ToString(),
                    ItemTypeName = _itemtyperepository.GetByIdAsync(a.ItemTypeId).Result.ItemTypeName.ToString(),
                    ModelName = a.ModelId == null ? "" :_modelrepository.GetByIdAsync(a.ModelId).Result.ModelName.ToString(),
                    ApproverName = a.ApproverId == null ? "" : _approverrepository.GetByIdAsync(a.ApproverId).Result.ApproverName.ToString(),
                    ApprovedUserName = a.ApprovedUserId == null ? "" : _userrepository.GetByIdAsync(a.ApprovedUserId).Result.UserName.ToString(),
                    RequestedUserName = _userrepository.GetByIdAsync(a.RequestedUserId).Result.UserName.ToString(),
                    DepartmentName = _departmentrepository.GetByIdAsync(a.DepartmentId).Result.DepartmentName.ToString(),
                    IssuedUserName = a.IssuedUserId == null ? "": _userrepository.GetByIdAsync(a.IssuedUserId).Result.UserName.ToString()

                //}).OrderByDescending(x => x.RequestedDateTime).ToList(); ; // cmd by Nipuna
                }).Where(x => (x.RequestedUserID == itemrequest.UserId)).OrderByDescending(x => x.RequestedDateTime).ToList(); ;
                

                DataTable dtItemRequest = CommonGenericService<ItemRequest>.ToDataTable(itemRequestList);

                var dataTable = new DataGridTable
                {
                    rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                    enableSorting = true,
                    enableColResize = false,
                    DataGridColumns = GetItemRequestColumnsfromList(dtItemRequest),
                    DataGridRows = GetItemRequestRowsFromList(dtItemRequest)
                };

                return dataTable;
            }
            catch(Exception ex)
            {
                return null;
            }

     
           
        }

        private List<DataGridColumn> GetItemRequestColumnsfromList(DataTable dataTable)
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
                }
                if (column.ToString().Equals("AssetCode"))
                {
                    dataTableColumn.headerName = "Asset Code";
                }
                if (column.ToString().Equals("RequiredDate"))
                {
                    dataTableColumn.headerName = "Required Date";
                    dataTableColumn.width = 125;
                }
                if (column.ToString().Equals("NoOfUnits"))
                {
                    dataTableColumn.headerName = "No of Units";
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
                if (column.ToString().Equals("ItemDescription"))
                {
                    dataTableColumn.headerName = "Item Description";
                    dataTableColumn.width = 135;
                }
                if (column.ToString().Equals("PriorityLevelName"))
                {
                    dataTableColumn.headerName = "Priority Level Name";
                }
                if (column.ToString().Equals("RequestedUserName"))
                {
                    dataTableColumn.width = 165;
                    dataTableColumn.headerName = "Requested User Name";
                }
                if (column.ToString().Equals("DepartmentName"))
                {
                    dataTableColumn.headerName = "Department Name";
                    dataTableColumn.width = 145;
                }
                if (column.ToString().Equals("ApprovedUserName"))
                {
                    dataTableColumn.headerName = "Approved User Name";
                    dataTableColumn.width = 165;
                }
                if (column.ToString().Equals("IssuedUserName"))
                {
                    dataTableColumn.headerName = "Issued User Name";
                    dataTableColumn.width = 155;
                }
                if (column.ToString().Equals("Status"))
                {
                    dataTableColumn.headerName = "Status";
                    dataTableColumn.width = 90;
                }
                if (column.ToString().Equals("ItemTypeName"))
                {
                    dataTableColumn.headerName = "Item Type Name";
                    dataTableColumn.width = 130;
                }

                if (!column.ToString().Equals("ItemRequestID")
                    && !column.ToString().Equals("CategoryID")
                    && !column.ToString().Equals("UserId")
                     && !column.ToString().Equals("MakeID")
                      && !column.ToString().Equals("PriorityID")
                       && !column.ToString().Equals("ItemTypeID")
                       && !column.ToString().Equals("ItemID")
                        && !column.ToString().Equals("ModelID")
                        && !column.ToString().Equals("TotalItemRequests")
                        && !column.ToString().Equals("TotalApprovedItemRequests")
                       && !column.ToString().Equals("TotalRejectedItemRequests")
                        && !column.ToString().Equals("TotalPendingItemRequests")
                         && !column.ToString().Equals("ApproverID")
                          && !column.ToString().Equals("IsReplaceble")
                           && !column.ToString().Equals("Remark")
                           && !column.ToString().Equals("ApprovedUserID")
                           && !column.ToString().Equals("AssetCode")
                           && !column.ToString().Equals("PriorityLevelName")
                           && !column.ToString().Equals("Attachment")
                            && !column.ToString().Equals("ApprovedDateTime")
                             && !column.ToString().Equals("ApprovalComment")
                              && !column.ToString().Equals("IsApproved")
                               && !column.ToString().Equals("IsRejected")
                                && !column.ToString().Equals("RequestedUserID")
                                 && !column.ToString().Equals("DepartmentID")
                                  && !column.ToString().Equals("ItemCategoryName")
                                   && !column.ToString().Equals("MakeName")
                                     && !column.ToString().Equals("ModelName")
                                     && !column.ToString().Equals("IsIssued")
                                      && !column.ToString().Equals("ApproverName")
                                      && !column.ToString().Equals("IssuedUserId")
                                      && !column.ToString().Equals("BalancedQty")
                                      && !column.ToString().Equals("ReceivedQty")
                                      && !column.ToString().Equals("Status")  //Added by Nipuna Francisku
                                       && !column.ToString().Equals("UpdatedRequestedDateTime"))
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

        private static List<Object> GetItemRequestRowsFromList(DataTable dataTable)
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


        public async Task<GenericSaveResponse<ItemRequest>> SaveItemRequestAsync(ItemRequest itemrequest)
        {
            try
            {
                if (itemrequest.ItemRequestId == Guid.Empty)
                {
                    itemrequest.ItemRequestId = Guid.NewGuid();
                }

                await _repository.InsertAsync(itemrequest);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<ItemRequest>(itemrequest);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<ItemRequest>($"An error occured when saving the Item Request :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<ItemRequest>> UpdateItemRequestAsync(string id, ItemRequest itemrequest , double ReceivedQty , double BalancedQty)
        {
            try
            {
                ItemRequest existingItemrequest = await _repository.GetByIdAsync(itemrequest.ItemRequestId);

                if (existingItemrequest == null)
                    return new GenericSaveResponse<ItemRequest>($"Item Request not found");

                Stock stock = new Stock();
                //stock = await _stockrepository.GetByIdAsync(itemrequest.ItemId);
                //Item itm = new Item();
                Stock stock2 = (await _stockrepository.GetAll()).Where(a => a.ItemId == itemrequest.ItemId).ToList().FirstOrDefault();
                //itm = await _itemrepository.GetByIdAsync(itemrequest.ItemId);

                if (stock2.BalancedQty < ReceivedQty)
                {
                    return new GenericSaveResponse<ItemRequest>($"Balance Quantity is not enough to received this.!");
                }

                ResourceComparer<ItemRequest> Comparer = new ResourceComparer<ItemRequest>(itemrequest, existingItemrequest);
                ResourceComparerResult<ItemRequest> CompareResult = Comparer.GetUpdatedObject();

                if (CompareResult.Updated)
                {
                    _repository.Update(CompareResult.Obj);
                    
                }

                
                
                stock2.ReceivedQty = stock2.ReceivedQty + ReceivedQty;
                stock2.BalancedQty = stock2.BalancedQty - ReceivedQty;

                //stock.ReceivedQty = ReceivedQty;
                //stock.BalancedQty = stock.BalancedQty - stock.ReceivedQty;

                _stockrepository.Update(stock2);

                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<ItemRequest>(itemrequest);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<ItemRequest>($"An error occured when updating the Item Request:" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<ItemRequest>> DeleteItemRequestAsync(string id, ItemRequest itemrequest)
        {
            try
            {
                ItemRequest existingItemRequest = await _repository.GetByIdAsync(itemrequest.ItemRequestId);

                if (existingItemRequest == null)
                    return new GenericSaveResponse<ItemRequest>($"Item Request not found");

                else

                    _repository.Delete(itemrequest.ItemRequestId);
                await _unitOfWork.CompleteAsync();
                return new GenericSaveResponse<ItemRequest>(itemrequest);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<ItemRequest>($"An error occured when deleting the Item Request: " + (ex.Message ?? ex.InnerException.Message));
            }
        }
    }
}
