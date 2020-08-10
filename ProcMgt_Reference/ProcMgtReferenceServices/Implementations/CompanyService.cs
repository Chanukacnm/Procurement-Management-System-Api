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

namespace ProcMgt_Reference_Services
{
   public class CompanyService :ICompanyServices
    {

        private IGenericRepo<Company> _repository = null;
        private IGenericRepo<GroupCompany> _groupCompanyrepository = null;
        private IGenericRepo<CompanyGroupCompany> _companygroupCompanyrepository = null;
        private IGenericRepo<UploadFile> _uploadfileepository = null;
        private IUnitOfWorks _unitOfWork;
         
        public CompanyService(IGenericRepo<Company> repository, IGenericRepo<UploadFile> uploadfilerepository, IGenericRepo<GroupCompany> groupCompanyrepository , IGenericRepo<CompanyGroupCompany> companygroupCompanyrepository, IUnitOfWorks unitfwork)
        {
            this._repository = repository;
            this._companygroupCompanyrepository = companygroupCompanyrepository;
            this._groupCompanyrepository = groupCompanyrepository;
            this._uploadfileepository = uploadfilerepository;
            this._unitOfWork = unitfwork;
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<DataGridTable> GetCompanyGridAsync()
        {
            try
            {
                var companylist = (await _repository.GetAll()).Select(a => new CompanyResource
                {
                    CompanyID = a.CompanyId,
                    CompanyName = a.CompanyName,
                    CompanyCode = a.CompanyCode,
                    IsActive = a.IsActive.HasValue ? a.IsActive.Value : false,
                    Status = a.IsActive == true ? "Active" : "Inactive",
                    IsGroupofCompany = a.IsGroupofCompany.HasValue? a.IsGroupofCompany.Value : false,
                    CompanyStatus = a.IsGroupofCompany == true? "Group Company":"Not a Group Company",
                    CompanyLogoID = a.CompanyLogoId == null? null: a.CompanyLogoId,
                    CompanyAddressLine1 = a.CompanyAddressLine1,
                    CompanyAddressLine2 = a.CompanyAddressLine2,
                    CompanyAddressLine3 = a.CompanyAddressLine3,
                    CompanyAddressLine4 = a.CompanyAddressLine4,
                    CompanyTelephoneNo = a.CompanyTelephoneNo,
                    CompanyFax =a.CompanyFax,
                    Email = a.Email,
                    CompanyWeb = a.CompanyWeb,
                    CompanyRegistrationNo = a.CompanyRegistrationNo,
                    VatRegistrationNo = a.VatRegistrationNo,
                    UploadFileName = a.CompanyLogoId == null ? "" : _uploadfileepository.GetByIdAsync(a.CompanyLogoId).Result.UploadFileName.ToString(),



                }).OrderBy(b => b.CompanyName).ToList();

                DataTable dtCompany = CommonGenericService<Company>.ToDataTable(companylist);

                var dataTable = new DataGridTable
                {
                    rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                    enableSorting = true,
                    enableColResize = false,
                    suppressSizeToFit = true,
                    DataGridColumns = GetCompanyColumnsfromList(dtCompany),
                    DataGridRows = GetCompanyRowsFromList(dtCompany)
                };

                return dataTable;

            }
            catch(Exception ex)
            {
                return null;
            }
        }

        private List<DataGridColumn> GetCompanyColumnsfromList(DataTable dataTable)
        {
            var DataGridColumns = new List<DataGridColumn>();

            foreach (DataColumn column in dataTable.Columns)
            {
                var dataTableColumn = new DataGridColumn
                {
                    field = column.ToString().Replace(" ", "_"),
                    headerName = column.ToString(),
                    //visible = 

                    //halign = DataAlignEnum.Center.ToString().ToLower()
                };

                if (!column.ToString().Equals("CompanyID")
                    && !column.ToString().Equals("IsActive")
                    && !column.ToString().Equals("IsGroupofCompany")
                    && !column.ToString().Equals("CompanyLogoID"))
                {
                    dataTableColumn.hide = false;
                }
                else
                {
                    dataTableColumn.hide = true;
                }
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

        private static List<Object> GetCompanyRowsFromList(DataTable dataTable)
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

        public async Task<DataGridTable> getGroupCompanyGridAsync(string id, Company company)
        {
            var groupCompanyList = (await _groupCompanyrepository.GetAll()).Select(c => new GroupCompanyResource
            {
                GroupCompanyID = c.GroupCompanyId,
                GroupCompanyName = c.GroupCompanyName,
                GroupCompanyCode = c.GroupCompanyCode,
                IsActive = c.IsActive,
                Status = c.IsActive == true ? "Active" : "Inactive",
                GroupCompanyLogoID = c.GroupCompanyLogoId == null ? null : c.GroupCompanyLogoId,
                GroupCompanyAddressLine1 = c.GroupCompanyAddressLine1,
                GroupCompanyAddressLine2 = c.GroupCompanyAddressLine2,
                GroupCompanyAddressLine3 = c.GroupCompanyAddressLine3,
                GroupCompanyAddressLine4 = c.GroupCompanyAddressLine4,
                GcompanyTelephoneNo = c.GcompanyTelephoneNo,
                GcompanyFax = c.GcompanyFax,
                GcompanyEmail = c.GcompanyEmail,
                GcompanyWeb = c.GcompanyWeb,
                GcompanyRegistrationNo = c.GcompanyRegistrationNo,
                VatRegistrationNo = c.VatRegistrationNo,
                CompanyID = c.CompanyId,
                GroupUploadFileName = c.GroupCompanyLogoId == null ? "" : _uploadfileepository.GetByIdAsync(c.GroupCompanyLogoId).Result.UploadFileName.ToString(),


            }).Where(d => d.CompanyID == company.CompanyId).ToList();

            DataTable dtGrpCompanyDetails = CommonGenericService<GroupCompany>.ToDataTable(groupCompanyList);

            var dataTable = new DataGridTable
            {
                rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                enableSorting = true,
                enableColResize = false,
                suppressSizeToFit = true,
                DataGridColumns = GetGroupCompanyDetailsColumnsfromList(dtGrpCompanyDetails),
                DataGridRows = GetGroupCompanyRowsFromList(dtGrpCompanyDetails)
            };

            return dataTable;
        }

        private List<DataGridColumn> GetGroupCompanyDetailsColumnsfromList(DataTable dataTable)
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

                

                if (!column.ToString().Equals("GroupCompanyID")
                    && !column.ToString().Equals("IsActive")
                    && !column.ToString().Equals("CompanyID")
                    && !column.ToString().Equals("GroupCompany")
                    && !column.ToString().Equals("GroupCompanyLogoID"))
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

        private static List<Object> GetGroupCompanyRowsFromList(DataTable dataTable)
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

        public async Task<GenericSaveResponse<Company>> SaveCompanyAsync(Company company)
        {
            try
            {

                if (company.CompanyId == Guid.Empty)
                {
                    company.CompanyId = Guid.NewGuid();
                }

                if (company.IsGroupofCompany == true)
                {
                    foreach (GroupCompany grpCompany in company.GroupCompany)
                    {
                        grpCompany.GroupCompanyId = Guid.NewGuid();
                        GroupCompany grpcon = new GroupCompany();
                        grpCompany.CompanyId = company.CompanyId;


                        await _groupCompanyrepository.InsertAsync(grpCompany);
                    }

                    //foreach (GroupCompany cmpanygrpCompany in company.GroupCompany)
                    //{
                    //    CompanyGroupCompany grpcon = new CompanyGroupCompany();
                    //    grpcon.CompanyGroupCompanyId = Guid.NewGuid();
                    //    grpcon.CompanyId = company.CompanyId;
                    //    grpcon.GroupCompanyId = cmpanygrpCompany.GroupCompanyId;


                    //    await _companygroupCompanyrepository.InsertAsync(grpcon);
                    //}

                }





                await _repository.InsertAsync(company);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<Company>(company);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Company>($"An error occured when saving the Company :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<Company>> UpdateCompanyAsync(string id, Company company)
        {
            try
            {
                Company existingCompany = await _repository.GetByIdAsync(company.CompanyId);

                var grpCompList = (await _groupCompanyrepository.GetAll()).Select(c => new GroupCompany
                {
                    GroupCompanyId = c.GroupCompanyId,
                    GroupCompanyName = c.GroupCompanyName,
                    GroupCompanyCode = c.GroupCompanyCode,
                    IsActive = c.IsActive,
                    GroupCompanyLogoId = c.GroupCompanyLogoId == null ? null : c.GroupCompanyLogoId,
                    GroupCompanyAddressLine1 = c.GroupCompanyAddressLine1,
                    GroupCompanyAddressLine2 = c.GroupCompanyAddressLine2,
                    GroupCompanyAddressLine3 = c.GroupCompanyAddressLine3,
                    GroupCompanyAddressLine4 = c.GroupCompanyAddressLine4,
                    GcompanyTelephoneNo = c.GcompanyTelephoneNo,
                    GcompanyFax = c.GcompanyFax,
                    GcompanyEmail = c.GcompanyEmail,
                    GcompanyWeb = c.GcompanyWeb,
                    GcompanyRegistrationNo = c.GcompanyRegistrationNo,
                    VatRegistrationNo = c.VatRegistrationNo,
                    CompanyId = c.CompanyId,

                }).Where(d => d.CompanyId == company.CompanyId).ToList();

                var cmpGrpCompList = (await _companygroupCompanyrepository.GetAll()).Select(c => new CompanyGroupCompany
                {
                    GroupCompanyId = c.GroupCompanyId,
                    CompanyId = c.CompanyId,
                    CompanyGroupCompanyId = c.CompanyGroupCompanyId

                }).Where(d => d.CompanyId == company.CompanyId).ToList();

                if (existingCompany == null)
                {
                    return new GenericSaveResponse<Company>($"Company not found");
                }

                if (company.IsGroupofCompany == true)
                {

                    foreach (CompanyGroupCompany cmpanygrpCompany in cmpGrpCompList)
                    {
                        CompanyGroupCompany grpcon = new CompanyGroupCompany();
                        //grpcon.CompanyGroupCompanyId = Guid.NewGuid();
                        grpcon.CompanyId = company.CompanyId;
                        grpcon.GroupCompanyId = cmpanygrpCompany.GroupCompanyId;
                        grpcon.CompanyGroupCompanyId = cmpanygrpCompany.CompanyGroupCompanyId;


                        _companygroupCompanyrepository.Delete(grpcon.CompanyGroupCompanyId);
                    }

                    foreach (GroupCompany grpCompany in grpCompList)
                    {
                        GroupCompany grpcon = new GroupCompany();
                        grpcon.CompanyId = company.CompanyId;
                        grpcon.GroupCompanyId = grpCompany.GroupCompanyId;


                        _groupCompanyrepository.Delete(grpcon.GroupCompanyId);
                    }

                    foreach (GroupCompany grpCompany in company.GroupCompany)
                    {
                        grpCompany.GroupCompanyId = Guid.NewGuid();
                        GroupCompany grpcon = new GroupCompany();
                        grpCompany.CompanyId = company.CompanyId;


                        await _groupCompanyrepository.InsertAsync(grpCompany);
                    }

                    //foreach (GroupCompany cmpanygrpCompany in company.GroupCompany)
                    //{
                    //    CompanyGroupCompany grpcon = new CompanyGroupCompany();
                    //    grpcon.CompanyGroupCompanyId = Guid.NewGuid();
                    //    grpcon.CompanyId = company.CompanyId;
                    //    grpcon.GroupCompanyId = cmpanygrpCompany.GroupCompanyId;


                    //    await _companygroupCompanyrepository.InsertAsync(grpcon);
                    //}

                    //foreach (GroupCompany grpCompany in company.GroupCompany)
                    //{
                    //    grpCompany.GroupCompanyId = Guid.NewGuid();
                    //    GroupCompany grpcon = new GroupCompany();
                    //    grpCompany.CompanyId = company.CompanyId;


                    //    await _groupCompanyrepository.InsertAsync(grpCompany);
                    //}

                }






                ResourceComparer<Company> Comparer = new ResourceComparer<Company>(company, existingCompany);
                ResourceComparerResult<Company> CompareResult = Comparer.GetUpdatedObject();

                if (CompareResult.Updated)
                {
                    _repository.Update(CompareResult.Obj);
                    
                }
                await _unitOfWork.CompleteAsync();
                return new GenericSaveResponse<Company>(company);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Company>($"An error occured when updating the Company :" + (ex.Message ?? ex.InnerException.Message));
            }


        }

        public async Task<GenericSaveResponse<Company>> UpdateCompanyGroupCompanyAsync(string id, Company company)
        {
            try
            {
                if (company.IsGroupofCompany == true)
                {
                    foreach (GroupCompany cmpanygrpCompany in company.GroupCompany)
                    {
                        CompanyGroupCompany grpcon = new CompanyGroupCompany();
                        grpcon.CompanyGroupCompanyId = Guid.NewGuid();
                        grpcon.CompanyId = company.CompanyId;
                        grpcon.GroupCompanyId = cmpanygrpCompany.GroupCompanyId;


                        await _companygroupCompanyrepository.InsertAsync(grpcon);
                    }
                }

                await _unitOfWork.CompleteAsync();
                return new GenericSaveResponse<Company>(company);
            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Company>($"An error occured when updating the Company Group Company :" + (ex.Message ?? ex.InnerException.Message));
            }

        }

            //public async Task<List<DropDownValues>> GetDropDown(string id, Company company)
            //{
            //    List<DropDownValues> LDP = new List<DropDownValues>();




            //    IEnumerable<Company> companylst = await _repository.GetAll();

            //    foreach (Company item in companylst)
            //    {
            //        DropDownValues itemLDP = new DropDownValues();

            //        itemLDP.Id = item.CompanyId.ToString(); 
            //        itemLDP.Value = item.CompanyName;
            //        LDP.Add(itemLDP);
            //    }

            //    return LDP;

            //}

        }
}
