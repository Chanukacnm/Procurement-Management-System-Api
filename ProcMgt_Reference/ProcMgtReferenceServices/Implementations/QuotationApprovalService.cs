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
    public class QuotationApprovalService : IQuotationApprovalServices
    {
        private IGenericRepo<QuotationRequestHeader> _repository = null;
        private IUnitOfWorks _unitOfWork;
        private IGenericRepo<User> _userrepository = null;
        private IGenericRepo<Supplier> _supplierrepository = null;
        private IGenericRepo<QuotationRequestStatus> _quotationrequeststatusrepository = null;

        public QuotationApprovalService(IGenericRepo<QuotationRequestHeader> repository, IGenericRepo<User> userrepository, IGenericRepo<Supplier> supplierrepository, IGenericRepo<QuotationRequestStatus> quotationrequeststatusrepository, IGenericRepo<QuotationRequestDetails> quotationreqdetailsrepository, IUnitOfWorks unitfwork)
        {
            this._repository = repository;
            this._unitOfWork = unitfwork;
            this._userrepository = userrepository;
            this._supplierrepository = supplierrepository;
            this._quotationrequeststatusrepository = quotationrequeststatusrepository;
        }

        public async Task<IEnumerable<QuotationRequestHeader>> GetAllAsync()
        {
            return await _repository.GetAll();
           
        }

        public async Task<DataGridTable> GetQuotationApprovalGridAsync()
        {
            //var quotApprovaltList = await _repository.GetAll();

            var quotApprovaltList = (await _repository.GetAll()).Select(a => new QuotationRequestHeaderResource()
            {
                QuotationRequestHeaderID = a.QuotationRequestHeaderId,
                SupplierID = a.SupplierId,
                QuotationNumber = a.QuotationNumber,
                UserID = a.UserId,
                QuotationRequestStatusID = a.QuotationRequestStatusId,
                //IsCanceled = a.IsCanceled,
                IsEnteringCompleted = a.IsEnteringCompleted,

                UserName = _userrepository.GetByIdAsync(a.UserId).Result.UserName.ToString(),
                QuotationRequestedDate = a.QuotationRequestedDate,
                SupplierName = _supplierrepository.GetByIdAsync(a.SupplierId).Result.SupplierName.ToString(),
                RequiredDate = a.RequiredDate,
                QuotationRequestStatus1 = _quotationrequeststatusrepository.GetByIdAsync(a.QuotationRequestStatusId).Result.QuotationRequestStatus1.ToString()

            }).Where(a => a.QuotationRequestStatusID == 1 && a.IsEnteringCompleted == true).OrderBy(x => x.QuotationRequestedDate).ToList();
            DataTable dtQuotApproval = CommonGenericService<QuotationRequestHeader>.ToDataTable(quotApprovaltList);

            var dataTable = new DataGridTable
            {
                rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                enableSorting = true,
                enableColResize = false,
                DataGridColumns = GetQuotApprovalColumnsfromList(dtQuotApproval),
                DataGridRows = GetQuotApprovalRowsFromList(dtQuotApproval)
            };

            return dataTable;
        }

        private List<DataGridColumn> GetQuotApprovalColumnsfromList(DataTable dataTable)
        {

            var DataGridColumns = new List<DataGridColumn>();

            
            foreach (DataColumn column in dataTable.Columns)
            {
                var dataTableColumn = new DataGridColumn
                {
                    field = column.ToString().Replace(" ", "_"),
                    headerName = column.ToString(),
                    width = 120,
                    
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
                    dataTableColumn.width = 150;
                    dataTableColumn.headerName = "Supplier Name";

                }
                   
                if (column.ToString().Equals("UserName"))
                {
                    dataTableColumn.width = 150;
                    dataTableColumn.headerName = "User Name";

                }
                
                if (column.ToString().Equals("RequiredDate"))
                {
                    dataTableColumn.width = 170;
                    dataTableColumn.headerName = "Required Date";

                }
      
              

                if (column.ToString().Equals("QuotationCompleted"))
                {
                    dataTableColumn.width = 170;
                    dataTableColumn.headerName = "Quotation Completed";

                }

                if (column.ToString().Equals("QuotationRequestStatus1"))
                {
                    dataTableColumn.width = 160;
                    dataTableColumn.headerName = "Quotation Status";
                     
                }

                if (!column.ToString().Equals("SupplierID")
                    && !column.ToString().Equals("UserID")
                     && !column.ToString().Equals("IsCanceled")
                     && !column.ToString().Equals("QuotationCompleted")
                      && !column.ToString().Equals("QuotationRequestStatusID")
                       && !column.ToString().Equals("QuotationRequestHeaderID")
                         && !column.ToString().Equals("ApprovalComment")
                         && !column.ToString().Equals("IsEnteringCompleted")
                           && !column.ToString().Equals("QuotationRequestDetails"))
               
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
                        
                        dataTableColumn.filter = "agTextColumnFilter";
                        break;
                }

                DataGridColumns.Add(dataTableColumn);
            }
            return DataGridColumns;
        }

        private static List<Object> GetQuotApprovalRowsFromList(DataTable dataTable)
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
