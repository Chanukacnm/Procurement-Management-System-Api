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
   public class TaxService : ITaxServices
    {
        private IGenericRepo<Tax> _repository = null;
        private IUnitOfWorks _unitOfWork;

        public TaxService(IGenericRepo<Tax> repository, IUnitOfWorks unitfwork)
        {
            this._repository = repository;
            this._unitOfWork = unitfwork;
        }

        public async Task<IEnumerable<Tax>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<DataGridTable> GetTaxGridAsync()
        {
            //var taxtList = await _repository.GetAll();

            var taxtList=(await _repository.GetAll()).Select(a => new TaxResource
            {
                TaxID = a.TaxId,
                TaxCode = a.TaxCode,
                TaxName = a.TaxName,
                Percentage = a.Percentage,
                UserID = a.UserId,
                EntryDateTime = a.EntryDateTime.HasValue ? a.EntryDateTime : Convert.ToDateTime("2000-01-01"),
            }).OrderBy(b => b.TaxName);


            DataTable dtTax = CommonGenericService<Tax>.ToDataTable(taxtList);

            var dataTable = new DataGridTable
            {
                rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                enableSorting = true,
                enableColResize = false,
                DataGridColumns = GetTaxColumnsfromList(dtTax),
                DataGridRows = GetTaxRowsFromList(dtTax)
            };

            return dataTable;
        }

        private List<DataGridColumn> GetTaxColumnsfromList(DataTable dataTable)
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

                if (column.ToString().Equals("TaxName"))
                {
                    dataTableColumn.headerName = "Tax Name";
                }

                 if (column.ToString().Equals("TaxCode"))
                {
                    dataTableColumn.headerName = "Tax Code";
                }

                if (column.ToString().Equals("Percentage"))
                {
                    dataTableColumn.headerName = "Percentage %";
                }

                if (!column.ToString().Equals("TaxID")
                    && !column.ToString().Equals("UserID")
                          && !column.ToString().Equals("EntryDateTime"))

                //&& !column.ToString().Equals("CompanyId"))
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

        private static List<Object> GetTaxRowsFromList(DataTable dataTable)
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

        public async Task<GenericSaveResponse<Tax>> SaveTaxAsync(Tax tax)
        {
            try
            {
                if (tax.TaxId == Guid.Empty)
                {
                    tax.TaxId = Guid.NewGuid();
                }


                var getallTaxNames = (await _repository.GetAll()).Where(d => d.TaxName == tax.TaxName).ToList();

                if (getallTaxNames.Count != 0)
                {
                    return new GenericSaveResponse<Tax>($"Tax Name already exists. Please Re Enter");
                }

                var getallempNO = (await _repository.GetAll()).Where(d => d.TaxCode == tax.TaxCode).ToList();

                if (getallempNO.Count != 0)
                {
                    return new GenericSaveResponse<Tax>($"Tax Code already exists. Please Re Enter");
                }
                tax.EntryDateTime = DateTime.Now;
                await _repository.InsertAsync(tax);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<Tax>(true, "Successfully Saved.", tax);
            }

            catch (Exception ex)
            {
                return new GenericSaveResponse<Tax>($"An error occured when saving the Tax :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<Tax>> UpdateTaxAsync(string id, Tax tax)
        {
            try
            {
                Tax existingTax = await _repository.GetByIdAsync(tax.TaxId);

                if (existingTax == null)
                    return new GenericSaveResponse<Tax>($"Tax not found");

                var getallTaxNames = (await _repository.GetAll()).Where(d => d.TaxName == tax.TaxName && d.TaxId != existingTax.TaxId).ToList();

                if (getallTaxNames.Count != 0)
                {
                    return new GenericSaveResponse<Tax>($"Tax Name already exists. Please Re Enter");
                }

                var getallempNO = (await _repository.GetAll()).Where(d => d.TaxCode == tax.TaxCode && d.TaxId != existingTax.TaxId).ToList();

                if (getallempNO.Count != 0)
                {
                    return new GenericSaveResponse<Tax>($"Tax Code already exists. Please Re Enter");
                }
                tax.EntryDateTime = DateTime.Now;
                ResourceComparer<Tax> Comparer = new ResourceComparer<Tax>(tax, existingTax);
                ResourceComparerResult<Tax> CompareResult = Comparer.GetUpdatedObject();

                if (CompareResult.Updated)
                {
                    _repository.Update(CompareResult.Obj);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<Tax>(tax);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Tax>($"An error occured when updating the Model :" + (ex.Message ?? ex.InnerException.Message));
            }


        }

        public async Task<GenericSaveResponse<Tax>> DeleteTaxAsync(string id, Tax tax)
        {
            try
            {
                Tax existingTax = await _repository.GetByIdAsync(tax.TaxId);

                if (existingTax == null)
                    return new GenericSaveResponse<Tax>($"Payment Method not found");

                else
                _repository.Delete(tax.TaxId);
                await _unitOfWork.CompleteAsync();
                return new GenericSaveResponse<Tax>(tax);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Tax>($"An error occured when deleting the tax  :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

    }
}
