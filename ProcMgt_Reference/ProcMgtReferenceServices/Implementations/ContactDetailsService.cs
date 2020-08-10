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
    public class ContactDetailsService : IConatcDetailsServices
    {
        private IGenericRepo<ContactDetails> _repository = null;
        private IUnitOfWorks _unitOfWork;

        public ContactDetailsService(IGenericRepo<ContactDetails> repository, IUnitOfWorks unitfwork)
        {
            this._repository = repository;
            this._unitOfWork = unitfwork;
        }

        public async Task<IEnumerable<ContactDetails>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<DataGridTable> getContactList(string id, ContactDetails contactdetails)
        {
            var contactDetList = (await _repository.GetAll()).Select(c => new ContactDetails
            {
                ContactDetailsId = c.ContactDetailsId,
                ContactMobile = c.ContactMobile,
                ContactName = c.ContactName,
                Email = c.Email,
                IsDefault = c.IsDefault.HasValue ? c.IsDefault.Value : false,
                SupplierId = c.SupplierId,
                UserId = c.UserId,
                EntryDateTime = c.EntryDateTime.HasValue ? c.EntryDateTime : Convert.ToDateTime("2000-01-01")
            }).Where(d => d.SupplierId== contactdetails.SupplierId).ToList();

            DataTable dtContactDetails = CommonGenericService<ContactDetails>.ToDataTable(contactDetList);

            var dataTable = new DataGridTable
            {
                rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                enableSorting = true,
                enableColResize = false,
                suppressSizeToFit = true,
                DataGridColumns = GetContactDetailsColumnsfromList(dtContactDetails),
                DataGridRows = GetContactDetailsRowsFromList(dtContactDetails)
            };

            return dataTable;
        }


        //public async Task<DataGridTable> GetContactDetailsGridAsync()
        //{
        //    var contactDetailsList = await _repository.GetAll();

        //    DataTable dtContactDetails = CommonGenericService<ContactDetails>.ToDataTable(contactDetailsList);

        //    var dataTable = new DataGridTable
        //    {
        //        rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
        //        enableSorting = true,
        //        enableColResize = false,
        //        suppressSizeToFit = true,
        //        DataGridColumns = GetContactDetailsColumnsfromList(dtContactDetails),
        //        DataGridRows = GetContactDetailsRowsFromList(dtContactDetails)
        //    };

        //    return dataTable;
        //}

        private List<DataGridColumn> GetContactDetailsColumnsfromList(DataTable dataTable)
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

                if (column.ToString().Equals("ContactName"))
                {
                    dataTableColumn.headerName = "Contact Name";
                    dataTableColumn.width = 135;
                }
                if (column.ToString().Equals("ContactMobile"))
                {
                    dataTableColumn.headerName = "Contact Mobile";
                    dataTableColumn.width = 135;
                }
                if (column.ToString().Equals("Email"))
                {
                    dataTableColumn.headerName = "E-mail";
                    dataTableColumn.width = 120;
                }
                if (column.ToString().Equals("IsDefault"))
                {
                    dataTableColumn.headerName = "IsDefault";
                    dataTableColumn.width = 120;
                }
                if (column.ToString().Equals("Default"))
                {
                    dataTableColumn.headerName = "Default?";
                    dataTableColumn.width = 125;
                }

                if (!column.ToString().Equals("ContactDetailsId")
                     && !column.ToString().Equals("UserId")
                          && !column.ToString().Equals("EntryDateTime")
                    && !column.ToString().Equals("SupplierId")

                    && !column.ToString().Equals("Supplier"))
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

        private static List<Object> GetContactDetailsRowsFromList(DataTable dataTable)
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

        public async Task<GenericSaveResponse<ContactDetails>> SaveContactDetailsAsync(ContactDetails contactdetails)
        {
            try
            {
                if (contactdetails.ContactDetailsId == Guid.Empty)
                {
                    contactdetails.ContactDetailsId = Guid.NewGuid();
                }

                await _repository.InsertAsync(contactdetails);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<ContactDetails>(true, "Successfully Saved.", contactdetails);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<ContactDetails>($"An error occured when saving the Contact Details:" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<ContactDetails>> UpdateContactDetailsAsync(string id, ContactDetails contactdetails)
        {
            try
            {
                ContactDetails existingContactDetails = await _repository.GetByIdAsync(contactdetails.ContactDetailsId);

                if (existingContactDetails == null)
                    return new GenericSaveResponse<ContactDetails>($"Contact Details not found");

                ResourceComparer<ContactDetails> Comparer = new ResourceComparer<ContactDetails>(contactdetails, existingContactDetails);
                ResourceComparerResult<ContactDetails> CompareResult = Comparer.GetUpdatedObject();

                if (CompareResult.Updated)
                {
                    _repository.Update(CompareResult.Obj);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<ContactDetails>(contactdetails);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<ContactDetails>($"An error occured when updating the Contact Details :" + (ex.Message ?? ex.InnerException.Message));
            }


        }
    }
}
