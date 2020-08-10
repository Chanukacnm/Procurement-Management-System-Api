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
    public class CategoryMasterService : ICategoryMasterServices
    {
        private IGenericRepo<Category> _repository = null;
        private IUnitOfWorks _unitOfWork;
        private IGenericRepo<ItemRequest> _itemrequestrepository = null; //--- Add By Nipuna Francisku

        public CategoryMasterService(IGenericRepo<Category> repository, IUnitOfWorks unitfwork, IGenericRepo<ItemRequest> itemrequestrepository)
        {
            this._repository = repository;
            this._unitOfWork = unitfwork;
            this._itemrequestrepository = itemrequestrepository; //--- Add By Nipuna Francisku
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var getallCategory = (await _repository.GetAll()).Select(c => new Category
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                CategoryCode = c.CategoryCode,
                IsActive = c.IsActive.HasValue ? c.IsActive.Value : false
            }).Where(d => d.IsActive == true).OrderBy(e => e.CategoryName);

            return getallCategory;
            //return await _repository.GetAll();
        }

        public async Task<DataGridTable> GetCategoryGridAsync()
        {
            //var categoryList = await _repository.GetAll();
            var categoryList = (await _repository.GetAll()).Select(a => new CategoryMasterResource
            {
                CategoryID = a.CategoryId,
                CategoryCode = a.CategoryCode,
                CategoryName = a.CategoryName,
                IsActive = a.IsActive.HasValue ? a.IsActive.Value : false,
                Status = a.IsActive == true ? "Active" : "Inactive",
                UserID = a.UserId, 
                EntryDateTime = a.EntryDateTime.HasValue? a.EntryDateTime: Convert.ToDateTime("2000-01-01")

            }).OrderBy(b => b.CategoryName).ToList();

            //--------- Add By Nipuna Francisku --------------------------------
            foreach (var q in categoryList)
            {
                var ItemReqList = (await _itemrequestrepository.GetAll()).Select(b => new ItemRequest() //-- Check ItemRequest
                {
                    CategoryId = b.CategoryId,
                    ItemRequestId = b.ItemRequestId

                }).Where(d => d.CategoryId == q.CategoryID).ToList();

                if (ItemReqList.Count != 0)
                {
                    q.IsTansactions = true;
                }
                else
                {
                    q.IsTansactions = false;
                }
            }
            //-------------------------------------------------------------------


            DataTable dtCategory = CommonGenericService<Category>.ToDataTable(categoryList);

            var dataTable = new DataGridTable
            {
                rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                enableSorting = true,
                enableColResize = false,
                suppressSizeToFit = true,
                DataGridColumns = GetCategoryColumnsfromList(dtCategory),
                DataGridRows = GetCategoryRowsFromList(dtCategory)
            };

            return dataTable;
        }


        private List<DataGridColumn> GetCategoryColumnsfromList(DataTable dataTable)
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

                if (column.ToString().Equals("CategoryName"))
                {
                    dataTableColumn.headerName = "Category Name";
                    dataTableColumn.width = 145;
                }
                if (column.ToString().Equals("CategoryCode"))
                {
                    dataTableColumn.headerName = "Category Code";
                    dataTableColumn.width = 135;
                    //dataTableColumn.editable = true;
                }

                if (column.ToString().Equals("Status"))
                {
                    dataTableColumn.headerName = "Status";
                    dataTableColumn.width = 100;
                }

                if (!column.ToString().Equals("CategoryID")
                    && !column.ToString().Equals("ItemType")
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
        
        private static List<Object> GetCategoryRowsFromList(DataTable dataTable)
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

        public async Task<GenericSaveResponse<Category>> SaveCategoryMasterAsync(Category categorymaster)
        {
            try
            {
                if (categorymaster.CategoryId == Guid.Empty)
                {
                    categorymaster.CategoryId = Guid.NewGuid();
                }

                var getallCategoryNames = (await _repository.GetAll()).Where(d => d.CategoryName == categorymaster.CategoryName).ToList();

                if (getallCategoryNames.Count != 0)
                {
                    return new GenericSaveResponse<Category>($"The Category Name already exists. Please use a different Category Name");
                }

                var getallCategoryCode = (await _repository.GetAll()).Where(d => d.CategoryCode == categorymaster.CategoryCode).ToList();

                if (getallCategoryCode.Count != 0)
                {
                    return new GenericSaveResponse<Category>($"The Category Code already exists. Please use a different Category Code");
                }

                categorymaster.EntryDateTime = DateTime.Now;

                await _repository.InsertAsync(categorymaster);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<Category>(true, "Successfully Saved.", categorymaster);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Category>($"An error occured when saving the Category Master :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<Category>> UpdateCategoryMasterAsync(string id, Category categorymaster)
        {
            try
            {
                Category existingCategoryMaster = await _repository.GetByIdAsync(categorymaster.CategoryId);

                if (existingCategoryMaster == null)
                {
                    return new GenericSaveResponse<Category>($"Category Master not found");
                }

                var getallCategoryNames = (await _repository.GetAll()).Where(d => d.CategoryName == categorymaster.CategoryName && d.CategoryId != existingCategoryMaster.CategoryId).ToList();

                if (getallCategoryNames.Count != 0)
                {
                    return new GenericSaveResponse<Category>($"The Category Name already exists. Please use a different Category Name");
                }

                var getallCategoryCode = (await _repository.GetAll()).Where(d => d.CategoryCode == categorymaster.CategoryCode && d.CategoryId != existingCategoryMaster.CategoryId).ToList();

                if (getallCategoryCode.Count != 0)
                {
                    return new GenericSaveResponse<Category>($"The Category Code already exists. Please use a different Category Code");
                }
                categorymaster.EntryDateTime = DateTime.Now;
                ResourceComparer<Category> Comparer = new ResourceComparer<Category>(categorymaster, existingCategoryMaster);
                ResourceComparerResult<Category> CompareResult = Comparer.GetUpdatedObject();

                if (CompareResult.Updated)
                {
                    _repository.Update(CompareResult.Obj);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<Category>(categorymaster);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Category>($"An error occured when updating the Category Master :" + (ex.Message ?? ex.InnerException.Message));
            }


        }

        public async Task<GenericSaveResponse<Category>> DeleteCategoryMasterAsync(string id, Category categorymaster)
        {
            try
            {
                Category existingCategoryMaster = await _repository.GetByIdAsync(categorymaster.CategoryId);
                
                if (existingCategoryMaster == null)
                {
                    return new GenericSaveResponse<Category>($"Category Master not found");
                }
                else
                {
                    _repository.Delete(categorymaster.CategoryId);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<Category>(categorymaster);
            }
            catch(Exception ex)
            {
                return new GenericSaveResponse<Category>($"An error occured when deleting the Category Master :" + (ex.Message ?? ex.InnerException.Message));
            }
        }
    }
}
