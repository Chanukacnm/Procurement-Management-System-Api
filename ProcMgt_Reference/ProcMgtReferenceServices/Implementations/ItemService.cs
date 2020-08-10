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
    public class ItemService : IItemServices
    {
        private IGenericRepo<Item> _repository = null;
        private IGenericRepo<ItemType> _itemtyperepository = null;
        private IGenericRepo<Stock> _stockrepository = null;
        private IUnitOfWorks _unitOfWork;
        private IGenericRepo<Podetail> _podetailrepository = null; //--- Add By Nipuna Francisku
        private IGenericRepo<Arndetail> _arndetailrepository = null; //--- Add By Nipuna Francisku
        private IGenericRepo<ItemRequest> _itemrequestrepository = null; //--- Add By Nipuna Francisku
        private IGenericRepo<QuotationRequestDetails> _quotationrequestdetailsrepository = null; //--- Add By Nipuna Francisku

        public ItemService(IGenericRepo<Item> repository, IGenericRepo<ItemType> itemtyperepository, IGenericRepo<Stock> stockrepository, IUnitOfWorks unitfwork, IGenericRepo<Podetail> podetailrepository, IGenericRepo<Arndetail> arndetailrepository, IGenericRepo<ItemRequest> itemrequestrepository, IGenericRepo<QuotationRequestDetails> quotationrequestdetailsrepository)
        {
            this._repository = repository;
            this._itemtyperepository = itemtyperepository;
            this._stockrepository = stockrepository;
            this._unitOfWork = unitfwork;
            this._podetailrepository = podetailrepository; //--- Add By Nipuna Francisku
            this._arndetailrepository = arndetailrepository; //--- Add By Nipuna Francisku
            this._itemrequestrepository = itemrequestrepository; //--- Add By Nipuna Francisku
            this._quotationrequestdetailsrepository = quotationrequestdetailsrepository; //--- Add By Nipuna Francisku
        }

        public async Task<IEnumerable<Item>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<IEnumerable<Stock>> GetStockAllAsync()
        {
            return await _stockrepository.GetAll();
        }

        //public async Task<IEnumerable<Stock>> GetAllStockAsync()
        //{
        //    return await _stockrepository.GetAll();
        //}

        public async Task<IEnumerable<Item>> GetSpecItemAllAsync(string id, Item item)
        {
            var getItemspecList = (await _repository.GetAll()).Select(c => new Item
            {
                ItemId = c.ItemId,
                ItemTypeId = c.ItemTypeId,
                ItemDescription = c.ItemDescription,
                ItemCode = c.ItemCode,
                CurrentQty = c.CurrentQty,
                InitialQty = c.InitialQty,
                IsActive = c.IsActive.HasValue ? c.IsActive.Value : false,
                ReOrderQuantity = c.ReOrderQuantity

        
            }).Where(d => d.IsActive == true && item.ItemTypeId == d.ItemTypeId).OrderBy(e => e.ItemDescription).ToList();
            return getItemspecList;
            //return await _repository.GetAll();
        }

        public async Task<DataGridTable> GetItemGridAsync()
        {
            //var makeList = await _repository.GetAll();

            try
            {
                var itemList = (await _repository.GetAll()).Select(a => new ItemResource()
                {
                    ItemID = a.ItemId,
                    ItemDescription = a.ItemDescription,
                    ItemCode = a.ItemCode,
                    UserID = a.UserId,
                    EntryDateTime = a.EntryDateTime.HasValue ? a.EntryDateTime : Convert.ToDateTime("2000-01-01"),

                    InitialQty = a.InitialQty,
                    IsActive = a.IsActive.HasValue ? a.IsActive.Value : false,
                    Status = a.IsActive == true ? "Active" : "Inactive",
                    ItemTypeID = a.ItemTypeId,
                    ReOrderQuantity = a.ReOrderQuantity,


                    CurrentQty = a.CurrentQty,
                    //StockQty = _stockrepository.GetByIdAsync(a.ItemId).Result.StockQty,  //a.ItemId == null ? "" : _stockrepository.GetByIdAsync(a.ItemId).Result.StockQty.ToString(),
                    ItemTypeName = _itemtyperepository.GetByIdAsync(a.ItemTypeId).Result.ItemTypeName.ToString(),
                    //StockID = _stockrepository.GetByIdAsync(a.ItemId).Result.StockId


                }).OrderBy(x => x.ItemTypeName).ToList();

                //--------- Add By Nipuna Francisku --------------------------------
                foreach (var q in itemList)
                {
                    var PodetailList = (await _podetailrepository.GetAll()).Select(b => new Podetail() //-- Check Podetail
                    {
                        ItemId = b.ItemId,
                        PoheaderId = b.PoheaderId,
                        PodetailId = b.PodetailId

                    }).Where(d => d.ItemId == q.ItemID).ToList();

                    var ArndetailList = (await _arndetailrepository.GetAll()).Select(b => new Arndetail() //-- Check Arndetail
                    {
                        ItemId = b.ItemId,
                        ArndetailId = b.ArndetailId,
                        ArnheaderId = b.ArnheaderId

                    }).Where(d => d.ItemId == q.ItemID).ToList();

                    var ItemReqList = (await _itemrequestrepository.GetAll()).Select(b => new ItemRequest() //-- Check ItemRequest
                    {
                        ItemId = b.ItemId,
                        ItemRequestId = b.ItemRequestId

                    }).Where(d => d.ItemId == q.ItemID).ToList();

                    var QuotationReqList = (await _quotationrequestdetailsrepository.GetAll()).Select(b => new QuotationRequestDetails() //-- Check QuotationRequestDetails
                    {
                        ItemId = b.ItemId,
                        QuotationRequestDetailId = b.QuotationRequestDetailId,
                        QuotationRequestHeaderId = b.QuotationRequestHeaderId

                    }).Where(d => d.ItemId == q.ItemID).ToList();

                    if (PodetailList.Count != 0 || ArndetailList.Count != 0 || ItemReqList.Count != 0 || QuotationReqList.Count != 0)
                    {
                        q.IsTansactions = true;
                    }
                    else
                    {
                        q.IsTansactions = false;
                    }
                }
                //-------------------------------------------------------------------

                DataTable dtitem = CommonGenericService<Make>.ToDataTable(itemList);

                var dataTable = new DataGridTable
                {
                    rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                    enableSorting = true,
                    enableColResize = false,
                    DataGridColumns = GetItemColumnsfromList(dtitem),
                    DataGridRows = GetItemRowsFromList(dtitem)
                };

                return dataTable;
            }
            catch(Exception ex)
            {
                return null;
            }
            


            
        }

        private List<DataGridColumn> GetItemColumnsfromList(DataTable dataTable)
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
                    dataTableColumn.width = 125;
                    dataTableColumn.headerName = "Item Type Name";
                }
                if (column.ToString().Equals("ItemDescription"))
                {
                    dataTableColumn.width = 130;
                    dataTableColumn.headerName = "Item Name";
                }
                if (column.ToString().Equals("ItemCode"))
                {
                    dataTableColumn.width = 110;
                    dataTableColumn.headerName = "Item Code";
                }
                if (column.ToString().Equals("InitialQty"))
                {
                    dataTableColumn.width = 100;
                    dataTableColumn.headerName = "Initial Qty";
                }
                if (column.ToString().Equals("ReOrderQuantity"))
                {
                    dataTableColumn.width = 115;
                    dataTableColumn.headerName = "ReOrder Qty";
                }
                if (column.ToString().Equals("Status"))
                {
                    dataTableColumn.width = 100;
                }

                if (!column.ToString().Equals("ItemID")
                    && !column.ToString().Equals("ItemTypeID")
                     && !column.ToString().Equals("Model")
                     && !column.ToString().Equals("StockQty")
                     && !column.ToString().Equals("IsActive")
                     && !column.ToString().Equals("UserID")
                     && !column.ToString().Equals("EntryDateTime")
                     && !column.ToString().Equals("CurrentQty")
                     && !column.ToString().Equals("IsTansactions") //--------- Add By Nipuna Francisku 
                      && !column.ToString().Equals("ItemRequest"))

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

        private static List<Object> GetItemRowsFromList(DataTable dataTable)
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


        //public async Task<GenericSaveResponse<Item>> SaveItemAsync(Item item)
        //{
        //    try
        //    {
        //        if (item.ItemId== Guid.Empty)
        //        {
        //            item.ItemId = Guid.NewGuid();
        //        }


        //        var getallItem = (await _repository.GetAll()).Where(a => a.ItemDescription == item.ItemDescription && a.ItemTypeId == item.ItemTypeId).ToList();

        //        if (getallItem.Count != 0)
        //        {

        //            return new GenericSaveResponse<Item>($"The Item Description already exists. Please use a different Item Description.");
        //        }

        //        var getallItemCode = (await _repository.GetAll())
        //            .Where(a => a.ItemDescription == item.ItemDescription && a.ItemTypeId == item.ItemTypeId  && a.ItemCode==item.ItemCode).ToList();

        //        if (getallItem.Count != 0)
        //        {
        //            return new GenericSaveResponse<Item>($"The Item Code already exists. Please use a different Item Code.");
        //        }

        //        await _repository.InsertAsync(item);

        //        await _unitOfWork.CompleteAsync();

        //        return new GenericSaveResponse<Item>(true, "Successfully Saved.", item);

        //    }
        //    catch (Exception ex)
        //    {
        //        return new GenericSaveResponse<Item>($"An error occured when saving the Item :" + (ex.Message ?? ex.InnerException.Message));
        //    }
        //}



        public async Task<GenericSaveResponse<Item>> SaveItemAsync(Item item , Stock stock)
        {
            try
            {
                if (item.ItemId == Guid.Empty)
                {
                    item.ItemId = Guid.NewGuid();
                }

                var getallItem = (await _repository.GetAll()).Where(a => a.ItemDescription == item.ItemDescription && a.ItemTypeId == item.ItemTypeId).ToList();

                if (getallItem.Count != 0)
                {

                    return new GenericSaveResponse<Item>($"The Item Description already exists. Please use a different Item Description.");
                }

                var getallItemCode = (await _repository.GetAll())
                    .Where(a =>  a.ItemTypeId == item.ItemTypeId  && a.ItemCode == item.ItemCode).ToList();

                if (getallItemCode.Count != 0)
                {
                    return new GenericSaveResponse<Item>($"The Item Code already exists. Please use a different Item Code.");
                }

                item.EntryDateTime = DateTime.Now;

                if (stock.StockId == Guid.Empty)
                {
                    stock.StockId = Guid.NewGuid();
                }
                if (stock.ItemId == Guid.Empty)
                {
                    stock.ItemId = item.ItemId;
                }

                await _repository.InsertAsync(item);
                await _stockrepository.InsertAsync(stock);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<Item>(true, "Successfully Saved.", item);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Item>($"An error occured when saving the Item :" + (ex.Message ?? ex.InnerException.Message));
            }
        }


        public async Task<GenericSaveResponse<Item>> UpdateItemAsync(string id, Item item , Stock stock)
        {
            try
            {

                Item existingItem = await _repository.GetByIdAsync(item.ItemId);

                if (existingItem == null)
                    return new GenericSaveResponse<Item>($"Item not found");

                var getallItem = (await _repository.GetAll()).Where(a => a.ItemDescription == item.ItemDescription && a.ItemTypeId == item.ItemTypeId && a.ItemId != existingItem.ItemId).ToList();

                if (getallItem.Count != 0)
                {

                    return new GenericSaveResponse<Item>($"The Item Description already exists. Please use a different Item Description.");
                }

                var getallItemCode = (await _repository.GetAll())
                    .Where(a =>  a.ItemTypeId == item.ItemTypeId && a.ItemCode == item.ItemCode && a.ItemId != existingItem.ItemId).ToList();

                if (getallItemCode.Count != 0)
                {
                    return new GenericSaveResponse<Item>($"The Item Code already exists. Please use a different Item Code.");
                }
                item.EntryDateTime = DateTime.Now;

                ResourceComparer<Item> Comparer = new ResourceComparer<Item>(item, existingItem);
                ResourceComparerResult<Item> CompareResult = Comparer.GetUpdatedObject();
                                
                if (CompareResult.Updated)
                {
                    _repository.Update(CompareResult.Obj);
                    
                }

                
                await _unitOfWork.CompleteAsync();
                return new GenericSaveResponse<Item>(item);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Item>($"An error occured when updating the Item :" + (ex.Message ?? ex.InnerException.Message));
            }


        }


        //public async Task<GenericSaveResponse<Item>> UpdateItemAsync(string id, Item item)
        //{
        //    try
        //    {

        //        Item existingItem = await _repository.GetByIdAsync(item.ItemId);

        //        if (existingItem == null)
        //            return new GenericSaveResponse<Item>($"Item not found");

        //        var getallItem = (await _repository.GetAll()).Where(a => a.ItemDescription == item.ItemDescription && a.ItemTypeId == item.ItemTypeId && a.ItemId != existingItem.ItemId).ToList();

        //        if (getallItem.Count != 0)
        //        {

        //            return new GenericSaveResponse<Item>($"The Item Description already exists. Please use a different Item Description.");
        //        }

        //        var getallItemCode = (await _repository.GetAll())
        //            .Where(a => a.ItemDescription == item.ItemDescription && a.ItemTypeId == item.ItemTypeId && a.ItemCode == item.ItemCode && a.ItemId != existingItem.ItemId).ToList();

        //        if (getallItem.Count != 0)
        //        {
        //            return new GenericSaveResponse<Item>($"The Item Code already exists. Please use a different Item Code.");
        //        }

        //        ResourceComparer<Item> Comparer = new ResourceComparer<Item>(item, existingItem);
        //        ResourceComparerResult<Item> CompareResult = Comparer.GetUpdatedObject();

        //        if (CompareResult.Updated)
        //        {
        //            _repository.Update(CompareResult.Obj);
        //            await _unitOfWork.CompleteAsync();
        //        }

        //        return new GenericSaveResponse<Item>(item);

        //    }
        //    catch (Exception ex)
        //    {
        //        return new GenericSaveResponse<Item>($"An error occured when updating the Item :" + (ex.Message ?? ex.InnerException.Message));
        //    }


        //}

        public async Task<GenericSaveResponse<Item>> DeleteItemAsync(Item item, string id)
        {
            try
            {
                Item existingItem = await _repository.GetByIdAsync(item.ItemId);

                if (existingItem == null)
                    return new GenericSaveResponse<Item>($"Item not found");

                else

                    _repository.Delete(item.ItemId);
                await _unitOfWork.CompleteAsync();
                return new GenericSaveResponse<Item>(item);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Item>($"An error occured when deleting the Item :" + (ex.Message ?? ex.InnerException.Message));
            }
        }


    }
}
