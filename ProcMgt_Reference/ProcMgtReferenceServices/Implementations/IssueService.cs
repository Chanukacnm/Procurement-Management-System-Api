using ProcMgt_Reference_Core;
using ProcMgt_Reference_Core.GenericRepoInter;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Common;
using ProcMgt_Reference_Services.Communication;
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
    public class IssueService : IIssueServices
    {
        private IGenericRepo<ItemRequest> _repository = null;
        private IGenericRepo<IssueHeader> _issuerepository = null;
        private IGenericRepo<IssueDetails> _issuedetrepository = null;
        private IGenericRepo<Category> _categoryrepository = null;
        private IGenericRepo<Item> _itemrepository = null;
        private IGenericRepo<ItemType> _itemtyperepository = null;
        private IGenericRepo<Model> _modelrepository = null;
        private IGenericRepo<Make> _makerepository = null;
        private IGenericRepo<User> _userrepository = null;
        private IGenericRepo<UserRole> _userrolerepository = null;
        private IGenericRepo<Department> _departmentrepository = null;
        private IGenericRepo<Stock> _stockrepository = null;
        private IUnitOfWorks _unitOfWork;


        public IssueService(IGenericRepo<ItemRequest> repository, IGenericRepo<IssueHeader> issuerepository, IGenericRepo<IssueDetails> issuedetrepository, IUnitOfWorks unitfwork, IGenericRepo<Category> categoryrepository,
               IGenericRepo<Item> itemrepository, IGenericRepo<UserRole> userrolerepository,
               IGenericRepo<ItemType> itemtyperepository, IGenericRepo<Model> modelrepository,
               IGenericRepo<Make> makerepository, IGenericRepo<User> userrepository, IGenericRepo<Stock> stockrepository, IGenericRepo<Department> departmentrepository)
        {
            this._repository = repository;
            this._unitOfWork = unitfwork;
            this._issuerepository = issuerepository;
            this._issuedetrepository = issuedetrepository;
            this._categoryrepository = categoryrepository;
            this._itemrepository = itemrepository;
            this._itemtyperepository = itemtyperepository;
            this._makerepository = makerepository;
            this._modelrepository = modelrepository;
            this._userrolerepository = userrolerepository;
            this._userrepository = userrepository;
            this._stockrepository = stockrepository;
            this._departmentrepository = departmentrepository;
        }

        public async Task<DataGridTable> GetIssueGridAsync()
        {
            var approvedlist = (await _repository.GetAll()).Select(a => new IssueGridResource()
            {
                ItemRequestID = a.ItemRequestId,
                RequestTitle = a.RequestTitle,
                CategoryID = a.CategoryId,
                ItemTypeID = a.ItemTypeId,
                IsIssued = a.IsIssued,
                ItemID = a.ItemId,
                MakeID = a.MakeId == null ? null : a.MakeId,
                ModelID = a.ModelId == null ? null : a.ModelId,
                PriorityID = a.PriorityId,
                IsReplaceble = a.IsReplaceble,
                Remark = a.Remark,
                RequiredDate = a.RequiredDate,
                NoOfUnits = a.NoOfUnits,
                IsApproved = a.IsApproved,
                IsRejected = a.IsRejected,
                Status = a.IsApproved == true ? "Approved" : a.IsRejected == true ? "Rejected" : "Pending",
                ApprovedDateTime = a.ApprovedDateTime.HasValue ? a.ApprovedDateTime : Convert.ToDateTime("1900-01-01"),
                ApprovalComment = a.ApprovalComment,
                RequestedDateTime = a.RequestedDateTime,
                RequestedUserID = a.RequestedUserId,
                DepartmentID = a.DepartmentId,
                ApprovedUserID = a.ApprovedUserId == null ? null : a.ApprovedUserId,
                UpdatedRequestedDateTime = a.UpdatedRequestedDateTime.HasValue ? a.UpdatedRequestedDateTime : Convert.ToDateTime("1900-01-01"),

                CategoryName = _categoryrepository.GetByIdAsync(a.CategoryId).Result.CategoryName.ToString(),
                ItemDescription = _itemrepository.GetByIdAsync(a.ItemId).Result.ItemDescription.ToString(),
                ItemTypeName = _itemtyperepository.GetByIdAsync(a.ItemTypeId).Result.ItemTypeName.ToString(),
                MakeName = a.MakeId == null ? "" : _makerepository.GetByIdAsync(a.MakeId).Result.MakeName.ToString(),
                ModelName = a.ModelId == null ? "" : _modelrepository.GetByIdAsync(a.ModelId).Result.ModelName.ToString(),
                ApprovedUserName = a.ApprovedUserId == null ? "" : _userrepository.GetByIdAsync(a.ApprovedUserId).Result.UserName.ToString(),
                RequestedUserName = _userrepository.GetByIdAsync(a.RequestedUserId).Result.UserName.ToString(),
                DepartmentName = _departmentrepository.GetByIdAsync(a.DepartmentId).Result.DepartmentName.ToString(),
            }).Where(x => (x.IsApproved == true) && (x.IsIssued == false)).OrderByDescending(y => y.ApprovedDateTime).ToList();


            foreach (var q in approvedlist)
            {

                var qty2 = (await _stockrepository.GetAll()).Select(c => new StockResource()
                {
                    StockID = c.StockId,
                    ItemID = c.ItemId,
                    StockQty = c.StockQty,
                    BalancedQty = c.BalancedQty,
                    ReceivedQty = c.ReceivedQty

                }).Where(d => d.ItemID == q.ItemID).ToList();
                q.BalancedQty = qty2.FirstOrDefault().BalancedQty;
                q.ReceivedQty = qty2.FirstOrDefault().ReceivedQty;
                // dictionaryList2.Add(qty2);
            }


            DataTable dtissued = CommonGenericService<ItemRequest>.ToDataTable(approvedlist);

            var dataTable = new DataGridTable
            {
                rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                enableSorting = true,
                enableColResize = false,
                DataGridColumns = GetIssuedGridColumnsfromList(dtissued),
                DataGridRows = GetIssuedGridRowsFromList(dtissued)
            };

            return dataTable;
        }

        private List<DataGridColumn> GetIssuedGridColumnsfromList(DataTable dataTable)
        {
            var DataGridColumns = new List<DataGridColumn>();


            foreach (DataColumn column in dataTable.Columns)
            {
                var dataTableColumn = new DataGridColumn
                {
                    field = column.ToString().Replace(" ", "_"),
                    headerName = column.ToString(),
                    //visible = 

                    //halign = DataAlignEnum.Center.ToString().ToLower()
                };

                if (column.ToString().Equals("ApprovedUserName"))
                {
                    dataTableColumn.width = 140;
                    dataTableColumn.headerName = "Approved User";
                }
                if (column.ToString().Equals("ApprovalComment"))
                {
                    dataTableColumn.width = 160;
                    dataTableColumn.headerName = "Approval Comment";
                }
                if (column.ToString().Equals("CategoryName"))
                {
                    dataTableColumn.width = 130;
                    dataTableColumn.headerName = "Category Name";
                }
                if (column.ToString().Equals("ItemTypeName"))
                {
                    dataTableColumn.width = 130;
                    dataTableColumn.headerName = "Item Type Name";
                }
                if (column.ToString().Equals("ItemDescription"))
                {
                    dataTableColumn.width = 120;
                    dataTableColumn.headerName = "Item Name";
                }
                if (column.ToString().Equals("MakeName"))
                {
                    dataTableColumn.width = 110;
                    dataTableColumn.headerName = "Make Name";
                }
                if (column.ToString().Equals("ModelName"))
                {
                    dataTableColumn.width = 115;
                    dataTableColumn.headerName = "Model Name";
                }
                if (column.ToString().Equals("NoOfUnits"))
                {
                    dataTableColumn.width = 105;
                    dataTableColumn.headerName = "No of Units";
                }
                if (column.ToString().Equals("RequestedDateTime"))
                {
                    dataTableColumn.width = 160;
                    dataTableColumn.headerName = "Requested Date Time";
                }
                if (column.ToString().Equals("ApprovedDateTime"))
                {
                    dataTableColumn.width = 150;
                    dataTableColumn.headerName = "Approved Date";
                }
                if (column.ToString().Equals("RequestTitle"))
                {
                    dataTableColumn.headerName = "Request Title";
                }
                if (column.ToString().Equals("RequiredDate"))
                {
                    dataTableColumn.width = 135;
                    dataTableColumn.headerName = "Required Date";
                }
                if (column.ToString().Equals("RequestedUserName"))
                {
                    dataTableColumn.width = 140;
                    dataTableColumn.headerName = "Requested User";
                }
                if (column.ToString().Equals("ReceivedQty"))
                {
                    dataTableColumn.width = 115;
                    dataTableColumn.headerName = "Received Qty";
                }

                if (!column.ToString().Equals("ItemRequestID")
                    && !column.ToString().Equals("CategoryID")
                    && !column.ToString().Equals("MakeID")
                    && !column.ToString().Equals("PriorityID")
                    && !column.ToString().Equals("DepartmentName")
                    && !column.ToString().Equals("ItemTypeID")
                    && !column.ToString().Equals("ModelID")
                    && !column.ToString().Equals("BalancedQty")
                    && !column.ToString().Equals("Remark")
                    && !column.ToString().Equals("AssetCode")
                    && !column.ToString().Equals("IsReplaceble")
                    && !column.ToString().Equals("ItemID")
                    && !column.ToString().Equals("ApprovedUserID")
                    && !column.ToString().Equals("IsApproved")
                    && !column.ToString().Equals("IsRejected")
                    && !column.ToString().Equals("Status")
                    && !column.ToString().Equals("RequestedUserID")
                    && !column.ToString().Equals("DepartmentID")
                    && !column.ToString().Equals("ApproverID")
                    && !column.ToString().Equals("Attachment")
                    && !column.ToString().Equals("IsIssued")
                    && !column.ToString().Equals("UpdatedRequestedDateTime")
                    && !column.ToString().Equals("PriorityLevelName")
                    && !column.ToString().Equals("ApproverName")
                    && !column.ToString().Equals("TotalPendingItemRequests")
                    && !column.ToString().Equals("TotalRejectedItemRequests")
                    && !column.ToString().Equals("TotalApprovedItemRequests")
                    && !column.ToString().Equals("TotalItemRequests"))


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


        private static List<Object> GetIssuedGridRowsFromList(DataTable dataTable)
        {
            var dictionaryList = new List<object>();
            foreach (DataRow row in dataTable.Rows)
            {
                var dictionary = new Dictionary<string, string>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    var rowValue = row[column].ToString();
                    if (column.ToString().Equals("ApprovedDateTime"))
                    {
                        rowValue = Convert.ToDateTime(rowValue).ToString();
                    }
                    else if(column.ToString().Equals("RequestedDateTime"))
                    {

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


        public async Task<GenericSaveResponse<IssueHeader>> SaveIssueAsync(IssueHeader issue)
        {

            try
            {
                if (issue.IssuedHeaderId == Guid.Empty)
                {
                    issue.IssuedHeaderId = Guid.NewGuid();
                }

                foreach(IssueDetails issudet in issue.IssueDetails)
                {
                    issudet.IssueDetailId = Guid.NewGuid();
                    IssueDetails issdetails = new IssueDetails();
                    issudet.IssuedHeaderId = issue.IssuedHeaderId;

                    //Stock currStock = new Stock();
                    var getcuustock = (await _stockrepository.GetAll()).Where(a => a.ItemId == issudet.ItemId).ToList();

                    foreach(Stock stck in getcuustock)
                    {
                        stck.StockQty = stck.StockQty - issudet.Qty;
                        stck.ReceivedQty = stck.ReceivedQty - issudet.Qty;

                        if (!(stck.StockQty>=0))
                        {
                            return new GenericSaveResponse<IssueHeader>($"This Item hasn't stock!.");
                        }
                        //Stock currStock = new Stock();
                        //currStock.StockQty = stck.StockQty;
                        //currStock.StockId = stck.StockId;
                        _stockrepository.Update(stck);

                        //_stockrepository.Update(currStock);

                    }

                    await _issuedetrepository.InsertAsync(issudet);
                }

                await _issuerepository.InsertAsync(issue);

                ItemRequest itemRequest = new ItemRequest();
                itemRequest =  await _repository.GetByIdAsync(issue.ItemRequestId);
                itemRequest.IsIssued = true;
                itemRequest.IssuedUserId = issue.IssuedUserId;

                _repository.Update(itemRequest);
                

                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<IssueHeader>(true, "Successfully Saved.", issue);
            }
            catch(Exception ex)
            {
                return new GenericSaveResponse<IssueHeader>($"An error occured when saving the Issue :" + (ex.Message ?? ex.InnerException.Message));
            }

        }

    }
}
