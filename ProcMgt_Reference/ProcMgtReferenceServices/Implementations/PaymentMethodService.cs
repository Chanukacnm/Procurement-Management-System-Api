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
   public class PaymentMethodService : IPaymentMethodServices
    {
        private IGenericRepo<PaymentMethod> _repository = null;
        private IUnitOfWorks _unitOfWork;
        private IGenericRepo<Poheader> _poheaderrepository = null; //--- Add By Nipuna Francisku

        public PaymentMethodService(IGenericRepo<PaymentMethod> repository, IUnitOfWorks unitfwork,IGenericRepo<Poheader> poheaderrepository)
        { 
            this._repository = repository;
            this._unitOfWork = unitfwork;
            this._poheaderrepository = poheaderrepository; //--- Add By Nipuna Francisku
        }

        public async Task<IEnumerable<PaymentMethod>> GetAllAsync()
        {

            var getallPaymentMethod = (await _repository.GetAll()).Select(c => new PaymentMethod
            {
                PaymentMethodId = c.PaymentMethodId,
                PaymentMethodName = c.PaymentMethodName,
                IsActive = c.IsActive.HasValue ? c.IsActive.Value : false
            }).Where(d => d.IsActive == true).OrderBy(e => e.PaymentMethodName);
            
            return getallPaymentMethod; 
        }

        public async Task<DataGridTable> GetPaymentMethodGridAsync()
        {
            //var paymentMethodList = await _repository.GetAll();

            var paymentMethodList = (await _repository.GetAll()).Select(a => new PaymentMethodResource
            {
                PaymentMethodID = a.PaymentMethodId,
                PaymentMethodCode = a.PaymentMethodCode,
                PaymentMethodName = a.PaymentMethodName,
                IsActive = a.IsActive.HasValue ? a.IsActive.Value : false, 
                Status = a.IsActive == true ? "Active" : "Inactive",
                UserID = a.UserId,
                EntryDateTime = a.EntryDateTime.HasValue ? a.EntryDateTime : Convert.ToDateTime("2000-01-01"),
            }).OrderBy(b => b.PaymentMethodName).ToList();

            //--------- Add By Nipuna Francisku --------------------------------
            foreach (var q in paymentMethodList)
            {
                var PoheaderList = (await _poheaderrepository.GetAll()).Select(b => new Poheader() //-- Check Poheader
                {
                    PaymentMethodId = b.PaymentMethodId,
                    PoheaderId = b.PoheaderId

                }).Where(d => d.PaymentMethodId == q.PaymentMethodID).ToList();

                if ( PoheaderList.Count != 0)
                {
                    q.IsTansactions = true;
                }
                else
                {
                    q.IsTansactions = false;
                }
            }
            //-------------------------------------------------------------------

            DataTable dtPaymentMethod = CommonGenericService<PaymentMethod>.ToDataTable(paymentMethodList);

            var dataTable = new DataGridTable
            {
                rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                enableSorting = true,
                enableColResize = false,
                DataGridColumns = GetPaymentMethodColumnsfromList(dtPaymentMethod),
                DataGridRows = GetPaymentMethodRowsFromList(dtPaymentMethod)
            };

            return dataTable;
        }

        private List<DataGridColumn> GetPaymentMethodColumnsfromList(DataTable dataTable)
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

               

                if (column.ToString().Equals("PaymentMethodName"))
                {
                    dataTableColumn.headerName = "Payment Method Name";
                }

                if (column.ToString().Equals("PaymentMethodCode"))
                {
                    dataTableColumn.headerName = "Payment Method Code";
                }

                  if (column.ToString().Equals("Status"))
                {
                    dataTableColumn.headerName = "Status";
                }

                if(!column.ToString().Equals("PaymentMethodID")
                    &&!column.ToString().Equals("Supplier")
                    && !column.ToString().Equals("IsActive")
                    && !column.ToString().Equals("UserID")
                    && !column.ToString().Equals("EntryDateTime")
                    && !column.ToString().Equals("IsTansactions") //--------- Add By Nipuna Francisku 
                    && !column.ToString().Equals("Poheader"))

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

        private static List<Object> GetPaymentMethodRowsFromList(DataTable dataTable)
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


        public async Task<GenericSaveResponse<PaymentMethod>> SavePaymentMethodAsync(PaymentMethod paymentmethod)
        {
            try
            {
                if (paymentmethod.PaymentMethodId == Guid.Empty)
                {
                    paymentmethod.PaymentMethodId = Guid.NewGuid();
                }

               

                var getallPaymentMethodNames = (await _repository.GetAll()).Where(d => d.PaymentMethodName == paymentmethod.PaymentMethodName).ToList();
           
                if (getallPaymentMethodNames.Count != 0)
                {
                    return new GenericSaveResponse<PaymentMethod>($"Payment Method Name already exists. Please Re Enter");
                }

                var getallempNO = (await _repository.GetAll()).Where(d => d.PaymentMethodCode == paymentmethod.PaymentMethodCode).ToList();

                if (getallempNO.Count != 0)
                {
                    return new GenericSaveResponse<PaymentMethod>($"Payment Method Code already exists. Please Re Enter");
                }
                paymentmethod.EntryDateTime = DateTime.Now;
                await _repository.InsertAsync(paymentmethod);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<PaymentMethod>(true, "Successfully Saved.", paymentmethod);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<PaymentMethod>($"An error occured when saving the Payment Method :" + (ex.Message ?? ex.InnerException.Message));
            }
        }


  
        public async Task<GenericSaveResponse<PaymentMethod>> DeletePaymentMethodAsync(PaymentMethod paymentmethod, string id)
        {
            try
            {
                PaymentMethod existingPaymentMethod = await _repository.GetByIdAsync(paymentmethod.PaymentMethodId);

                if (existingPaymentMethod == null)
                    return new GenericSaveResponse<PaymentMethod>($"Payment Method not found");

                else

                _repository.Delete(paymentmethod.PaymentMethodId);
                await _unitOfWork.CompleteAsync();
                return new GenericSaveResponse<PaymentMethod>(paymentmethod);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<PaymentMethod>($"An error occured when deleting the Payment Method :" + (ex.Message ?? ex.InnerException.Message));
            }
        }


        public async Task<GenericSaveResponse<PaymentMethod>> UpdatePaymentMethodAsync(string id, PaymentMethod paymentmethod)
        {
            try
            {
                PaymentMethod existingPaymentMethod = await _repository.GetByIdAsync(paymentmethod.PaymentMethodId);

                if (existingPaymentMethod == null)
                    return new GenericSaveResponse<PaymentMethod>($"Payment Method not found");

                var getallPaymentMethodNames = (await _repository.GetAll()).Where(d => d.PaymentMethodName == paymentmethod.PaymentMethodName && d.PaymentMethodId != existingPaymentMethod.PaymentMethodId).ToList();

                if (getallPaymentMethodNames.Count != 0)
                {
                    return new GenericSaveResponse<PaymentMethod>($"Payment Method Name already exists. Please Re Enter");
                }

                var getallempNO = (await _repository.GetAll()).Where(d => d.PaymentMethodCode == paymentmethod.PaymentMethodCode && d.PaymentMethodId  != existingPaymentMethod.PaymentMethodId).ToList();

                if (getallempNO.Count != 0)
                {
                    return new GenericSaveResponse<PaymentMethod>($"Payment Method Code already exists. Please Re Enter");
                }
                paymentmethod.EntryDateTime = DateTime.Now; 
                ResourceComparer<PaymentMethod> Comparer = new ResourceComparer<PaymentMethod>(paymentmethod, existingPaymentMethod);
                ResourceComparerResult<PaymentMethod> CompareResult = Comparer.GetUpdatedObject();

                if (CompareResult.Updated)
                {
                    _repository.Update(CompareResult.Obj);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<PaymentMethod>(paymentmethod);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<PaymentMethod>($"An error occured when updating the Payment Method :" + (ex.Message ?? ex.InnerException.Message));
            }


        }
    }


}

