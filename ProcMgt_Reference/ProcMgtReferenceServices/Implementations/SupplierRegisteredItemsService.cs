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
    public class SupplierRegisteredItemsService : ISupplierRegisteredItemsServices
    {
        private IGenericRepo<SupplierRegisteredItems> _repository = null;
        private IUnitOfWorks _unitOfWork;
        private IGenericRepo<ItemType> _itemtyperepository = null;
        private IGenericRepo<Item> _itemepository = null;

        public SupplierRegisteredItemsService(IGenericRepo<SupplierRegisteredItems> repository, 
            IGenericRepo<ItemType> itemtyperepository, IGenericRepo<Item> itemepository ,IUnitOfWorks unitfwork)
        {
            this._repository = repository;
            this._unitOfWork = unitfwork;
            this._itemtyperepository = itemtyperepository;
            this._itemepository = itemepository;
        }

        public async Task<IEnumerable<SupplierRegisteredItems>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<IEnumerable<ItemResource>> GetItemsDescription(string id, SupplierRegisteredItems supplierregistereditem)
        {
            try
            {

                //var itemTypeList = (from i in await _itemepository.GetAll()
                //                    join s in await _repository.GetAll()
                //                    on i.ItemTypeId equals s.ItemTypeId
                //                    where s.SupplierId == supplierregistereditem.SupplierId && i.IsActive == true
                //                    select new Item
                //                    {
                //                        ItemId = i.ItemId,
                //                        ItemTypeId = i.ItemTypeId,
                //                        ItemDescription = i.ItemDescription,
                //                        ItemCode = i.ItemCode,
                //                        CurrentQty = i.CurrentQty,
                //                        InitialQty = i.InitialQty,
                //                        IsActive = i.IsActive.HasValue ? i.IsActive.Value : false,
                //                        ReOrderQuantity = i.ReOrderQuantity,



                //                    }).ToList();

                var supplier = (await _repository.GetAll()).Where(s => s.SupplierId == supplierregistereditem.SupplierId).ToList();
                var itemTypeList = (await _itemepository.GetAll()).Join(
                    supplier,
                    itemsl => itemsl.ItemTypeId,
                    supLt => supLt.ItemTypeId, (itemsl, supLt) => new ItemResource {

                        ItemID = itemsl.ItemId,
                        ItemTypeID = itemsl.ItemTypeId,
                        ItemDescription = itemsl.ItemDescription,
                        ItemCode = itemsl.ItemCode,
                        CurrentQty = itemsl.CurrentQty,
                        InitialQty = itemsl.InitialQty,
                        IsActive = itemsl.IsActive.HasValue ? itemsl.IsActive.Value : false,
                        ReOrderQuantity = itemsl.ReOrderQuantity,
                        

                        

    }).ToList();
                   
                
                return itemTypeList;

            }

            catch (Exception ex)
            {
                return null;
            }


        }

        public async Task<DataGridTable> getSupRegItemsList(string id, SupplierRegisteredItems supplierRegisteredItems)
        {
            try
            {
                var supplierRegistereditemsList = (await _repository.GetAll()).Select(a => new SupplierRegisteredItemsResource()
                {
                    SupplierRegisteredItemsID = a.SupplierRegisteredItemsId, 
                    ItemTypeID = a.ItemTypeId,
                    MinimumItemCapacity = a.MinimumItemCapacity,
                    SupplierLeadTime = a.SupplierLeadTime,
                    SupplierID = a.SupplierId,
                    UserID = a.UserId,
                    EntryDateTime = a.EntryDateTime.HasValue ? a.EntryDateTime : Convert.ToDateTime("2000-01-01"),
                    itemTypeName = _itemtyperepository.GetByIdAsync(a.ItemTypeId).Result.ItemTypeName.ToString(),

                }).Where(d => d.SupplierID == supplierRegisteredItems.SupplierId).ToList();


                DataTable dtSupplierRegisteredItems = CommonGenericService<Category>.ToDataTable(supplierRegistereditemsList);

                var dataTable = new DataGridTable
                {
                    rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                    enableSorting = true,
                    enableColResize = false,
                    suppressSizeToFit = true,
                    DataGridColumns = GetSupplierRegisteredItemsColumnsfromList(dtSupplierRegisteredItems),
                    DataGridRows = GetSupplierRegisteredItemsRowsFromList(dtSupplierRegisteredItems)
                };

                return dataTable;


            }
            catch (Exception ex)
            {
                return null;
            }

            //var supplierRegistereditemsList = await _repository.GetAll();

        }

       

        //public async Task<DataGridTable> GetSupplierRegisteredItemsGrid()
        //{
        //    try
        //    {
        //        var supplierRegistereditemsList = (await _repository.GetAll()).Select(a => new SupplierRegisteredItemsResource()
        //        {
        //            SupplierRegisteredItemsID = a.SupplierRegisteredItemsId,
        //            ItemTypeID = a.ItemTypeId,
        //            MinimumItemCapacity = a.MinimumItemCapacity,
        //            SupplierLeadTime = a.SupplierLeadTime,
        //            SupplierID = a.SupplierId,

        //            itemTypeName = _itemtyperepository.GetByIdAsync(a.ItemTypeId).Result.ItemTypeName.ToString(),

        //        });


        //        DataTable dtSupplierRegisteredItems = CommonGenericService<Category>.ToDataTable(supplierRegistereditemsList);

        //        var dataTable = new DataGridTable
        //        {
        //            rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
        //            enableSorting = true,
        //            enableColResize = false,
        //            suppressSizeToFit = true,
        //            DataGridColumns = GetSupplierRegisteredItemsColumnsfromList(dtSupplierRegisteredItems),
        //            DataGridRows = GetSupplierRegisteredItemsRowsFromList(dtSupplierRegisteredItems)
        //        };

        //        return dataTable;


        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //    //var supplierRegistereditemsList = await _repository.GetAll();

        //}
        private List<DataGridColumn> GetSupplierRegisteredItemsColumnsfromList(DataTable dataTable)
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

                if (!column.ToString().Equals("SupplierID")
                    && !column.ToString().Equals("UserID")
                          && !column.ToString().Equals("EntryDateTime")
                    && !column.ToString().Equals("SupplierRegisteredItemsID")
                    && !column.ToString().Equals("ItemTypeID"))
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
        private static List<Object> GetSupplierRegisteredItemsRowsFromList(DataTable dataTable)
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

        public async Task<GenericSaveResponse<SupplierRegisteredItems>> SaveSupplierRegisteredItemsAsync(SupplierRegisteredItems supplierRegisteredItems)
        {
            try
            {
                if (supplierRegisteredItems.SupplierRegisteredItemsId == Guid.Empty)
                {
                    supplierRegisteredItems.SupplierRegisteredItemsId = Guid.NewGuid();
                }

                await _repository.InsertAsync(supplierRegisteredItems);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<SupplierRegisteredItems>(true, "Successfully Saved.", supplierRegisteredItems);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<SupplierRegisteredItems>($"An error occured when saving the Supplier Registered Items :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<SupplierRegisteredItems>> UpdateSupplierRegisteredItemsAsync(string id, SupplierRegisteredItems supplierRegisteredItems)
        {
            try
            {
                SupplierRegisteredItems existingSupplierRegisteredItems = await _repository.GetByIdAsync(supplierRegisteredItems.SupplierRegisteredItemsId);

                if (existingSupplierRegisteredItems == null)
                    return new GenericSaveResponse<SupplierRegisteredItems>($"Company not found");

                ResourceComparer<SupplierRegisteredItems> Comparer = new ResourceComparer<SupplierRegisteredItems>(supplierRegisteredItems, existingSupplierRegisteredItems);
                ResourceComparerResult<SupplierRegisteredItems> CompareResult = Comparer.GetUpdatedObject();

                if (CompareResult.Updated)
                {
                    _repository.Update(CompareResult.Obj);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<SupplierRegisteredItems>(supplierRegisteredItems);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<SupplierRegisteredItems>($"An error occured when updating the Supplier Registered Items :" + (ex.Message ?? ex.InnerException.Message));
            }


        }

    }

}
