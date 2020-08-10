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
    public class ArnEntryService : IArnEntryServices
    {
        private IGenericRepo<Arnheader> _arnheaderrepository = null;
        private IGenericRepo<Arndetail> _arndetailrepository = null;
        private IGenericRepo<Poheader> _poheaderrepository = null;
        private IGenericRepo<Podetail> _podetailrepository = null;
        private IGenericRepo<User> _userrepository = null;
        private IGenericRepo<PaymentMethod> _paymentmethodrepository = null;
        private IGenericRepo<Item> _itemrepository = null;
        private IGenericRepo<QuotationRequestHeader> _quotationReqHeaderrepository = null;
        private IGenericRepo<Stock> _stockrepository = null;

        private IUnitOfWorks _unitOfWork;

        public ArnEntryService(IGenericRepo<Arnheader> arnheaderrepository, IGenericRepo<Arndetail> arndetailrepository, IGenericRepo<Poheader> poheaderrepository,
            IGenericRepo<Podetail> podetailrepository ,
            IGenericRepo<QuotationRequestHeader> quotationReqHeaderrepository, IGenericRepo<Stock> stockrepository, IGenericRepo<User> userrepository, IGenericRepo<PaymentMethod> paymentmethodrepository, IGenericRepo<Item> itempository, IUnitOfWorks unitfwork)
        {
            this._arnheaderrepository = arnheaderrepository;
            this._arndetailrepository = arndetailrepository;
            this._poheaderrepository = poheaderrepository;
            this._podetailrepository = podetailrepository;
            this._quotationReqHeaderrepository = quotationReqHeaderrepository;
            this._userrepository = userrepository;
            this._itemrepository = itempository;
            this._paymentmethodrepository = paymentmethodrepository;
            this._stockrepository = stockrepository;
            this._unitOfWork = unitfwork;
        }

        public async Task<Arnheader> GetAllArnheaderAsync(Arnheader arnheader)
        {
            
            var arnHead = await _arnheaderrepository.GetAll();
            var arnHeadSin = (from a in arnHead
                              where a.PoheaderId == arnheader.PoheaderId
                              select a).FirstOrDefault();

            return  arnHeadSin;
        }

        public async Task<IEnumerable<Arndetail>> GetArndetailAsync()
        {
            return await _arndetailrepository.GetAll();
        }


        public async Task<DataGridTable> GetArndetailGridAsync(Arnheader arnheader)
        {

            try
            {
                List<ArndetailResource> arndetailList = new List<ArndetailResource>();

                var items = _podetailrepository.GetAll().Result.Where(b => b.PoheaderId == arnheader.PoheaderId)
                                            .Select(x => new {
                                                x.ItemId,
                                                x.Qty,
                                                itemDesc = _itemrepository.GetByIdAsync(x.ItemId).Result.ItemDescription }
                                            ).ToList();

                var arnID = _arnheaderrepository.GetAll().Result.Where(b => b.PoheaderId == arnheader.PoheaderId).Select(x => x.ArnheaderId).FirstOrDefault();

                foreach (var item in items)
                {
                    ArndetailResource tmpArnDetail = new ArndetailResource();

                                       
                    tmpArnDetail.ArndetailID = Guid.Empty;
                    tmpArnDetail.ArnheaderID = Guid.Empty;
                    tmpArnDetail.InvoiceQty = item.Qty;
                    tmpArnDetail.RecivedQty = 0;
                    tmpArnDetail.RejectedQty = 0;
                    tmpArnDetail.Remark = "";
                    tmpArnDetail.ItemID = item.ItemId;
                    tmpArnDetail.ItemDescription = item.itemDesc;
                    //tmpArnDetail.isEdit = false;

                    ////if (arnID != Guid.Empty)
                    ////{
                    ////    var detail = _arndetailrepository.GetAll().Result.Where(b => b.ItemId == item.ItemId && b.ArnheaderId == arnID).
                    ////        Select(x => new { x.RecivedQty, x.RejectedQty, x.Remark }).FirstOrDefault();
                    ////    tmpArnDetail.RecivedQty = detail.RecivedQty;
                    ////    tmpArnDetail.RejectedQty = detail.RejectedQty;
                    ////    tmpArnDetail.Remark = detail.Remark;
                    ////    //tmpArnDetail.isEdit = true;
                    ////}

                    arndetailList.Add(tmpArnDetail);
                }

               DataTable dtarndetail = CommonGenericService<Arnheader>.ToDataTable(arndetailList);

                var dataTable = new DataGridTable
                {
                    rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                    enableSorting = true,
                    enableColResize = false,
                    suppressSizeToFit = true,
                    DataGridColumns = GetArndetailColumnsfromList(dtarndetail),
                    DataGridRows = GetArndetailRowsFromList(dtarndetail)
                };

                return dataTable;
            }
            catch (Exception ex)
            {

                return null;
            }

        }

        private List<DataGridColumn> GetArndetailColumnsfromList(DataTable dataTable)
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

                if (column.ToString().Equals("RecivedQty"))
                {
                    dataTableColumn.editable = true;
                    dataTableColumn.width = 140;
                    dataTableColumn.headerName = "Recived Qty";
                    //dataTableColumn.width = 100;
                }

                if (column.ToString().Equals("ItemDescription"))
                {
                    dataTableColumn.headerName = "Item Name";
                    dataTableColumn.width = 140;
                }

                if (column.ToString().Equals("InvoiceQty"))
                {
                    dataTableColumn.headerName = "Invoice Qty";
                    dataTableColumn.width = 140;
                }

                if (column.ToString().Equals("RejectedQty"))
                {
                    dataTableColumn.editable = true;
                    dataTableColumn.width = 140;
                    dataTableColumn.headerName = "Rejected Qty";
                    //dataTableColumn.width = 100;
                }
                if (column.ToString().Equals("Remark"))
                {
                    dataTableColumn.editable = true;
                    dataTableColumn.width = 300;
                }
                if (!column.ToString().Equals("ArndetailID")
                    && !column.ToString().Equals("ArnheaderID")
                    && !column.ToString().Equals("ItemID")
                    && !column.ToString().Equals("Qty"))
                    
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
                        
                        dataTableColumn.filter = "agTextColumnFilter";
                        break;
                }
                DataGridColumns.Add(dataTableColumn);
            }
            return DataGridColumns;
        }

        

        private static List<Object> GetArndetailRowsFromList(DataTable dataTable)
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
                       
                        rowValue = Convert.ToDateTime(rowValue).ToString("d", CultureInfo.CurrentCulture);
                    }

                    dictionary.Add(column.ToString().Replace(" ", "_"), rowValue);
                }
                dictionaryList.Add(dictionary);
            }
            return dictionaryList;
        }


        public async Task<DataGridTable> GetPOGrListGridAsync()
        {
            try
            {


                var poList = (await _poheaderrepository.GetAll()).Select(a => new PoHeaderResource()
                {


                    PoheaderID = a.PoheaderId,
                    QuotationRequestHeaderID = a.QuotationRequestHeaderId,
                    Ponumber = a.Ponumber,
                    TotalPoamount = a.TotalPoamount == null ? null : a.TotalPoamount,
                    RequestedDeliveryDate = a.RequestedDeliveryDate.HasValue ? a.RequestedDeliveryDate : Convert.ToDateTime("1900-01-01"),
                    PodateTime = a.PodateTime,
                    UserID = a.UserId == null ? null : a.UserId,
                    IsDeliver = a.IsDeliver,
                    IsEnter = a.IsEnter,
                    ActualDeliveryDate = a.ActualDeliveryDate.HasValue ? a.ActualDeliveryDate : Convert.ToDateTime("1900-01-01"),
                    PaymentMethodID = a.PaymentMethodId == null ? null : a.PaymentMethodId,
                    TaxAmount = a.TaxAmount == null ? null : a.TaxAmount,



                    Podetail = a.Podetail,


                    UserName = a.UserId == null ? "" : _userrepository.GetByIdAsync(a.UserId).Result.UserName.ToString(),
                    QuotationNumber = _quotationReqHeaderrepository.GetByIdAsync(a.QuotationRequestHeaderId).Result.QuotationNumber.ToString(),
                    PaymentMethodName = a.PaymentMethodId == null ? "" : _paymentmethodrepository.GetByIdAsync(a.PaymentMethodId).Result.PaymentMethodName.ToString(),

                    
                }).Where(a => a.IsDeliver.Equals(true) && a.IsEnter.Equals(false)).ToList();


                DataTable dtPO = CommonGenericService<PoHeaderResource>.ToDataTable(poList);

                var dataTable = new DataGridTable
                {
                    rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                    enableSorting = true,
                    enableColResize = false,
                    suppressSizeToFit = true,
                    DataGridColumns = GetPOColumnsfromList(dtPO),
                    DataGridRows = GetPOFromList(dtPO)
                };

                return dataTable;
            }
            catch (Exception ex)
            {

                return null;
            }

    }
    

        private List<DataGridColumn> GetPOColumnsfromList(DataTable dataTable)
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

                if (column.ToString().Equals("Ponumber"))
                {
                    dataTableColumn.headerName = "Po Number";
                    dataTableColumn.width = 120;
                }
                if (column.ToString().Equals("UserName"))
                {
                    dataTableColumn.headerName = "User Name";
                    dataTableColumn.width = 125;
                }
                if (column.ToString().Equals("PaymentMethodName"))
                {
                    dataTableColumn.headerName = "Payment Method";
                    dataTableColumn.width = 135;
                }
                if (column.ToString().Equals("QuotationNumber"))
                {
                    dataTableColumn.width = 150;
                    dataTableColumn.headerName = "Quotation Number";
                }

                if (!column.ToString().Equals("QuotationRequestHeaderID")
                     && !column.ToString().Equals("PoheaderID")
                     && !column.ToString().Equals("TotalPoamount")
                     && !column.ToString().Equals("RequestedDeliveryDate")
                     && !column.ToString().Equals("PodateTime")
                     && !column.ToString().Equals("PaymentMethodID")
                     && !column.ToString().Equals("UserID")
                     && !column.ToString().Equals("TaxAmount")
                     && !column.ToString().Equals("IsDeliver")
                     && !column.ToString().Equals("Podetail")
                     && !column.ToString().Equals("ActualDeliveryDate")
                     && !column.ToString().Equals("QuotationRequestDetails")
                     && !column.ToString().Equals("IsEnter"))
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

        private static List<Object> GetPOFromList(DataTable dataTable)
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


        public async Task<GenericSaveResponse<Arnheader>> SaveArnheaderAsync(Arnheader arnheader)
        {
            try
            {
                if (arnheader.ArnheaderId == Guid.Empty)
                {
                    arnheader.ArnheaderId = Guid.NewGuid();
                }

                foreach (Arndetail arndetail in arnheader.Arndetail)
                {
                    arndetail.ArndetailId = Guid.NewGuid();
                    Arndetail arn = new Arndetail();
                    arn.ArnheaderId = arnheader.ArnheaderId;

                    var getcuustock = (await _stockrepository.GetAll()).Where(a => a.ItemId == arndetail.ItemId).ToList();

                    foreach (Stock stck in getcuustock)
                    {
                        stck.StockQty = stck.StockQty + Convert.ToDouble(arndetail.RecivedQty);
                        stck.BalancedQty = stck.BalancedQty + Convert.ToDouble(arndetail.RecivedQty);
                        if (!(stck.StockQty >= 0))
                        {
                            return new GenericSaveResponse<Arnheader>($"This Item hasn't stock!.");
                        }
                        //Stock currStock = new Stock();
                        //currStock.StockQty = stck.StockQty;
                        //currStock.StockId = stck.StockId;
                        _stockrepository.Update(stck);

                        //_stockrepository.Update(currStock);

                    }
                    await _arndetailrepository.InsertAsync(arndetail);
                }

                Poheader existingPOheader = await _poheaderrepository.GetByIdAsync(arnheader.PoheaderId);
                existingPOheader.IsEnter = true;

                //------------ Add By Nipuna Francisku -----------------------------------------

                var ArnNoCount = (await _arnheaderrepository.GetAll()).Count() + 1;

                string Arnnumber = ArnNoCount.ToString().PadLeft(4, '0');
                arnheader.Arnnumber = Arnnumber;
                //------------------------------------------------------------------------------

                await _arnheaderrepository.InsertAsync(arnheader);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<Arnheader>(true, "Successfully Saved.", arnheader);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Arnheader>($"An error occured when saving the Supplier Master :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<Arnheader>> UpdateArnheaderAsync(string id, Arnheader arnheader)
        {
            try
            {
                Arnheader existingArn = await _arnheaderrepository.GetByIdAsync(arnheader.ArnheaderId);

                if (existingArn == null)
                    return new GenericSaveResponse<Arnheader>($"Supplier Master not found");

                ResourceComparer<Arnheader> Comparer = new ResourceComparer<Arnheader>(arnheader, existingArn);
                ResourceComparerResult<Arnheader> CompareResult = Comparer.GetUpdatedObject();

                if (CompareResult.Updated)
                {
                    _arnheaderrepository.Update(CompareResult.Obj);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<Arnheader>(arnheader);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Arnheader>($"An error occured when updating the Supplier Master :" + (ex.Message ?? ex.InnerException.Message));
            }


        }

        public async Task<GenericSaveResponse<Arnheader>> DeleteArnheaderAsync(string id, Arnheader arnheader)
        {
            try
            {
                Arnheader existingSupplier = await _arnheaderrepository.GetByIdAsync(arnheader.ArnheaderId);

                if (existingSupplier == null)
                {
                    return new GenericSaveResponse<Arnheader>($"Arn Header not found");
                }
                else
                {

                    _arnheaderrepository.Delete(arnheader.ArnheaderId);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<Arnheader>(arnheader);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Arnheader>($"An error occured when updating the Arn Header :" + (ex.Message ?? ex.InnerException.Message));
            }


        }


    }
}
