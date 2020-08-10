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
    public class DesignationService : IDesignationServices
    {
        private IGenericRepo<Designation> _repository = null;
        private IUnitOfWorks _unitOfWork;
        private IGenericRepo<BusinessUnitType> _businessunittyperepository = null;

        public DesignationService(IGenericRepo<Designation> repository, IGenericRepo<BusinessUnitType> businessunittyperepository, IUnitOfWorks unitfwork)
        {
            this._repository = repository;
            this._unitOfWork = unitfwork;
            this._businessunittyperepository = businessunittyperepository;
        }

        public async Task<IEnumerable<Designation>> GetAllAsync()
        {
            var designationlist = (await _repository.GetAll()).Select(a => new Designation
            {
                DesignationId = a.DesignationId,
                DesignationName = a.DesignationName,
                IsActive = a.IsActive.HasValue ? a.IsActive.Value : false,
                User = a.User
            }).Where(c => c.IsActive==true).OrderBy(b => b.DesignationName);

            return designationlist;
            //return await _repository.GetAll();
        }

        public async Task<DataGridTable> GetDesignationGridAsync()
        {
            var designationList = (await _repository.GetAll()).Select(a => new DesignationResource
            {
                DesignationID = a.DesignationId,
                DesignationCode = a.DesignationCode,
                DesignationName = a.DesignationName,
                IsActive = a.IsActive.HasValue ? a.IsActive.Value : false,
                Status = a.IsActive == true ? "Active" : "Inactive",
                BusinessUnitTypeName = a.BusinessUnitTypeName,
                EntryDateTime = a.EntryDateTime.HasValue ? a.EntryDateTime : Convert.ToDateTime("2020-01-01"),

            }).OrderBy(b => b.DesignationName);

            DataTable dtDesignation = CommonGenericService<Category>.ToDataTable(designationList);

            var dataTable = new DataGridTable
            {
                rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                enableSorting = true,
                enableColResize = false,
                suppressSizeToFit = true,
                DataGridColumns = GetDesignationColumnsfromList(dtDesignation),
                DataGridRows = GetDesignationRowsFromList(dtDesignation)
            };

            return dataTable;

        }

        private List<DataGridColumn> GetDesignationColumnsfromList(DataTable dataTable)
        {
            var DataGridColumns = new List<DataGridColumn>();

            foreach (DataColumn column in dataTable.Columns)
            {
                var dataTableColumn = new DataGridColumn
                {
                    field = column.ToString().Replace(" ", "_"),
                    headerName = column.ToString(),
                    //width = 130
                    //visible = 

                    //halign = DataAlignEnum.Center.ToString().ToLower()
                };

                if (column.ToString().Equals("DesignationName"))
                {
                    dataTableColumn.headerName = "Designation Name";
                    dataTableColumn.width = 145;
                }
                if (column.ToString().Equals("DesignationCode"))
                {
                    dataTableColumn.headerName = "Designation Code";
                    dataTableColumn.width = 135;
                    //dataTableColumn.editable = true;
                }

                if (column.ToString().Equals("Status"))
                {
                    dataTableColumn.headerName = "Status";
                    dataTableColumn.width = 100;
                }
                if (column.ToString().Equals("BusinessUnitTypeName"))
                {
                    dataTableColumn.headerName = "Designation Level";
                    dataTableColumn.width = 150;
                }

                if (!column.ToString().Equals("DesignationID")
                    && !column.ToString().Equals("BusinessUnitType")
                    && !column.ToString().Equals("IsActive")
                    && !column.ToString().Equals("User")
                    && !column.ToString().Equals("EntryDateTime")
                    && !column.ToString().Equals("DesignationBusinessUnit"))
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

        private static List<Object> GetDesignationRowsFromList(DataTable dataTable)
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

        public async Task<GenericSaveResponse<Designation>> SaveDesignationAsync(Designation designation )
        {
            try
            {
                if (designation.DesignationId == Guid.Empty)
                {
                    designation.DesignationId = Guid.NewGuid();
                }

                var getallDesigantionNames = (await _repository.GetAll()).Where(d => d.DesignationName == designation.DesignationName).ToList();

                if (getallDesigantionNames.Count != 0)
                {
                    return new GenericSaveResponse<Designation>($"The Designation Name already exists. Please use a different Designation Name");
                }

                var getallDesigantionCode = (await _repository.GetAll()).Where(d => d.DesignationCode == designation.DesignationCode).ToList();

                if (getallDesigantionCode.Count != 0)
                {
                    return new GenericSaveResponse<Designation>($"The Designation Code already exists. Please use a different Designation Code");
                }

                designation.EntryDateTime = DateTime.Now;

                BusinessUnitType businessUnitType = new BusinessUnitType();
                businessUnitType.BusinessUnitTypeId = Guid.NewGuid();
                businessUnitType.BusinessUnitTypeName = designation.BusinessUnitTypeName;
                businessUnitType.DesignationId = designation.DesignationId;
                businessUnitType.IsActive = true;
                businessUnitType.EntryDateTime = DateTime.Now;

                await _repository.InsertAsync(designation);
                await _businessunittyperepository.InsertAsync(businessUnitType);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<Designation>(true, "Successfully Saved.", designation);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Designation>($"An error occured when saving the Designation :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<Designation>> UpdateDesignationAsync(string id, Designation designation )
        {
            try
            {
                Designation existingDesignation = await _repository.GetByIdAsync(designation.DesignationId);

                if (existingDesignation == null)
                    return new GenericSaveResponse<Designation>($"Designation not found");

                var getallDesigantionNames = (await _repository.GetAll()).Where(d => d.DesignationName == designation.DesignationName && d.DesignationId!=existingDesignation.DesignationId).ToList();

                if (getallDesigantionNames.Count != 0)
                {
                    return new GenericSaveResponse<Designation>($"The Designation Name already exists. Please use a different Designation Name");
                }

                var getallDesigantionCode = (await _repository.GetAll()).Where(d => d.DesignationCode == designation.DesignationCode && d.DesignationId != existingDesignation.DesignationId).ToList();

                if (getallDesigantionCode.Count != 0)
                {
                    return new GenericSaveResponse<Designation>($"The Designation Code already exists. Please use a different Designation Code");
                }

                designation.EntryDateTime = DateTime.Now;
                //designation.BusinessUnitTypeName = designation.BusinessUnitTypeName;
                //BusinessUnitType getBUnitType = (await _businessunittyperepository.GetAll()).Where(a => a.DesignationId == designation.DesignationId).ToList().FirstOrDefault();

                //getBUnitType.BusinessUnitTypeName = designation.BusinessUnitTypeName;
                //getBUnitType.EntryDateTime = designation.EntryDateTime;

                //_businessunittyperepository.Update(getBUnitType);

                //BusinessUnitType

                //getBUnitType.BusinessUnitTypeName = designation.BusinessUnitTypeName;

                

                //BusinessUnitType businessUnitType2 = new BusinessUnitType();

                //businessUnitType2.BusinessUnitTypeId = Guid.NewGuid();
                //businessUnitType2.BusinessUnitTypeName = designation.BusinessUnitTypeName;
                //businessUnitType2.EntryDateTime = designation.EntryDateTime;
                //businessUnitType2.IsActive = true;
                //businessUnitType2.DesignationId = designation.DesignationId;

                ResourceComparer<Designation> Comparer = new ResourceComparer<Designation>(designation, existingDesignation);
                ResourceComparerResult<Designation> CompareResult = Comparer.GetUpdatedObject();

                if (CompareResult.Updated)
                {

                    
                    _repository.Update(CompareResult.Obj);
                    
                }

                

                //await _businessunittyperepository.InsertAsync(businessUnitType2);

                await _unitOfWork.CompleteAsync();
                return new GenericSaveResponse<Designation>(designation);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Designation>($"An error occured when updating the Designation :" + (ex.Message ?? ex.InnerException.Message));
            }


        }


        public async Task<GenericSaveResponse<Designation>> UpdateBusinessUnitTypeAsync(string id, Designation designation)
        {
            try
            {
                //businessunittype == designation level

                BusinessUnitType getBUnitType = (await _businessunittyperepository.GetAll()).Where(a => a.DesignationId == designation.DesignationId).ToList().FirstOrDefault();

                BusinessUnitType businessUnitType = new BusinessUnitType();
                businessUnitType.DesignationId = designation.DesignationId;

                _businessunittyperepository.Delete(getBUnitType.BusinessUnitTypeId);


                BusinessUnitType businessUnitType2 = new BusinessUnitType();

                businessUnitType2.BusinessUnitTypeId = Guid.NewGuid();
                businessUnitType2.BusinessUnitTypeName = designation.BusinessUnitTypeName;
                businessUnitType2.EntryDateTime = designation.EntryDateTime;
                businessUnitType2.IsActive = true;
                businessUnitType2.DesignationId = designation.DesignationId;


                await _businessunittyperepository.InsertAsync(businessUnitType2);

                await _unitOfWork.CompleteAsync();
                return new GenericSaveResponse<Designation>(designation);
            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Designation>($"An error occured when updating the Business Unit Type :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

    }


}
