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
  public class MeasurementUnitsService : IMeasurementUnitServices
    {
        private IGenericRepo<MeasurementUnits> _repository = null;
        private IUnitOfWorks _unitOfWork;
        private IGenericRepo<ItemRequest> _itemRequestrepository = null; //--- Add By Nipuna Francisku
        private IGenericRepo<QuotationRequestDetails> _quotationrequestdetailsrepository = null; //--- Add By Nipuna Francisku
        private IGenericRepo<Podetail> _podetailrepository = null; //--- Add By Nipuna Francisku



        public MeasurementUnitsService(IGenericRepo<MeasurementUnits> repository, IUnitOfWorks unitfwork, IGenericRepo<ItemRequest> itemRequestrepository, IGenericRepo<QuotationRequestDetails> quotationrequestdetailsrepository, IGenericRepo<Podetail> podetailrepository)
        {
            this._repository = repository;
            this._unitOfWork = unitfwork;
            this._itemRequestrepository = itemRequestrepository; //--- Add By Nipuna Francisku
            this._quotationrequestdetailsrepository = quotationrequestdetailsrepository; //--- Add By Nipuna Francisku
            this._podetailrepository = podetailrepository; //--- Add By Nipuna Francisku
        }

        public async Task<IEnumerable<MeasurementUnits>> GetAllAsync()
        {
            var measurementunitgetall = (await _repository.GetAll()).Select(c => new MeasurementUnits
            {
                MeasurementUnitId = c.MeasurementUnitId,
                MeasurementUnitName = c.MeasurementUnitName,
                Code = c.Code,
                IsActive = c.IsActive.HasValue ? c.IsActive.Value : false,
                
            }).Where(d => d.IsActive == true).OrderBy(e => e.MeasurementUnitName);

            return measurementunitgetall;
            //return await _repository.GetAll();
        }

        public async Task<DataGridTable> GetMeasurementUnitGridAsync()
        {
            //var measurementUnittList = await _repository.GetAll();

            var measurementUnittList = (await _repository.GetAll()).Select(a => new MeasurementUnitResource
            {
                MeasurementUnitID = a.MeasurementUnitId,
                MeasurementUnitName = a.MeasurementUnitName,
                Code = a.Code,
                UserID = a.UserId,
                EntryDateTime = a.EntryDateTime.HasValue ? a.EntryDateTime : Convert.ToDateTime("2000-01-01"),
                IsActive = a.IsActive.HasValue ? a.IsActive.Value : false,
                Status = a.IsActive == true ? "Active" : "Inactive"
            }).OrderBy(b => b.MeasurementUnitName).ToList();

            //--------- Add By Nipuna Francisku --------------------------------
            foreach (var q in measurementUnittList)
            {
                var QuotationReqList = (await _quotationrequestdetailsrepository.GetAll()).Select(b => new QuotationRequestDetails() //-- Check QuotationRequestDetails
                {
                    MeasurementUnitId = b.MeasurementUnitId,
                    QuotationRequestHeaderId = b.QuotationRequestHeaderId,
                    QuotationRequestDetailId = b.QuotationRequestDetailId,

                }).Where(d => d.MeasurementUnitId == q.MeasurementUnitID).ToList();

                if (QuotationReqList.Count != 0)
                {
                    q.IsTansactions = true;
                }
                else
                {
                    q.IsTansactions = false;
                }
            }
            //-------------------------------------------------------------------


            DataTable dtMeasurementUnit = CommonGenericService<MeasurementUnits>.ToDataTable(measurementUnittList);

            var dataTable = new DataGridTable
            {
                rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                enableSorting = true,
                enableColResize = false,
                DataGridColumns = GetMeasurementUnitColumnsfromList(dtMeasurementUnit),
                DataGridRows = GetMeasurementUnitRowsFromList(dtMeasurementUnit)
            };

            return dataTable;
        }

        private List<DataGridColumn> GetMeasurementUnitColumnsfromList(DataTable dataTable)
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

                if (column.ToString().Equals("MeasurementUnitName"))
                {
                    dataTableColumn.headerName = "Measurement Unit Name";
                }
                if (column.ToString().Equals("Code"))
                {
                    dataTableColumn.headerName = "Code";
                }
                if (column.ToString().Equals("Status"))
                {
                    dataTableColumn.headerName = "Status";
                }

                if (!column.ToString().Equals("MeasurementUnitID")
                    && !column.ToString().Equals("ItemType")
                    && !column.ToString().Equals("IsActive")
                    && !column.ToString().Equals("UserID")
                    && !column.ToString().Equals("EntryDateTime")
                    && !column.ToString().Equals("QuotationRequestDetails")
                    && !column.ToString().Equals("IsTansactions") //--------- Add By Nipuna Francisku 
                    && !column.ToString().Equals("Accers"))
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

        private static List<Object> GetMeasurementUnitRowsFromList(DataTable dataTable)
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


        public async Task<GenericSaveResponse<MeasurementUnits>> SaveMeasurementUnitsAsync(MeasurementUnits measurementunits)
        {
            try
            {
                if (measurementunits.MeasurementUnitId == Guid.Empty)
                {
                    measurementunits.MeasurementUnitId = Guid.NewGuid();
                }

                 var getallMeasurementUnitName = (await _repository.GetAll()).Where(d => d.MeasurementUnitName == measurementunits.MeasurementUnitName).ToList();

                if (getallMeasurementUnitName.Count != 0)
                {
                    return new GenericSaveResponse<MeasurementUnits>($"The MeasurementUnit Name already exists. Please use a different MeasurementUnit Name");
                }

                var getallCode = (await _repository.GetAll()).Where(d => d.Code == measurementunits.Code).ToList();

                if (getallCode.Count != 0)
                {
                    return new GenericSaveResponse<MeasurementUnits>($"The MeasurementUnit Code already exists. Please use a different  MeasurementUnit Code");
                }
                measurementunits.EntryDateTime = DateTime.Now;
                await _repository.InsertAsync(measurementunits);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<MeasurementUnits>(true, "Successfully Saved.", measurementunits);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<MeasurementUnits>($"An error occured when saving the Category Master :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<MeasurementUnits>> UpdateMeasurementUnitsAsync(string id, MeasurementUnits measurementunits)
        {
            try
            {
                MeasurementUnits existingMeasurementUnits = await _repository.GetByIdAsync(measurementunits.MeasurementUnitId);

                if (existingMeasurementUnits == null)
                    return new GenericSaveResponse<MeasurementUnits>($"Measurement Units not found");

               

                var getallMeasurementUnitName = (await _repository.GetAll()).Where(d => d.MeasurementUnitName == measurementunits.MeasurementUnitName && d.MeasurementUnitId != existingMeasurementUnits.MeasurementUnitId).ToList();

                if (getallMeasurementUnitName.Count != 0)
                {
                    return new GenericSaveResponse<MeasurementUnits>($"The MeasurementUnit Name already exists. Please use a different MeasurementUnit Name");
                }

                var getallCode = (await _repository.GetAll()).Where(d => d.Code == measurementunits.Code && d.MeasurementUnitId != existingMeasurementUnits.MeasurementUnitId).ToList();

                if (getallCode.Count != 0)
                {
                    return new GenericSaveResponse<MeasurementUnits>($"The MeasurementUnit Code already exists. Please use a different  MeasurementUnit Code");
                }
                measurementunits.EntryDateTime = DateTime.Now;
                ResourceComparer<MeasurementUnits> Comparer = new ResourceComparer<MeasurementUnits>(measurementunits, existingMeasurementUnits);
                ResourceComparerResult<MeasurementUnits> CompareResult = Comparer.GetUpdatedObject();

                if (CompareResult.Updated)
                {
                    _repository.Update(CompareResult.Obj);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<MeasurementUnits>(measurementunits);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<MeasurementUnits>($"An error occured when updating the Measurement Units :" + (ex.Message ?? ex.InnerException.Message));
            }


        }

        public async Task<GenericSaveResponse<MeasurementUnits>> DeleteMeasurementUnitsAsync(string id, MeasurementUnits measurementunits)
        {
            try
            {
                MeasurementUnits existingMeasurementUnits = await _repository.GetByIdAsync(measurementunits.MeasurementUnitId);

                if (existingMeasurementUnits == null)
                    return new GenericSaveResponse<MeasurementUnits>($"Payment measurement units not found");

                else
                    _repository.Delete(measurementunits.MeasurementUnitId);
                await _unitOfWork.CompleteAsync();
                return new GenericSaveResponse<MeasurementUnits>(measurementunits);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<MeasurementUnits>($"An error occured when deleting the measurement units  :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

    }
}
