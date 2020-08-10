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
//using ProcMgt_Reference_Infrastructure.Models;
//using Microsoft.EntityFrameworkCore;

namespace ProcMgt_Reference_Services.Implementations
{
   public class MakeService :IMakeServices
    {
            //private readonly ReferenceContext _context;
        private IGenericRepo<Make> _repository = null;
        private IGenericRepo<ItemType> _itemtyperepository = null;
        private IGenericRepo<Item> _itemrepository = null;
        private IUnitOfWorks _unitOfWork;
        private IGenericRepo<ItemRequest> _itemRequestrepository = null; //--- Add By Nipuna Francisku
        private IGenericRepo<QuotationRequestDetails> _quotationrequestdetailsrepository = null; //--- Add By Nipuna Francisku

        public MakeService(IGenericRepo<Make> repository, IGenericRepo<Item> itemrepository, IGenericRepo<ItemType> itemtyperepository, IUnitOfWorks unitfwork, IGenericRepo<ItemRequest> itemRequestrepository, IGenericRepo<QuotationRequestDetails> quotationrequestdetailsrepository)
        {
            this._repository = repository;
            this._itemtyperepository = itemtyperepository;
            this._itemrepository = itemrepository;
            this._unitOfWork = unitfwork;
            //this._context = context;
            this._itemRequestrepository = itemRequestrepository; //--- Add By Nipuna Francisku
            this._quotationrequestdetailsrepository = quotationrequestdetailsrepository; //--- Add By Nipuna Francisku
        }

        public async Task<IEnumerable<Make>> GetAllAsync()
        {
            var makegetall = (await _repository.GetAll()).Select(c => new Make
            {
               MakeId = c.MakeId,
               MakeName = c.MakeName,
               MakeCode = c.MakeCode,
               IsActive = c.IsActive.HasValue ? c.IsActive.Value : false,

            }).Where(d => d.IsActive == true).OrderBy(e => e.MakeName);

            return makegetall; 
        }

        public async Task<IEnumerable<Make>> GetSpecMakeAllAsync(string id, Make make)
        {
            var getmakespecList = (await _repository.GetAll()).Select(c => new Make
            {
                MakeId = c.MakeId,
                MakeCode = c.MakeCode,
                MakeName = c.MakeName,
                ItemTypeId = c.ItemTypeId,
                IsActive = c.IsActive.HasValue ? c.IsActive.Value : false

            }).Where(d => d.IsActive == true && make.ItemTypeId == d.ItemTypeId).OrderBy(e => e.MakeName).ToList();
            //return await _repository.GetAll();

            return getmakespecList;
        }

        public async Task<IEnumerable<MakeResource>> GetSpecMakeListAsync(string id, Item item) {

            var makeTypeList = (from i in await _repository.GetAll()
                                join s in await _itemrepository.GetAll()
                                on i.ItemTypeId equals s.ItemTypeId
                                where s.ItemId == item.ItemId && i.IsActive == true
                                select new MakeResource
                                {
                                    MakeID = i.MakeId,
                                    MakeCode = i.MakeCode,
                                    MakeName = i.MakeName,
                                    ItemTypeID = i.ItemTypeId,
                                    IsActive = i.IsActive.HasValue ? i.IsActive.Value : false,

                                }).ToList();

            return makeTypeList;
        }

        public async Task<IEnumerable<Make>> GetSpecMakeListAllAsync(string id, Make make)
        {
            var getmake2specList = (await _repository.GetAll()).Select(c => new Make
            {
                MakeId = c.MakeId,
                //MakeCode = c.MakeCode,
                MakeName = c.MakeName,
                ItemTypeId = c.ItemTypeId,
                IsActive = c.IsActive.HasValue ? c.IsActive.Value : false

            }).Where(d => d.IsActive == true && make.ItemTypeId == d.ItemTypeId).OrderBy(e => e.MakeName).ToList();
            return getmake2specList;
            //return await _repository.GetAll();
        }


        public async Task<DataGridTable> GetMakeGridAsync()
        {
            //var makeList = await _repository.GetAll();

            var makeList = (await _repository.GetAll()).Select(a => new MakeResource()
            {
                MakeID = a.MakeId,
                ItemTypeID = a.ItemTypeId,
                MakeCode = a.MakeCode,
                MakeName = a.MakeName,
                IsActive = a.IsActive.HasValue ? a.IsActive.Value : false,
                Status = a.IsActive == true ? "Active" : "Inactive",
                UserID = a.UserId, 
                EntryDateTime = a.EntryDateTime.HasValue ? a.EntryDateTime : Convert.ToDateTime("2000-01-01"),
                 

                ItemTypeName = _itemtyperepository.GetByIdAsync(a.ItemTypeId).Result.ItemTypeName.ToString()

            }).OrderBy(b => b.MakeName).ToList();

            //_context.ItemType
            //              .Where(s => s.ItemTypeId == a.ItemTypeId)
            //              .Select(s => s.ItemTypeName).FirstOrDefault(),

            //_ebetEntities.SlipStags.AsNoTracking().Where(a => a.SlipCode == slipCode).Select(a => a.SlipStagID).FirstOrDefault();

            //--------- Add By Nipuna Francisku --------------------------------
            foreach (var q in makeList)
            {
                var ItemReqList = (await _itemRequestrepository.GetAll()).Select(b => new ItemRequest() //-- Check ItemRequest
                {
                    MakeId = b.MakeId,
                    ItemRequestId = b.ItemRequestId
                }).Where(d => d.MakeId == q.MakeID).ToList();

                var QuotationReqList = (await _quotationrequestdetailsrepository.GetAll()).Select(b => new QuotationRequestDetails() //-- Check QuotationRequestDetails
                {
                    MakeId = b.MakeId,
                    QuotationRequestHeaderId = b.QuotationRequestHeaderId,
                    QuotationRequestDetailId = b.QuotationRequestDetailId

                }).Where(d => d.MakeId == q.MakeID).ToList();

                if (ItemReqList.Count != 0 || QuotationReqList.Count != 0)
                {
                    q.IsTansactions = true;
                }
                else
                {
                    q.IsTansactions = false;
                }
            }
            //-------------------------------------------------------------------

            DataTable dtmake = CommonGenericService<Make>.ToDataTable(makeList);

                var dataTable = new DataGridTable
                {
                    rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                    enableSorting = true,
                    enableColResize = false,
                    DataGridColumns = GetMakeColumnsfromList(dtmake),
                    DataGridRows = GetMakeRowsFromList(dtmake)
                };

                return dataTable;
        }
            private List<DataGridColumn> GetMakeColumnsfromList(DataTable dataTable)
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
                if (column.ToString().Equals("MakeName"))
                {
                    dataTableColumn.headerName = "Make Name";
                    dataTableColumn.width = 135;

                }
                     
                if (column.ToString().Equals("MakeCode"))
                {
                    dataTableColumn.headerName = "Make Code";
                    dataTableColumn.width = 135;

                }
                 
                if (column.ToString().Equals("Status"))
                {
                    dataTableColumn.headerName = "Status";
                    dataTableColumn.width = 135;
                }

                if (!column.ToString().Equals("MakeID")
                    && !column.ToString().Equals("ItemTypeID")
                     && !column.ToString().Equals("Model")
                     && !column.ToString().Equals("IsActive")
                      && !column.ToString().Equals("UserID")
                      && !column.ToString().Equals("EntryDateTime")
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

        private static List<Object> GetMakeRowsFromList(DataTable dataTable)
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


        public async Task<GenericSaveResponse<Make>> SaveMakeAsync(Make make)
        {
            try
            {
                if (make.MakeId == Guid.Empty)
                {
                    make.MakeId = Guid.NewGuid();
                }

                var getallMakeName = (await _repository.GetAll()).Where(d => d.ItemTypeId == make.ItemTypeId && d.MakeName==make.MakeName).ToList();

                if (getallMakeName.Count != 0)
                {

                    return new GenericSaveResponse<Make>($"The Make Name already exists. Please use a different Make Name.");
                }
                
                var getallmakeCode = (await _repository.GetAll())
                    .Where(a => a.ItemTypeId == make.ItemTypeId  && a.MakeCode == make.MakeCode).ToList();
                make.EntryDateTime = DateTime.Now;
                if (getallmakeCode.Count != 0)
                {
                    return new GenericSaveResponse<Make>($"The Make Code already exists. Please use a different Make Code.");
                }

                await _repository.InsertAsync(make);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<Make>(true, "Successfully Saved.", make);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Make>($"An error occured when saving the Make :" + (ex.Message ?? ex.InnerException.Message));
            }

        }

            public async Task<GenericSaveResponse<Make>> UpdateMakeAsync(string id, Make make)
            {
                try
                {
                    Make existingMake = await _repository.GetByIdAsync(make.MakeId);

                    if (existingMake == null)
                        return new GenericSaveResponse<Make>($"Make not found");

                var getallMakeName = (await _repository.GetAll()).
                    Where(d => d.ItemTypeId == make.ItemTypeId && d.MakeName == make.MakeName && d.MakeId != existingMake.MakeId).ToList();

                if (getallMakeName.Count != 0)
                {

                    return new GenericSaveResponse<Make>($"The Make Name already exists. Please use a different Make Name.");
                }

                var getallmakeCode = (await _repository.GetAll())
                    .Where(a => a.ItemTypeId == make.ItemTypeId  && a.MakeCode == make.MakeCode && a.MakeId != existingMake.MakeId).ToList();

                if (getallmakeCode.Count != 0)
                {
                    return new GenericSaveResponse<Make>($"The Make Code already exists. Please use a different Make Code.");
                }
                make.EntryDateTime = DateTime.Now;
                ResourceComparer<Make> Comparer = new ResourceComparer<Make>(make, existingMake);
                    ResourceComparerResult<Make> CompareResult = Comparer.GetUpdatedObject();

                    if (CompareResult.Updated)
                    {
                        _repository.Update(CompareResult.Obj);
                        await _unitOfWork.CompleteAsync();
                    }

                    return new GenericSaveResponse<Make>(make);

                }
                catch (Exception ex)
                {
                    return new GenericSaveResponse<Make>($"An error occured when updating the Make :" + (ex.Message ?? ex.InnerException.Message));
                }


            }

        public async Task<GenericSaveResponse<Make>> DeleteMakeAsync(Make make, string id)
        {
            try
            {
                Make existingMake = await _repository.GetByIdAsync(make.MakeId);

                if (existingMake == null)
                    return new GenericSaveResponse<Make>($"Make not found");

                else

                    _repository.Delete(make.MakeId);
                await _unitOfWork.CompleteAsync();
                return new GenericSaveResponse<Make>(make);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Make>($"An error occured when deleting the Make :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        //public async Task<List<DropDownValues>> GetDropDown(string id, Make make)
        //{

        //    List<DropDownValues> LDP = new List<DropDownValues>();

        //    IEnumerable<Make> makelst = await _repository.GetAll();

        //    foreach (Make item in makelst)
        //    {
        //        DropDownValues itemLDP = new DropDownValues();

        //        itemLDP.Id = item.UserId;
        //        itemLDP.Value = item.Name;
        //        LDP.Add(itemLDP);
        //    }

        //    return LDP;


        //}

    }

    }

