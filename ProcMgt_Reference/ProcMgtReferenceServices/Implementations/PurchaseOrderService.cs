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
    public class PurchaseOrderService : IPurchaseOrderServices
    {
        private IGenericRepo<QuotationRequestHeader> _repository = null;
        private IGenericRepo<Poheader> _poHeaderrepository = null;
        private IGenericRepo<Podetail> _poDetailrepository = null;
        private IGenericRepo<QuotationRequestDetails> _quotationReqDetailsrepository = null;
        private IGenericRepo<QuotationRequestHeader> _quotationReqHeaderrepository = null;
        private IGenericRepo<User> _userrepository = null;
        private IGenericRepo<Supplier> _supplierrepository = null;
        private IGenericRepo<QuotationRequestStatus> _quoreqstatusrepository = null;
        private IGenericRepo<Item> _itemrepository = null;
        private IGenericRepo<Model> _modelrepository = null;
        private IGenericRepo<Make> _makerepository = null;
        private IGenericRepo<MeasurementUnits> _measurementUnitrepository = null;
        private IUnitOfWorks _unitOfWork;
        private IGenericRepo<Poheader> _PoReportDetailsrepository = null;
        private IGenericRepo<Supplier> _SuppliersIDrespository = null; //rajitha

        public PurchaseOrderService(IGenericRepo<Poheader> poHeaderrepository, IGenericRepo<Podetail> poDetailrepository, IGenericRepo<Poheader> PoReportDetailsrepository, IGenericRepo<QuotationRequestDetails> quotationReqDetailsrepository,  //
            IGenericRepo<QuotationRequestHeader> quotationReqHeaderrepository, IGenericRepo<QuotationRequestStatus> quoreqstatusrepository,
            IGenericRepo<Supplier> supplierrepository, IGenericRepo<User> userrepository, IGenericRepo<Model> modelrepository,
            IGenericRepo<Make> makerepository, IGenericRepo<MeasurementUnits> measurementunitrepository,
            IGenericRepo<Item> itemrepository, IUnitOfWorks unitfwork, IGenericRepo<Supplier> SuppliersIDrespository)

        {
            this._poDetailrepository = poDetailrepository;
            this._poHeaderrepository = poHeaderrepository;
            this._quotationReqDetailsrepository = quotationReqDetailsrepository;
            this._quotationReqHeaderrepository = quotationReqHeaderrepository;
            this._userrepository = userrepository;
            this._supplierrepository = supplierrepository;
            this._quoreqstatusrepository = quoreqstatusrepository;
            this._itemrepository = itemrepository;
            this._makerepository = makerepository;
            this._modelrepository = modelrepository;
            this._measurementUnitrepository = measurementunitrepository;
            this._unitOfWork = unitfwork;
            this._PoReportDetailsrepository = PoReportDetailsrepository; //
            this._SuppliersIDrespository = SuppliersIDrespository; //
           
        }

        public async Task<IEnumerable<PoHeaderResource>> GetAllPoReportDetailsAsync(string id, Poheader poheader)  //
        {


            try
            {
                //Poheader poReportDetails = new Poheader();


                var poReportDetails = (await _poHeaderrepository.GetAll()).Select(a => new PoHeaderResource()
                {
                    PoheaderID = a.PoheaderId,
                    QuotationRequestHeaderID = a.QuotationRequestHeaderId,
                    RequestedDeliveryDate = a.RequestedDeliveryDate,
                    Ponumber = a.Ponumber,
                    TaxAmount = a.TaxAmount,
                    Podetail = a.Podetail,
                    ActualDeliveryDate = a.ActualDeliveryDate,
                    PaymentMethodID = a.PaymentMethodId == null ? null : a.PaymentMethodId,
                    PodateTime = a.PodateTime,
                    TotalPoamount = a.TotalPoamount == null ? 0 : a.TotalPoamount,
                    UserID = a.UserId == null ? null : a.UserId,

                    QuotationNumber = _quotationReqHeaderrepository.GetByIdAsync(a.QuotationRequestHeaderId).Result.QuotationNumber.ToString(),
                    SupplierID = _quotationReqHeaderrepository.GetByIdAsync(a.QuotationRequestHeaderId).Result.SupplierId,
                    UserName = a.UserId == null ? "" : _userrepository.GetByIdAsync(a.UserId).Result.UserName.ToString(),
                    PaymentMethodName = "",


                }).Where(b => b.PoheaderID == poheader.PoheaderId).ToList();

                foreach (var q in poReportDetails)
                {
                    var podetails = (await _poDetailrepository.GetAll()).Select(b => new PoDetailResource()
                    {
                        PodetailID = b.PodetailId,
                        PoheaderID = b.PoheaderId,
                        ItemID = b.ItemId,
                        Qty = b.Qty,
                        TotalAmount = b.TotalAmount,
                        UnitPrice = b.UnitPrice,
                        DiscountAmount = b.DiscountAmount == null ? 0 : b.DiscountAmount,
                        InvoiceQty = 0,
                        RecivedQty = 0,
                        RejectedQty = 0,
                        Remark = "",

                        ItemDescription = _itemrepository.GetByIdAsync(b.ItemId).Result.ItemDescription.ToString(),
                    }).Where(d => d.PoheaderID == q.PoheaderID).ToList();

                    //q.Podetail.FirstOrDefault().PodetailId = podetails.FirstOrDefault().PodetailID;
                    //q.Podetail.FirstOrDefault().PoheaderId = podetails.FirstOrDefault().PoheaderID;
                    //q.Podetail.FirstOrDefault().ItemId = podetails.FirstOrDefault().ItemID;
                    //q.Podetail.FirstOrDefault().Qty = podetails.FirstOrDefault().Qty;
                    //q.Podetail.FirstOrDefault().TotalAmount = podetails.FirstOrDefault().TotalAmount;
                    //q.Podetail.FirstOrDefault().UnitPrice = podetails.FirstOrDefault().UnitPrice;

                    q.PoReportDetails = podetails;

                }

                foreach (var z in poReportDetails)
                {
                    var supplierDetails = (await _supplierrepository.GetAll()).Select(c => new SupplierResource()
                    {
                        SupplierID = c.SupplierId,
                        SupplierName = c.SupplierName,
                        Address = c.Address,

                    }).Where(e => e.SupplierID == z.SupplierID).ToList();

                    z.SupplierName = supplierDetails.FirstOrDefault().SupplierName;
                    z.SupplierAddress = supplierDetails.FirstOrDefault().Address;
                }

                return poReportDetails;

            }
            catch (Exception ex)
            {
                return null;
            }


        }
   
    public async Task<IEnumerable<Poheader>> GetAllPoHeaderAsync()
        {
            return await _poHeaderrepository.GetAll();
        }

        public async Task<IEnumerable<Podetail>> GetAllPoDetailsAsync()
        {
            return await _poDetailrepository.GetAll();
        }

        public async Task<DataGridTable> GetQuotationListGridAsync()
        {

            try
            {
                var quotationList = (await _quotationReqHeaderrepository.GetAll()).Select(a => new QuotationRequestHeaderResource()
                {

                    QuotationRequestHeaderID = a.QuotationRequestHeaderId,
                    SupplierID = a.SupplierId == null ? null : a.SupplierId,
                    QuotationNumber = a.QuotationNumber,
                    QuotationRequestedDate = a.QuotationRequestedDate,
                    UserID = a.UserId,
                    RequiredDate = a.RequiredDate.HasValue ? a.RequiredDate : Convert.ToDateTime("1900-01-01"),
                    QuotationRequestStatusID = a.QuotationRequestStatusId,
                    IsEnteringCompleted = a.IsEnteringCompleted.HasValue ? a.IsEnteringCompleted.Value : false,
                    IsDelivered = a.IsDelivered.HasValue? a.IsDelivered.Value: false,

                    UserName = _userrepository.GetByIdAsync(a.UserId).Result.UserName.ToString(),
                    SupplierName = _supplierrepository.GetByIdAsync(a.SupplierId).Result.SupplierName.ToString(),
                    QuotationRequestStatus1 = _quoreqstatusrepository.GetByIdAsync(a.QuotationRequestStatusId).Result.QuotationRequestStatus1.ToString(),


                    //ItemDescription = String.Join(",", _podetailrepository.GetAll().Result.Where(b => b.PoheaderId == a.PoheaderId).Select(x => x.Item.ItemDescription).ToList())


                }).Where(a => a.QuotationRequestStatusID ==2 &  a.IsDelivered == false ).OrderByDescending(x => x.QuotationNumber);


                DataTable dtQuotation = CommonGenericService<QuotationRequestHeader>.ToDataTable(quotationList);


                var dataTable = new DataGridTable
                {
                    rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                    enableSorting = true,
                    enableColResize = false,
                    suppressSizeToFit = true,
                    DataGridColumns = GetQuotationColumnsfromList(dtQuotation),
                    DataGridRows = GetQuotationRowsFromList(dtQuotation)
                };

                return dataTable;
            }
            catch(Exception ex)
            {
                return null;
            }
          
           
        }

        private List<DataGridColumn> GetQuotationColumnsfromList(DataTable dataTable)
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

                if (column.ToString().Equals("QuotationNumber"))
                {
                    dataTableColumn.width = 150;
                    dataTableColumn.headerName = "Quotation Number";

                }

                if (column.ToString().Equals("QuotationRequestedDate"))
                {
                    dataTableColumn.width = 190;
                    dataTableColumn.headerName = "Quotation Requested Date";

                }

                if (column.ToString().Equals("SupplierName"))
                {
                    dataTableColumn.width = 170;
                    dataTableColumn.headerName = "Supplier Name";

                }

                if (column.ToString().Equals("UserName"))
                {
                    dataTableColumn.width = 170;
                    dataTableColumn.headerName = "User Name";

                } 
                   
                if (column.ToString().Equals("IsEnteringCompleted"))
                {
                    dataTableColumn.width = 170; 
                    dataTableColumn.headerName = "Is Entering Completed";

                } 
     
                if (column.ToString().Equals("RequiredDate"))
                {
                    dataTableColumn.width = 170;
                    dataTableColumn.headerName = "Required Date";

                } 

               if (column.ToString().Equals("IsDelivered"))
                {
                    dataTableColumn.width = 170;
                    dataTableColumn.headerName = "IsDelivered";

                }

                if (column.ToString().Equals("QuotationRequestStatus1"))
                {
                    dataTableColumn.width = 170;
                    dataTableColumn.headerName = "Quotation Request Status1";

                } 


                if (!column.ToString().Equals("QuotationRequestHeaderID")
                    && !column.ToString().Equals("SupplierID")
                     && !column.ToString().Equals("UserID")
                     && !column.ToString().Equals("IsEnteringCompleted")
                      && !column.ToString().Equals("QuotationCompleted")
                      && !column.ToString().Equals("QuotationRequestStatus1")
                      && !column.ToString().Equals("QuotationRequestStatusID")
                      && !column.ToString().Equals("QuotationRequestDetails")
                      && !column.ToString().Equals("ApprovalComment"))
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

        private static List<Object> GetQuotationRowsFromList(DataTable dataTable)
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


         
        public async Task<DataGridTable> GetQuotatioDetailsListGridAsync(Poheader poheader)
        {  try
            {
                var quotationReqDetailsList = (await _quotationReqDetailsrepository.GetAll()).Where(a => a.QuotationRequestHeaderId == poheader.QuotationRequestHeaderId).Select(b => new QuotationRequestDetailsResource()
                {

                    QuotationRequestDetailID = b.QuotationRequestDetailId,
                    QuotationRequestHeaderID = b.QuotationRequestHeaderId,
                   
                    MakeID = b.MakeId == null ? null : b.MakeId,
                    ModelID = b.ModelId == null ? null : b.ModelId,
                    ItemID = b.ItemId,
                    MeasurementUnitID = b.MeasurementUnitId,
                    UnitPrice = b.UnitPrice == null ? null : b.UnitPrice,
                    Quantity = b.Quantity == null ? null : b.Quantity,
                    GrossAmount = b.GrossAmount == null ? null : b.GrossAmount,
                    NetAmount = b.NetAmount == null ? null : b.NetAmount,
                    DiscountAmount = b.DiscountAmount,
                    QuotationValidDate = b.QuotationValidDate.HasValue ? b.QuotationValidDate : Convert.ToDateTime("1900-01-01"),
                    Attachment = b.Attachment,


                    itemDescription = _itemrepository.GetByIdAsync(b.ItemId).Result.ItemDescription.ToString(),
                    measurementUnitName = _measurementUnitrepository.GetByIdAsync(b.MeasurementUnitId).Result.MeasurementUnitName.ToString(),
                    makeName = b.MakeId == null ? "" : _makerepository.GetByIdAsync(b.MakeId).Result.MakeName.ToString(),
                    modelName = b.ModelId == null ? "" : _modelrepository.GetByIdAsync(b.ModelId).Result.ModelName.ToString(),

                   
                   

                }).ToList();



                DataTable dtquotationReqDetails = CommonGenericService<Poheader>.ToDataTable(quotationReqDetailsList);

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
            catch (Exception ex) {

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
                    
                  

                    //visible = 

                    //halign = DataAlignEnum.Center.ToString().ToLower()
                };
                
                //if (column.ToString().Equals("Qty")
                //     && !column.ToString().Equals("PounitPrice")
                //      && !column.ToString().Equals("TotalAmount"))
                //{
                //    dataTableColumn.edit = true;
                //}
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

                

                if (column.ToString().Equals("Quantity"))
                {
                    dataTableColumn.editable = true;
                    dataTableColumn.headerName = "Quantity";
                    dataTableColumn.width = 120;
                }
                 
                if (column.ToString().Equals("UnitPrice"))
                {
                    dataTableColumn.width = 100;
                    dataTableColumn.headerName = "Unit Price";
                }  
                            
                 if (column.ToString().Equals("DiscountAmount"))
                {
                    dataTableColumn.width = 140;
                    dataTableColumn.headerName = "Discount Amount";
                } 
                

                if (!column.ToString().Equals("CategoryId")
                    && !column.ToString().Equals("Accers")
                     && !column.ToString().Equals("QuotationRequestDetailID")
                      && !column.ToString().Equals("QuotationRequestHeaderID")
                       && !column.ToString().Equals("MakeID")
                        && !column.ToString().Equals("ModelID")
                         && !column.ToString().Equals("ItemID")
                          && !column.ToString().Equals("MeasurementUnitID")
                           && !column.ToString().Equals("GrossAmount")
                            && !column.ToString().Equals("NetAmount")
                             && !column.ToString().Equals("PodetailId")
                              && !column.ToString().Equals("QuotationValidDate")
                            
                             && !column.ToString().Equals("Attachment"))
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


        //public async Task<GenericSaveResponse<Poheader>> SavePurchaseOrderAsync(Poheader poheader)
        //{
        //    try
        //    {
        //        if (poheader.PoheaderId == Guid.Empty)
        //        {
        //            poheader.PoheaderId = Guid.NewGuid();
        //        }

        //        await _poHeaderrepository.InsertAsync(poheader);
        //        await _unitOfWork.CompleteAsync();

        //        return new GenericSaveResponse<Poheader>(true, "Successfully Saved.", poheader);

        //    }
        //    catch (Exception ex)
        //    {
        //        return new GenericSaveResponse<Poheader>($"An error occured when saving the PO :" + (ex.Message ?? ex.InnerException.Message));
        //    }
        //}


        public async Task<GenericSaveResponse<Poheader>> SavePurchaseOrderAsync(Poheader poheader)
        {
            try
            {
                if (poheader.PoheaderId == Guid.Empty)
                {
                    poheader.PoheaderId = Guid.NewGuid();
                }

                foreach (Podetail podetailitem in poheader.Podetail)
                {

                    podetailitem.PodetailId = Guid.NewGuid();
                    Podetail con = new Podetail();
                    con.PoheaderId = poheader.PoheaderId;


                    

                    await _poDetailrepository.InsertAsync(podetailitem);
                    //await _unitOfWork.CompleteAsync();
                }
                if (poheader.IsDeliver == true)
                {
                    QuotationRequestHeader existingQuotationReqHeader = await _quotationReqHeaderrepository.GetByIdAsync(poheader.QuotationRequestHeaderId);
                    existingQuotationReqHeader.IsDelivered = true;

                    _quotationReqHeaderrepository.Update(existingQuotationReqHeader);


                    //return new GenericSaveResponse<Poheader>(true, " PurchSuccessfully Saved.", poheader);
                }
                //foreach (ContactDetails conitem in supplier.ContactDetails)
                //{
                //    conitem.ContactDetailsId = Guid.NewGuid();
                //    ContactDetails con = new ContactDetails();
                //    con.SupplierId = supplier.SupplierId;


                //    await _contactrepository.InsertAsync(conitem);
                //}


                await _poHeaderrepository.InsertAsync(poheader);
                await _unitOfWork.CompleteAsync();

                


                return new GenericSaveResponse<Poheader>(true, " PurchSuccessfully Saved.", poheader);
                
            }
            
            catch (Exception ex)
            {
                return new GenericSaveResponse<Poheader>($"An error occured when saving the Purchase Order :" + (ex.Message ?? ex.InnerException.Message));
            }

            
        }

    }

}
