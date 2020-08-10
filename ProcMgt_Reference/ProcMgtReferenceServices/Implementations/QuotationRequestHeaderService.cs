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
    public class QuotationRequestHeaderService : IQuotationRequestHeaderServices
    {
        private IGenericRepo<QuotationRequestHeader> _repository = null;
        private IGenericRepo<QuotationRequestDetails> _quotationreqdetailsrepository = null;
        private IUnitOfWorks _unitOfWork;
        private IGenericRepo<User> _userrepository = null;
        private IGenericRepo<Supplier> _supplierrepository = null;
        private IGenericRepo<QuotationRequestStatus> _quotationrequeststatusrepository = null;


        public QuotationRequestHeaderService(IGenericRepo<QuotationRequestHeader> repository, IGenericRepo<User> userrepository, IGenericRepo<Supplier> supplierrepository, IGenericRepo<QuotationRequestStatus> quotationrequeststatusrepository, IGenericRepo<QuotationRequestDetails> quotationreqdetailsrepository, IUnitOfWorks unitfwork)
        {
            this._repository = repository;
            this._quotationreqdetailsrepository = quotationreqdetailsrepository;
            this._unitOfWork = unitfwork;
            this._userrepository = userrepository;
            this._supplierrepository = supplierrepository;
            this._quotationrequeststatusrepository = quotationrequeststatusrepository;
        }

        public async Task<IEnumerable<QuotationRequestHeader>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

       
        public async Task<DataGridTable> GetQuotationRequestHeaderGridAsync()
        {
            try
            {
                var QuatationReqList = (await _repository.GetAll()).Select(a => new QuotationRequestHeaderResource()
                {
                    QuotationRequestHeaderID = a.QuotationRequestHeaderId,
                    SupplierID = a.SupplierId,                    
                    QuotationNumber = a.QuotationNumber,
                    UserID = a.UserId,                   
                    QuotationRequestStatusID = a.QuotationRequestStatusId,
                    QuotationRequestedDate = a.QuotationRequestedDate,
                    RequiredDate = a.RequiredDate,
                    ApprovalComment = a.ApprovalComment,
                    IsEnteringCompleted = a.IsEnteringCompleted.HasValue ? a.IsEnteringCompleted.Value : false,
                    

                    UserName = _userrepository.GetByIdAsync(a.UserId).Result.UserName.ToString(),
                    SupplierName = _supplierrepository.GetByIdAsync(a.SupplierId).Result.SupplierName.ToString(),
                    QuotationRequestStatus1 = _quotationrequeststatusrepository.GetByIdAsync(a.QuotationRequestStatusId).Result.QuotationRequestStatus1.ToString()

                }).OrderByDescending(c => c.QuotationRequestedDate);

                DataTable dtQuotationReq = CommonGenericService<QuotationRequestHeader>.ToDataTable(QuatationReqList);

                var dataTable = new DataGridTable
                {
                    rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                    enableSorting = true,
                    enableColResize = false,
                    suppressSizeToFit = true,
                    DataGridColumns = GetQuatationReqColumnsfromList(dtQuotationReq),
                    DataGridRows = GetQuatationReqrRowsFromList(dtQuotationReq)
                };

                return dataTable;

            }
            catch (Exception ex)
            {
                return null;
            }
            

            
        }


        private List<DataGridColumn> GetQuatationReqColumnsfromList(DataTable dataTable)
        {
            var DataGridColumns = new List<DataGridColumn>();

            foreach (DataColumn column in dataTable.Columns)
            {
                var dataTableColumn = new DataGridColumn
                {
                    field = column.ToString().Replace(" ", "_"),
                    headerName = column.ToString(),
                    width = 115,
                };

                if (column.ToString().Equals("QuotationRequestedDate"))
                {
                    dataTableColumn.width = 190;
                    dataTableColumn.headerName = "Quotation Requested Date";
                }
                if (column.ToString().Equals("SupplierName"))
                {
                    dataTableColumn.width = 120;
                    dataTableColumn.headerName = "Supplier Name";
                }
                if (column.ToString().Equals("IsEnteringCompleted"))
                {
                    dataTableColumn.width = 130;
                    dataTableColumn.headerName = "IsEntering Completed?";
                }
                if (column.ToString().Equals("QuotationRequestStatus1"))
                {
                    dataTableColumn.width = 180;
                    dataTableColumn.headerName = "Quotation Request Status";
                }
                if (column.ToString().Equals("ApprovalComment"))
                {
                    dataTableColumn.width = 210;
                    dataTableColumn.headerName = "Approval Comment";
                }
                if (column.ToString().Equals("UserName"))
                {
                    dataTableColumn.width = 120;
                    dataTableColumn.headerName = "User Name";
                }
                if (column.ToString().Equals("RequiredDate"))
                {
                    dataTableColumn.width = 120;
                    dataTableColumn.headerName = "Required Date";
                }


                if (!column.ToString().Equals("QuotationRequestHeaderID")
                    && !column.ToString().Equals("SupplierID")
                    && !column.ToString().Equals("QuotationNumber")
                    && !column.ToString().Equals("Supplier")
                    && !column.ToString().Equals("IsEnteringCompleted")
                    && !column.ToString().Equals("SupplierName")
                    && !column.ToString().Equals("User")
                    && !column.ToString().Equals("QuotationCompleted")
                    && !column.ToString().Equals("Poheader")
                    && !column.ToString().Equals("QuotationRequestDetails")
                    && !column.ToString().Equals("QuotationRequestStatusID")
                    && !column.ToString().Equals("QuotationRequestStatus1") //Added by Nipuna Francisku
                    && !column.ToString().Equals("UserID"))
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

        private static List<Object> GetQuatationReqrRowsFromList(DataTable dataTable)
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

        public async Task<GenericSaveResponse<QuotationRequestHeader>> SaveQuotationRequestHeaderAsync(QuotationRequestHeader quotationrequestheader)
        {

            try
            {
                if (quotationrequestheader.QuotationRequestHeaderId == Guid.Empty)
                {
                    quotationrequestheader.QuotationRequestHeaderId = Guid.NewGuid();
                }

                var quotationNoCount = (await _repository.GetAll()).Count()+1;
                

                //int i = this.GetQuotationCount();
                string s = quotationNoCount.ToString().PadLeft(4, '0');
                quotationrequestheader.QuotationNumber = s;

                foreach (QuotationRequestDetails quotationDet in quotationrequestheader.QuotationRequestDetails)
                {
                    quotationDet.QuotationRequestDetailId = Guid.NewGuid();
                    QuotationRequestDetails quoDet = new QuotationRequestDetails();
                    quoDet.QuotationRequestHeaderId = quotationrequestheader.QuotationRequestHeaderId;

                    await _quotationreqdetailsrepository.InsertAsync(quotationDet);
                }
                

                await _repository.InsertAsync(quotationrequestheader);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<QuotationRequestHeader>(true, "Successfully Saved.", quotationrequestheader);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<QuotationRequestHeader>($"An error occured when saving the Quotation Request :" + (ex.Message ?? ex.InnerException.Message));
            }

        }

        public async Task<GenericSaveResponse<QuotationRequestHeader>> UpdateQuotationRequestHeaderAsync(string id, QuotationRequestHeader quotationrequestheader)
        {
            try
            {
                QuotationRequestHeader existingQuotationReqHeader = await _repository.GetByIdAsync(quotationrequestheader.QuotationRequestHeaderId);

                if (existingQuotationReqHeader == null)
                    return new GenericSaveResponse<QuotationRequestHeader>($" Quotation Request not found");


                ResourceComparer<QuotationRequestHeader> Comparer = new ResourceComparer<QuotationRequestHeader>(quotationrequestheader, existingQuotationReqHeader);
                ResourceComparerResult<QuotationRequestHeader> CompareResult = Comparer.GetUpdatedObject();

                if (CompareResult.Updated)
                {
                    _repository.Update(CompareResult.Obj);
                    await _unitOfWork.CompleteAsync();
                }
                
                return new GenericSaveResponse<QuotationRequestHeader>(quotationrequestheader);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<QuotationRequestHeader>($"An error occured when updating the Quotation Request :" + (ex.Message ?? ex.InnerException.Message));
            }


        }

        public async Task<GenericSaveResponse<QuotationRequestHeader>> UpdateQuotationRequestDetailsrAsync(string id, QuotationRequestHeader quotationrequestheader)
        {
            try
            {
                

                foreach (QuotationRequestDetails quotationDet in quotationrequestheader.QuotationRequestDetails)
                {

                    QuotationRequestDetails existingQuotationEnter = await _quotationreqdetailsrepository.GetByIdAsync(quotationDet.QuotationRequestDetailId);

                    if (existingQuotationEnter == null)
                        return new GenericSaveResponse<QuotationRequestHeader>($" Quotation Request not found");

                    ResourceComparer<QuotationRequestDetails> Comparer2 = new ResourceComparer<QuotationRequestDetails>(quotationDet, existingQuotationEnter);
                    ResourceComparerResult<QuotationRequestDetails> CompareResult2 = Comparer2.GetUpdatedObject();

                    
                    if (CompareResult2.Updated)
                    {
                        _quotationreqdetailsrepository.Update(CompareResult2.Obj);

                        
                    }


                }



                QuotationRequestHeader existingQuotationReqHeader = await _repository.GetByIdAsync(quotationrequestheader.QuotationRequestHeaderId);
                existingQuotationReqHeader.IsEnteringCompleted = true;

                _repository.Update(existingQuotationReqHeader);

                await _unitOfWork.CompleteAsync();
                return new GenericSaveResponse<QuotationRequestHeader>(quotationrequestheader);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<QuotationRequestHeader>($"An error occured when updating the Quotation Request :" + (ex.Message ?? ex.InnerException.Message));
            }


        }

        public async Task<GenericSaveResponse<QuotationRequestHeader>> DeleteQuotationRequestHeaderAsync(string id, QuotationRequestHeader quotationrequestheader)
        {
            try
            {
                QuotationRequestHeader existingQuotationReqHeader = await _repository.GetByIdAsync(quotationrequestheader.QuotationRequestHeaderId);

                if (existingQuotationReqHeader == null)
                {
                    return new GenericSaveResponse<QuotationRequestHeader>($"Quotation Request not found");
                }
                else
                {

                    _repository.Delete(quotationrequestheader.QuotationRequestHeaderId);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<QuotationRequestHeader>(quotationrequestheader);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<QuotationRequestHeader>($"An error occured when updating the Quotation Request :" + (ex.Message ?? ex.InnerException.Message));
            }


        }



    }
}
