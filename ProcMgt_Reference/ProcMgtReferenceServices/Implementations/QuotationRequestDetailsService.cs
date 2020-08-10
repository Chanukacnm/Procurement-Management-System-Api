using ProcMgt_Reference_Core;
using ProcMgt_Reference_Core.GenericRepoInter;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Common;
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
    public class QuotationRequestDetailsService : IQuotationRequestDetailsServices
    {
        private IGenericRepo<QuotationRequestDetails> _detrepository = null;
        private IUnitOfWorks _unitOfWork;
        private IGenericRepo<Item> _itemrepository = null;
        private IGenericRepo<MeasurementUnits> _measurementUnitrepository = null;
        private IGenericRepo<Model> _modelrepository = null;
        private IGenericRepo<Make> _makerepository = null;

        public QuotationRequestDetailsService(IGenericRepo<QuotationRequestDetails> detrepository, IGenericRepo<Item> itemrepository, IGenericRepo<MeasurementUnits> measurementunitrepository,
            IGenericRepo<Model> modelrepository, IGenericRepo<Make> makerepository, IUnitOfWorks unitfwork)
        {
            this._detrepository = detrepository;
            this._unitOfWork = unitfwork;
            this._itemrepository = itemrepository;
            this._makerepository = makerepository;
            this._modelrepository = modelrepository;
            this._measurementUnitrepository = measurementunitrepository;
        }

        public async Task<IEnumerable<QuotationRequestDetails>> GetAllAsync()
        {
            return await _detrepository.GetAll();
        }

        public async Task<DataGridTable> GetQuotationRequestDetailsGrid()
        {
            try
            {
                var quotationRequestDetailsList = (await _detrepository.GetAll()).Select(a => new QuotationRequestDetailsResource()
                {
                    QuotationRequestDetailID = a.QuotationRequestDetailId,
                    QuotationRequestHeaderID = a.QuotationRequestHeaderId,
                    MakeID = a.MakeId ?? Guid.Empty,
                    ModelID = a.ModelId ?? Guid.Empty,
                    ItemID = a.ItemId,
                    MeasurementUnitID = a.MeasurementUnitId,
                    UnitPrice = a.UnitPrice,                    
                    GrossAmount = a.GrossAmount,
                    NetAmount = a.NetAmount,
                    DiscountAmount = a.DiscountAmount,
                    QuotationValidDate = a.QuotationValidDate.HasValue ? a.QuotationValidDate : Convert.ToDateTime("1900-01-01"),
                    Attachment = a.Attachment,

                    makeName = a.MakeId == null ? "" : _makerepository.GetByIdAsync(a.MakeId).Result.MakeName.ToString(),
                    modelName = a.ModelId == null ? "" : _modelrepository.GetByIdAsync(a.ModelId).Result.ModelName.ToString(),
                    itemDescription = _itemrepository.GetByIdAsync(a.ItemId).Result.ItemDescription.ToString(),
                    Quantity = a.Quantity,
                    measurementUnitName = _measurementUnitrepository.GetByIdAsync(a.MeasurementUnitId).Result.MeasurementUnitName.ToString()
                });
                 
                DataTable dtQuotationRequestDetails = CommonGenericService<QuotationRequestDetails>.ToDataTable(quotationRequestDetailsList);

                var dataTable = new DataGridTable
                {
                    rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                    enableSorting = true,
                    enableColResize = false,
                    suppressSizeToFit = true,
                    DataGridColumns = GetQuotationRequestDetailsColumnsfromList(dtQuotationRequestDetails),
                    DataGridRows = GetQuotationRequestDetailsRowsFromList(dtQuotationRequestDetails)
                };

                return dataTable;
            }
            catch (Exception ex)
            {
                return null;
            }
            //var quotationRequestDetailsList = await _repository.GetAll();

            
        }

        private List<DataGridColumn> GetQuotationRequestDetailsColumnsfromList(DataTable dataTable)
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
                    width = 110,
                    //visible = 

                    //halign = DataAlignEnum.Center.ToString().ToLower()
                };

                if (column.ToString().Equals("itemDescription"))
                {
                    dataTableColumn.width = 140;
                    dataTableColumn.headerName = "Item Description";
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
                    dataTableColumn.width = 90;
                    dataTableColumn.headerName = "Quantity";
                }

                if (!column.ToString().Equals("QuotationRequestDetailID")
                    && !column.ToString().Equals("QuotationRequestHeaderID")
                    && !column.ToString().Equals("DiscountAmount")
                    && !column.ToString().Equals("QuotationValidDate")
                    && !column.ToString().Equals("Attachment")
                    && !column.ToString().Equals("Item")
                    && !column.ToString().Equals("Make")
                    && !column.ToString().Equals("MeasurementUnit")
                    && !column.ToString().Equals("Model")
                    && !column.ToString().Equals("QuotationRequestHeader")
                    && !column.ToString().Equals("UnitPrice")
                    && !column.ToString().Equals("GrossAmount")
                    && !column.ToString().Equals("NetAmount")
                    && !column.ToString().Equals("PodetailId")
                    && !column.ToString().Equals("PounitPrice")
                    && !column.ToString().Equals("Qty")
                    && !column.ToString().Equals("ItemID")
                    && !column.ToString().Equals("MeasurementUnitID")
                    && !column.ToString().Equals("MakeID")
                    && !column.ToString().Equals("ModelID")
                    && !column.ToString().Equals("TotalAmount"))
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

        private static List<Object> GetQuotationRequestDetailsRowsFromList(DataTable dataTable)
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
