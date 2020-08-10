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
    public class QuotationEnterService : IQuotationEnterServices
    {
        private IGenericRepo<QuotationRequestHeader> _headerrepository = null;
        private IGenericRepo<QuotationRequestDetails> _quotationReqDetailsrepository = null;
        private IUnitOfWorks _unitOfWork;
        private IGenericRepo<Supplier> _supplierrepository = null;
        private IGenericRepo<User> _userrepository = null;
        private IGenericRepo<Model> _modelrepository = null;
        private IGenericRepo<Make> _makerepository = null;
        private IGenericRepo<QuotationRequestStatus> _quoreqstatusrepository = null;
        private IGenericRepo<Item> _itemrepository = null;
        private IGenericRepo<MeasurementUnits> _measurementUnitrepository = null;

        public QuotationEnterService(IGenericRepo<QuotationRequestHeader> detrepository, IGenericRepo<Supplier> supplierrepository, IGenericRepo<Model> modelrepository,
            IGenericRepo<Make> makerepository, IGenericRepo<MeasurementUnits> measurementunitrepository,
            IGenericRepo<Item> itemrepository, IGenericRepo<QuotationRequestDetails> quotationReqDetailsrepository, IGenericRepo<QuotationRequestStatus> quoreqstatusrepository, IGenericRepo<User> userrepository, IUnitOfWorks unitfwork)
        {
            this._headerrepository = detrepository;
            this._quotationReqDetailsrepository = quotationReqDetailsrepository;
            this._unitOfWork = unitfwork;
            this._supplierrepository = supplierrepository;
            this._userrepository = userrepository;
            this._quoreqstatusrepository = quoreqstatusrepository;
            this._makerepository = makerepository;
            this._modelrepository = modelrepository;
            this._measurementUnitrepository = measurementunitrepository;
            this._itemrepository = itemrepository;
        }

        public async Task<IEnumerable<QuotationRequestHeader>> GetAllAsync()
        {
            return await _headerrepository.GetAll();
        }

        public async Task<DataGridTable> GetPendingQuotationRequestDetailsGrid()
        {
            try
            {
                var pendingQuotationRequestDetailsList = (await _headerrepository.GetAll()).Select(a => new QuotationRequestHeaderResource()
                {
                    QuotationNumber = a.QuotationNumber,
                    SupplierID = a.SupplierId,
                    UserID = a.UserId,
                    RequiredDate = a.RequiredDate.HasValue ? a.RequiredDate : Convert.ToDateTime("1990-01-01"),
                    QuotationRequestedDate = a.QuotationRequestedDate,
                    QuotationRequestHeaderID = a.QuotationRequestHeaderId,
                    QuotationRequestStatusID = a.QuotationRequestStatusId,
                    IsEnteringCompleted = a.IsEnteringCompleted.HasValue ? a.IsEnteringCompleted.Value : false,
                    QuotationCompleted = a.IsEnteringCompleted == true ? "Yes" :  "No",

                    SupplierName = _supplierrepository.GetByIdAsync(a.SupplierId).Result.SupplierName.ToString(),
                    UserName = _userrepository.GetByIdAsync(a.UserId).Result.UserName.ToString(),
                    QuotationRequestStatus1 = _quoreqstatusrepository.GetByIdAsync(a.QuotationRequestStatusId).Result.QuotationRequestStatus1.ToString()


                }).Where(b => b.QuotationRequestStatusID == 1).OrderByDescending(x => x.QuotationRequestedDate);

                DataTable dtQuotationRequestDetails = CommonGenericService<QuotationRequestDetails>.ToDataTable(pendingQuotationRequestDetailsList);

                var dataTable = new DataGridTable
                {
                    rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                    enableSorting = true,
                    enableColResize = false,
                    suppressSizeToFit = true,
                    DataGridColumns = GetPendingQuotationRequestDetailsColumnsfromList(dtQuotationRequestDetails),
                    DataGridRows = GetPendingQuotationRequestDetailsRowsFromList(dtQuotationRequestDetails)
                };

                return dataTable;
            }
            catch (Exception ex)
            {
                return null;
            }
            //var quotationRequestDetailsList = await _repository.GetAll();


        }

        private List<DataGridColumn> GetPendingQuotationRequestDetailsColumnsfromList(DataTable dataTable)
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

                if (column.ToString().Equals("QuotationNumber"))
                {
                    dataTableColumn.width = 150;
                    dataTableColumn.headerName = "Quotation Number";
                }
                if (column.ToString().Equals("SupplierName"))
                {
                    dataTableColumn.width = 125;
                    dataTableColumn.headerName = "Supplier Name";
                }
                if (column.ToString().Equals("UserName"))
                {
                    dataTableColumn.width = 125;
                    dataTableColumn.headerName = "User Name";
                }
                if (column.ToString().Equals("RequiredDate"))
                {
                    dataTableColumn.width = 120;
                    dataTableColumn.headerName = "Required Date";
                }
                if (column.ToString().Equals("QuotationRequestedDate"))
                {
                    dataTableColumn.width = 190;
                    dataTableColumn.headerName = "Quotation Requested Date";
                }
                if (column.ToString().Equals("QuotationRequestStatus1"))
                {
                    dataTableColumn.width = 160;
                    dataTableColumn.headerName = "Quotation Status";
                }
                if (column.ToString().Equals("QuotationCompleted"))
                {
                    dataTableColumn.width = 170;
                    dataTableColumn.headerName = "Quotation Completed";
                }
                

                if (!column.ToString().Equals("SupplierID")
                    && !column.ToString().Equals("UserID")
                    && !column.ToString().Equals("QuotationRequestHeaderID")
                    && !column.ToString().Equals("QuotationRequestStatusID")
                    && !column.ToString().Equals("QuotationRequestDetails")
                    && !column.ToString().Equals("ApprovalComment")
                    && !column.ToString().Equals("IsEnteringCompleted")) 
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


        private static List<Object> GetPendingQuotationRequestDetailsRowsFromList(DataTable dataTable)
        {
            var dictionaryList = new List<object>();
            foreach (DataRow row in dataTable.Rows)
            {
                var dictionary = new Dictionary<string, string>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    var rowValue = row[column].ToString();
                    if (column.ToString().Equals("QuotationRequestedDate"))
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


        public async Task<DataGridTable> QuotationDetailsGrid(QuotationRequestDetails quotationrequestdetails)
        {
            try
            {
                var quotationReqDetailsList = (await _quotationReqDetailsrepository.GetAll()).Where(a => a.QuotationRequestHeaderId == quotationrequestdetails.QuotationRequestHeaderId).Select(b => new QuotationRequestDetailsResource()
                {
                    QuotationRequestDetailID = b.QuotationRequestDetailId,
                    QuotationRequestHeaderID = b.QuotationRequestHeaderId,

                    MakeID = b.MakeId == null ? null : b.MakeId,
                    ModelID = b.ModelId == null ? null : b.ModelId, 
                    ItemID = b.ItemId,
                    MeasurementUnitID = b.MeasurementUnitId,
                    UnitPrice = b.UnitPrice == null ? 0 :  Decimal.Round(Convert.ToDecimal(b.UnitPrice), 2),
                    Quantity = b.Quantity,
                    GrossAmount = b.GrossAmount == null ? 0 : Decimal.Round(Convert.ToDecimal(b.GrossAmount) , 2) ,
                    NetAmount = b.NetAmount == null ? 0 :  Decimal.Round(Convert.ToDecimal(b.NetAmount), 2),
                    DiscountAmount = b.DiscountAmount == null? 0 : Decimal.Round(Convert.ToDecimal(b.DiscountAmount), 2),
                    QuotationValidDate = b.QuotationValidDate.HasValue ? b.QuotationValidDate : Convert.ToDateTime("0001-01-01"),
                    //QuotationValidDate = b.QuotationValidDate == null ? "" : b.QuotationValidDate,
                    Attachment = b.Attachment,

                    makeName = b.MakeId == null ? "" : _makerepository.GetByIdAsync(b.MakeId).Result.MakeName.ToString(),
                    modelName = b.ModelId == null ? "" : _modelrepository.GetByIdAsync(b.ModelId).Result.ModelName.ToString(),
                    itemDescription = _itemrepository.GetByIdAsync(b.ItemId).Result.ItemDescription.ToString(),
                    measurementUnitName = _measurementUnitrepository.GetByIdAsync(b.MeasurementUnitId).Result.MeasurementUnitName.ToString()

                }).ToList();

                DataTable dtquotationReqDetails = CommonGenericService<QuotationRequestDetails>.ToDataTable(quotationReqDetailsList);

                var dataTable = new DataGridTable
                {
                    rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                    enableSorting = true,
                    enableColResize = false,
                    suppressSizeToFit = true,
                    DataGridColumns = GetquotationReqDetailsColumnsfromList(dtquotationReqDetails),
                    DataGridRows = GetquotationReqDetailsRowsFromList(dtquotationReqDetails)
                };

                return dataTable;

            }
            catch (Exception ex)
            {

                return null;
            }
        }

        private List<DataGridColumn> GetquotationReqDetailsColumnsfromList(DataTable dataTable)
        {
            var DataGridColumns = new List<DataGridColumn>();

            foreach (DataColumn column in dataTable.Columns)
            {
                var dataTableColumn = new DataGridColumn
                {
                    field = column.ToString().Replace(" ", "_"),
                    headerName = column.ToString(),
                    width = 120,



                    //visible = 

                    //halign = DataAlignEnum.Center.ToString().ToLower()
                };
                
                if (column.ToString().Equals("Quantity"))
                {
                    dataTableColumn.width = 90;
                }

                if (column.ToString().Equals("UnitPrice"))
                {
                    dataTableColumn.editable = true;
                   
                    dataTableColumn.width = 95;
                    dataTableColumn.headerName = "Unit Price";
                    
                      


                                 

                }
                if (column.ToString().Equals("GrossAmount"))
                {
                    dataTableColumn.editable = false;
                    dataTableColumn.width = 120;
                    dataTableColumn.headerName = "Gross Amount";
                }
                if (column.ToString().Equals("NetAmount"))
                {
                    dataTableColumn.editable = false;
                    dataTableColumn.width = 110;
                    dataTableColumn.headerName = "Net Amount";
                }
                if (column.ToString().Equals("DiscountAmount"))
                {
                    dataTableColumn.editable = true;
                    dataTableColumn.width = 140;
                    dataTableColumn.headerName = "Discount Amount";
                }
                if (column.ToString().Equals("itemDescription"))
                {
                    dataTableColumn.headerName = "Item Name";
                    dataTableColumn.width = 120;
                }
                if (column.ToString().Equals("measurementUnitName"))
                {
                    dataTableColumn.width = 180;
                    dataTableColumn.headerName = "Measurement Unit Name";
                }
                if (column.ToString().Equals("makeName"))
                {
                    dataTableColumn.width = 130;
                    dataTableColumn.headerName = "Make Name";
                }
                if (column.ToString().Equals("modelName"))
                {
                    dataTableColumn.width = 130;
                    dataTableColumn.headerName = "Model Name";
                }



                if (!column.ToString().Equals("CategoryId")
                    && !column.ToString().Equals("Accers")
                    && !column.ToString().Equals("QuotationRequestDetailID")
                    && !column.ToString().Equals("QuotationRequestHeaderID")
                    && !column.ToString().Equals("ItemID")
                    && !column.ToString().Equals("MeasurementUnitID")
                    && !column.ToString().Equals("ModelID")
                    && !column.ToString().Equals("MakeID")
                    && !column.ToString().Equals("PodetailId")) 
                {
                    dataTableColumn.hide = false;

                }
                else
                {
                    dataTableColumn.hide = true;
                }
                
                if (column.ToString().Equals("Attachment"))
                {
                    dataTableColumn.editable = true;
                    dataTableColumn.width = 125;
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
                if (column.ToString().Equals("QuotationValidDate"))
                {
                    dataTableColumn.editable = false;
                    dataTableColumn.width = 160;
                    dataTableColumn.hide = false;
                    dataTableColumn.type = "dateColumn";
                    dataTableColumn.headerName = "Quotation Valid Date";
                }
                DataGridColumns.Add(dataTableColumn);
            }
            return DataGridColumns;
        }

        private static List<Object> GetquotationReqDetailsRowsFromList(DataTable dataTable)
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

        public async Task<GenericSaveResponse<QuotationRequestDetails>> UpdateQuotationEnterAsync(string id, QuotationRequestDetails quotationenter)
        {
            try
            {
                QuotationRequestDetails existingQuotationEnter = await _quotationReqDetailsrepository.GetByIdAsync(quotationenter.QuotationRequestDetailId);

                if(existingQuotationEnter == null)
                    return new GenericSaveResponse<QuotationRequestDetails>($"Quotation Enter not found");

                ResourceComparer<QuotationRequestDetails> Comparer = new ResourceComparer<QuotationRequestDetails>(quotationenter, existingQuotationEnter);
                ResourceComparerResult<QuotationRequestDetails> CompareResult = Comparer.GetUpdatedObject();

                if (CompareResult.Updated)
                {
                    _quotationReqDetailsrepository.Update(CompareResult.Obj);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<QuotationRequestDetails>(quotationenter);


            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<QuotationRequestDetails>($"An error occured when updating the Quotation Enter:" + (ex.Message ?? ex.InnerException.Message));
            }
        }

    }
}
