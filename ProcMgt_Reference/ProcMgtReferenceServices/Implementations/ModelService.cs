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
    public class ModelService : IModelServices
    {
        private IGenericRepo<Model> _repository ;
        private IGenericRepo<ItemType> _itemtyperepository = null;
        private IGenericRepo<Make> _makerepository = null;
        private IGenericRepo<UploadFile> _uploadfileepository = null;
        private IUnitOfWorks _unitOfWork;
        private IGenericRepo<ItemRequest> _itemrequestrepository = null; //--- Add By Nipuna Francisku
        private IGenericRepo<QuotationRequestDetails> _quotationrequestdetailsrepository = null; //--- Add By Nipuna Francisku

        public ModelService(IGenericRepo<Model> repository, IGenericRepo<ItemType> itemtyperepository, IGenericRepo<UploadFile> uploadfilerepository, IGenericRepo<Make> makerepository, IUnitOfWorks unitfwork, IGenericRepo<ItemRequest> itemrequestrepository, IGenericRepo<QuotationRequestDetails> quotationrequestdetailsrepository)
        {
            this._repository = repository;
            this._itemtyperepository = itemtyperepository;
            this._makerepository = makerepository;
            this._uploadfileepository = uploadfilerepository;
            this._unitOfWork = unitfwork;
            this._itemrequestrepository = itemrequestrepository; //--- Add By Nipuna Francisku
            this._quotationrequestdetailsrepository = quotationrequestdetailsrepository; //--- Add By Nipuna Francisku
        }

        public async Task<IEnumerable<Model>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<IEnumerable<Model>> GetSpecModelAllAsync(string id, Model model)
        {
            var getModelspecList = (await _repository.GetAll()).Select(c => new Model
            {
               ModelId = c.ModelId,
               ModelName = c.ModelName,
               //ModelCode = c.ModelCode,
               MakeId = c.MakeId,
               //ItemTypeId = c.ItemTypeId,
               //Image = c.Image,
               IsActive = c.IsActive.HasValue ? c.IsActive.Value : false,
               
            }).Where(d => d.IsActive == true && model.MakeId == d.MakeId).OrderBy(e => e.ModelName).ToList();
            return getModelspecList;
            //return await _repository.GetAll();
        }

        public async Task<DataGridTable> GetModelGridAsync()
        {
            //var modelList = await _repository.GetAll();
            try
            {
                var modelList = (await _repository.GetAll()).Select(a => new ModelResource()
                {
                    ModelID = a.ModelId,
                    ModelCode = a.ModelCode,
                    MakeID = a.MakeId,
                    UploadFileID = a.UploadFileId ==null ? null : a.UploadFileId,
                    ModelName = a.ModelName,
                    ItemTypeID = a.ItemTypeId,
                    Image = a.Image,
                    IsActive = a.IsActive.HasValue ? a.IsActive.Value : false,
                    Status = a.IsActive == true ? "Active" : "Inactive",
                    UserID = a.UserId,
                    EntryDateTime = a.EntryDateTime.HasValue ? a.EntryDateTime : Convert.ToDateTime("2000-01-01"),

                    UploadFileName = a.UploadFileId == null ? "" : _uploadfileepository.GetByIdAsync(a.UploadFileId).Result.UploadFileName.ToString(),
                    ItemTypeName = _itemtyperepository.GetByIdAsync(a.ItemTypeId).Result.ItemTypeName.ToString(),
                    MakeName = _makerepository.GetByIdAsync(a.MakeId).Result.MakeName.ToString(),

                }).OrderBy(b => b.ModelName).ToList();

                //--------- Add By Nipuna Francisku --------------------------------
                foreach (var q in modelList)
                {
                    var ItemReqList = (await _itemrequestrepository.GetAll()).Select(b => new ItemRequest() //-- Check ItemRequest
                    {
                        ModelId = b.ModelId,
                        ItemRequestId = b.ItemRequestId

                    }).Where(d => d.ModelId == q.ModelID).ToList();

                    var QuotationReqList = (await _quotationrequestdetailsrepository.GetAll()).Select(b => new QuotationRequestDetails() //-- Check QuotationRequestDetails
                    {
                        ModelId = b.ModelId,
                        QuotationRequestDetailId = b.QuotationRequestDetailId,
                        QuotationRequestHeaderId = b.QuotationRequestHeaderId

                    }).Where(d => d.ModelId == q.ModelID).ToList();

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

                DataTable dtModel = CommonGenericService<Model>.ToDataTable(modelList);

                var dataTable = new DataGridTable
                {
                    rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                    enableSorting = true,
                    enableColResize = false,
                    DataGridColumns = GetModelColumnsfromList(dtModel),
                    DataGridRows = GetModelRowsFromList(dtModel)
                };

                return dataTable;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private List<DataGridColumn> GetModelColumnsfromList(DataTable dataTable)
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
               

                if (column.ToString().Equals("ModelName"))
                {
                    dataTableColumn.headerName = "Model Name";
                    dataTableColumn.width = 145;
                }
                if (column.ToString().Equals("ModelCode"))
                {
                    dataTableColumn.headerName = "Model Code";
                    dataTableColumn.width = 135;

                }

                if (column.ToString().Equals("MakeName"))
                {
                    dataTableColumn.headerName = "Make Name";
                    dataTableColumn.width = 135;

                }

                if (column.ToString().Equals("ItemTypeName"))
                {
                    dataTableColumn.headerName = "Item Type Name";
                    dataTableColumn.width = 135;

                }

                if (column.ToString().Equals("Status"))
                {
                    dataTableColumn.width = 115;

                }

                if (column.ToString().Equals("UploadFileName"))
                {
                    dataTableColumn.headerName = "Upload File Name";
                    dataTableColumn.width = 280;

                }

                if (!column.ToString().Equals("ModelID")
                    && !column.ToString().Equals("ItemTypeID")
                     && !column.ToString().Equals("MakeID")
                      && !column.ToString().Equals("ItemRequest")
                      && !column.ToString().Equals("UploadFileID")
                      && !column.ToString().Equals("Image")
                      && !column.ToString().Equals("UserID")
                      && !column.ToString().Equals("EntryDateTime")
                      && !column.ToString().Equals("IsActive")
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

        private static List<Object> GetModelRowsFromList(DataTable dataTable)
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


        public async Task<GenericSaveResponse<Model>> SaveModelAsync(Model model)
        {
            try
            {
                if (model.ModelId == Guid.Empty)
                {
                    model.ModelId = Guid.NewGuid();
                }

                var getmodelList = (await _repository.GetAll())
                    .Where(d => d.ItemTypeId == model.ItemTypeId && d.MakeId == model.MakeId && d.ModelName == model.ModelName).ToList();

                if (getmodelList.Count != 0)
                {

                    return new GenericSaveResponse<Model>($"The Model Name already exists. Please use a different Model Name.");
                }

                var getmodelCode = (await _repository.GetAll())
                    .Where(d => d.ItemTypeId == model.ItemTypeId && d.MakeId == model.MakeId && d.ModelCode == model.ModelCode).ToList();

                if (getmodelCode.Count != 0)
                {

                    return new GenericSaveResponse<Model>($"The Model Code already exists. Please use a different Model Code.");
                }
                model.EntryDateTime = DateTime.Now;
                await _repository.InsertAsync(model);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<Model>(true, "Successfully Saved.", model);
            }

            catch (Exception ex)
            {
                return new GenericSaveResponse<Model>($"An error occured when saving the Model :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<Model>> UpdateModelAsync(string id, Model model)
        {
            try
            {
                Model existingModel = await _repository.GetByIdAsync(model.ModelId);

                //if (existingModel.UploadFileId == null)
                //{
                //    existingModel.UploadFileId = Guid.Parse("A96FBE70-2DB4-4D00-93CD-82FD76C16354");
                //}

                if (model.UploadFileId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {
                    model.UploadFileId = null;
                }

                if (existingModel == null)
                {
                    return new GenericSaveResponse<Model>($"Model not found");
                }

                var getmodelList = (await _repository.GetAll())
                    .Where(d => d.ItemTypeId == model.ItemTypeId && d.MakeId == model.MakeId && d.ModelName == model.ModelName && d.ModelId != existingModel.ModelId).ToList();

                if (getmodelList.Count != 0)
                {

                    return new GenericSaveResponse<Model>($"The Model Name already exists. Please use a different Model Name.");
                }

                var getmodelCode = (await _repository.GetAll())
                    .Where(d => d.ItemTypeId == model.ItemTypeId && d.MakeId == model.MakeId && d.ModelCode == model.ModelCode && d.ModelId != existingModel.ModelId).ToList();

                if (getmodelCode.Count != 0)
                {

                    return new GenericSaveResponse<Model>($"The Model Code already exists. Please use a different Model Code.");
                }

                model.EntryDateTime = DateTime.Now;
                ResourceComparer<Model> Comparer = new ResourceComparer<Model>(model, existingModel);
                ResourceComparerResult<Model> CompareResult = Comparer.GetUpdatedObject();
                
                if (CompareResult.Updated)
                {
                    _repository.Update(CompareResult.Obj);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<Model>(model);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Model>($"An error occured when updating the Model :" + (ex.Message ?? ex.InnerException.Message));
            }


        }

        public async Task<GenericSaveResponse<Model>> DeleteModelAsync(Model model, string id)
        {
            try
            {
                Model existingModel = await _repository.GetByIdAsync(model.ModelId);

                if (existingModel == null)
                    return new GenericSaveResponse<Model>($"Model not found");

                else

                    _repository.Delete(model.ModelId);
                await _unitOfWork.CompleteAsync();
                return new GenericSaveResponse<Model>(model);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Model>($"An error occured when deleting the Model :" + (ex.Message ?? ex.InnerException.Message));
            }
        }


        //public async Task<List<DropDownValues>> GetDropDown(string id, Model model)
        //{
        //    List<DropDownValues> LDP = new List<DropDownValues>();




        //    IEnumerable<Model> modellst = await _repository.GetAll();

        //    foreach (Model item in userlst)
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

