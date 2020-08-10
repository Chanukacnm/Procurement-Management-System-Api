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
    public class ItemTypeMasterService : IItemTypeMasterServices
    {

        private IGenericRepo<ItemType> _repository = null;
        private IGenericRepo<Category> _categoryrepository = null;
        private IGenericRepo<MeasurementUnits> _measurementunitsrepository = null;
        private IGenericRepo<ApprovalPatternType> _approvalpatterntyperepository = null;
        private IUnitOfWorks _unitOfWork;
        private IGenericRepo<ItemRequest> _itemrequestrepository = null; //--- Add By Nipuna Francisku
        private IGenericRepo<Model> _modelrepository = null; //--- Add By Nipuna Francisku
        private IGenericRepo<Make> _makerepository = null; //--- Add By Nipuna Francisku

        public ItemTypeMasterService(IGenericRepo<ItemType> repository,
            IGenericRepo<Category> categoryrepository,
            IGenericRepo<MeasurementUnits> measurementunitsrepository, 
            IGenericRepo<ApprovalPatternType> approvalpatterntyperepository, 
            IUnitOfWorks unitfwork, IGenericRepo<ItemRequest> itemrequestrepository, IGenericRepo<Model> modelrepository, IGenericRepo<Make> makerepository)
        {
            this._repository = repository;
            this._approvalpatterntyperepository = approvalpatterntyperepository;
            this._categoryrepository = categoryrepository;
            this._measurementunitsrepository = measurementunitsrepository;
            this._unitOfWork = unitfwork;
            this._itemrequestrepository = itemrequestrepository; //--- Add By Nipuna Francisku
            this._modelrepository = modelrepository; //--- Add By Nipuna Francisku
            this._makerepository = makerepository; //--- Add By Nipuna Francisku
        }

        public async Task<IEnumerable<ItemType>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<IEnumerable<ItemType>> GetSpecItemTypeAllAsync(string id, ItemType itemtypemaster)
        {
            var getitemtypespecList = (await _repository.GetAll()).Select(c => new ItemType
            {

                ItemTypeId = c.ItemTypeId,
                ItemTypeName = c.ItemTypeName,
                CategoryId = c.CategoryId,
                ItemTypeCode = c.ItemTypeCode,
                MeasurementUnitId = c.MeasurementUnitId,
                ApprovalPatternTypeId = c.ApprovalPatternTypeId,
                IsDisposable = c.IsDisposable.HasValue ? c.IsActive.Value : false,
                IsActive = c.IsActive.HasValue ? c.IsActive : false
                

            }).Where(d => d.IsActive == true && itemtypemaster.CategoryId == d.CategoryId).OrderBy(e => e.ItemTypeName).ToList();
            return getitemtypespecList;
            
        }

        public async Task<DataGridTable> GetItemTypeMasterGridAsync()
        {
            //var itemTypeList = await _repository.GetAll();

            var itemTypeList = (await _repository.GetAll()).Select(a => new ItemTypeMasterResource()
            {
                 ItemTypeID = a.ItemTypeId,
                 ItemTypeName =a.ItemTypeName,
                 CategoryID = a.CategoryId,
                 ItemTypeCode =a.ItemTypeCode,
                 DepreciationRate = a.DepreciationRate,
                 MeasurementUnitID =a.MeasurementUnitId,
                 IsDisposable =a.IsDisposable.HasValue ? a.IsDisposable.Value : true,
                 Disposable = a.IsDisposable == true ? "Yes" : "No",
                 IsActive=a.IsActive.HasValue ? a.IsActive.Value : true,
                 Status = a.IsActive == true ? "Active" : "Inactive",
                 ApprovalPatternTypeID =a.ApprovalPatternTypeId,
                 UserID = a.UserId,
                 EntryDateTime = a.EntryDateTime.HasValue ? a.EntryDateTime : Convert.ToDateTime("2000-01-01"),

                CategoryName = _categoryrepository.GetByIdAsync(a.CategoryId).Result.CategoryName.ToString(),
                MeasurementUnitName = _measurementunitsrepository.GetByIdAsync(a.MeasurementUnitId).Result.MeasurementUnitName.ToString(),
                PatternName =_approvalpatterntyperepository.GetByIdAsync(a.ApprovalPatternTypeId).Result.PatternName.ToString()
          
            }).OrderBy(b => b.ItemTypeName).ToList();

            //--------- Add By Nipuna Francisku --------------------------------
            foreach (var q in itemTypeList)
            {
                var ItemReqList = (await _itemrequestrepository.GetAll()).Select(b => new ItemRequest() //-- Check ItemRequest
                {
                    ItemTypeId = b.ItemTypeId,
                    ItemRequestId = b.ItemRequestId

                }).Where(d => d.ItemTypeId == q.ItemTypeID).ToList();

                var ModelList = (await _modelrepository.GetAll()).Select(b => new Model() //-- Check Model
                {
                    ItemTypeId = b.ItemTypeId,
                    ModelId = b.ModelId

                }).Where(d => d.ItemTypeId == q.ItemTypeID).ToList();

                var MakeList = (await _makerepository.GetAll()).Select(b => new Make() //-- Check Make
                {
                    ItemTypeId = b.ItemTypeId,
                    MakeId = b.MakeId

                }).Where(d => d.ItemTypeId == q.ItemTypeID).ToList();

                if (ModelList.Count != 0 || MakeList.Count != 0 || ItemReqList.Count != 0)
                {
                    q.IsTansactions = true;
                }
                else
                {
                    q.IsTansactions = false;
                }
            }
            //-------------------------------------------------------------------

            DataTable dtItemType = CommonGenericService<ItemType>.ToDataTable(itemTypeList);

            var dataTable = new DataGridTable
            {
                rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                enableSorting = true,
                enableColResize = false,
                DataGridColumns = GetItemTypeColumnsfromList(dtItemType),
                DataGridRows = GetItemTypeRowsFromList(dtItemType)
            };

            return dataTable;
        }

        private List<DataGridColumn> GetItemTypeColumnsfromList(DataTable dataTable)
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


                if (column.ToString().Equals("ItemTypeName"))
                {
                    dataTableColumn.headerName = "Item Type Name";
                    dataTableColumn.width = 145;
                }
                if (column.ToString().Equals("CategoryName"))
                {
                    dataTableColumn.headerName = "Category Name";
                    dataTableColumn.width = 135;
                    
                }

                if (column.ToString().Equals("ItemTypeCode"))
                {
                    dataTableColumn.headerName = "Item Type Code";
                    dataTableColumn.width = 145;
                }
                  
                if (column.ToString().Equals("DepreciationRate"))
                {
                    dataTableColumn.headerName = "Depreciation Rate";
                    dataTableColumn.width = 145;
                }

                if (column.ToString().Equals("MeasurementUnitName"))
                {
                    dataTableColumn.headerName = "Measurement Unit Name";
                    dataTableColumn.width = 145;
                }

                 if (column.ToString().Equals("PatternName"))
                {
                    dataTableColumn.headerName = "Pattern Name";
                    dataTableColumn.width = 145;
                }

                 if (column.ToString().Equals("Status"))
                {
                    dataTableColumn.headerName = "Status";
                    dataTableColumn.width = 145;
                }
                 
                  if (column.ToString().Equals("Disposable"))
                {
                    dataTableColumn.headerName = "Disposable";
                    dataTableColumn.width = 145;
                }
                     

                if (!column.ToString().Equals("ItemTypeID")
                  && !column.ToString().Equals("ApprovalPatternTypeID")
                   && !column.ToString().Equals("CategoryID")
                    && !column.ToString().Equals("MeasurementUnitID")
                          //&& !column.ToString().Equals("ApprovalPatternType")
                          // && !column.ToString().Equals("ItemCategory")
                          //  && !column.ToString().Equals("MeasurementUnit")
                          //   && !column.ToString().Equals("ReOrderLevel")
                          && !column.ToString().Equals("IsActive")
                          && !column.ToString().Equals("UserID")
                          && !column.ToString().Equals("EntryDateTime")
                           && !column.ToString().Equals("IsDisposable")
                             && !column.ToString().Equals("SupplierRegisteredItems")
                             && !column.ToString().Equals("ItemRequest")
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

        private static List<Object> GetItemTypeRowsFromList(DataTable dataTable)
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


        public async Task<GenericSaveResponse<ItemType>> SaveItemTypeMasterAsync(ItemType itemtypemaster)
        {
            try
            {
                if (itemtypemaster.ItemTypeId == Guid.Empty)
                {
                    itemtypemaster.ItemTypeId = Guid.NewGuid();
                }

                var getallItemTypeName = (await _repository.GetAll()).Where(d => d.CategoryId == itemtypemaster.CategoryId && d.ItemTypeName == itemtypemaster.ItemTypeName).ToList();


                if (getallItemTypeName.Count != 0)
                {

                    return new GenericSaveResponse<ItemType>($"ItemType Name already exists. Please Reenter");
                }

                var getallItemTypecode = (await _repository.GetAll()).Where(d => d.CategoryId == itemtypemaster.CategoryId && d.ItemTypeCode == itemtypemaster.ItemTypeCode).ToList();

                itemtypemaster.EntryDateTime = DateTime.Now;

                if (getallItemTypecode.Count != 0)
                {

                    return new GenericSaveResponse<ItemType>($"ItemType Code already exists. Please Reenter");
                }


                await _repository.InsertAsync(itemtypemaster); 
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<ItemType>(itemtypemaster);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<ItemType>($"An error occured when saving the Item Type :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<ItemType>> UpdateItemTypeMasterAsync(string id, ItemType itemtypemaster)
        {
            try
            {
                ItemType existingItemtype = await _repository.GetByIdAsync(itemtypemaster.ItemTypeId);

                if (existingItemtype == null)
                    return new GenericSaveResponse<ItemType>($"Item Type not found");

                var getallItemTypeName = (await _repository.GetAll()).Where(d => d.CategoryId == itemtypemaster.CategoryId && d.ItemTypeName == itemtypemaster.ItemTypeName && d.ItemTypeId != existingItemtype.ItemTypeId).ToList();


                if (getallItemTypeName.Count != 0)
                {

                    return new GenericSaveResponse<ItemType>($"ItemType Name already exists. Please Reenter");
                }

                var getallItemTypecode = (await _repository.GetAll()).Where(d => d.CategoryId == itemtypemaster.CategoryId && d.ItemTypeCode == itemtypemaster.ItemTypeCode && d.ItemTypeId != existingItemtype.ItemTypeId).ToList();


                if (getallItemTypecode.Count != 0)
                {

                    return new GenericSaveResponse<ItemType>($"ItemType Code already exists. Please Reenter");
                }
                itemtypemaster.EntryDateTime = DateTime.Now;

                ResourceComparer<ItemType> Comparer = new ResourceComparer<ItemType>(itemtypemaster, existingItemtype);
                ResourceComparerResult<ItemType> CompareResult = Comparer.GetUpdatedObject();

                if (CompareResult.Updated)
                {
                    _repository.Update(CompareResult.Obj);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<ItemType>(itemtypemaster);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<ItemType>($"An error occured when updating the Item Type:" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<ItemType>> DeleteItemTypeMasterAsync(ItemType itemtypemaster, string id)
        {
            try
            {
                ItemType existingItemType = await _repository.GetByIdAsync(itemtypemaster.ItemTypeId);

                if (existingItemType == null)
                    return new GenericSaveResponse<ItemType>($"Item Type not found");

                else

                    _repository.Delete(itemtypemaster.ItemTypeId);
                await _unitOfWork.CompleteAsync();
                return new GenericSaveResponse<ItemType>(itemtypemaster);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<ItemType>($"An error occured when deleting the Item Type: " + (ex.Message ?? ex.InnerException.Message));
            }
        }
    }
}
