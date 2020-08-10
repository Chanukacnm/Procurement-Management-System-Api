using ProcMgt_Reference_Core;
using ProcMgt_Reference_Core.GenericRepoInter;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Communication;
using ProcMgt_Reference_Services.Helpers;
using ProcMgt_Reference_Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ProcMgt_Reference_Services.Interfaces;
using ProcMgt_Reference_Services.Common;
using System.Data;
using System.Globalization;
using System.Linq;

namespace ProcMgt_Reference_Services.Implementations
{
    public class SupplierService : ISupplierServices
    {
        private IGenericRepo<Supplier> _repository = null;
        private IGenericRepo<ContactDetails> _contactrepository = null;
        private IGenericRepo<SupplierRegisteredItems> _suppItmrepository = null;
        private IUnitOfWorks _unitOfWork;

        public SupplierService(IGenericRepo<Supplier> repository, IGenericRepo<ContactDetails> contactrepository, IGenericRepo<SupplierRegisteredItems> suppItmrepository, IUnitOfWorks unitfwork)
        {
            this._repository = repository;
            this._contactrepository = contactrepository;
            this._suppItmrepository = suppItmrepository;
            this._unitOfWork = unitfwork;
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            var getallSupplier = (await _repository.GetAll()).Select(c => new Supplier
            {
                SupplierId = c.SupplierId,
                SupplierName =  c.SupplierName,
                IsActive = c.IsActive


               
            }).Where(d => d.IsActive == true).OrderBy(e => e.SupplierName);

            return getallSupplier;
           
                
        }

        public async Task<DataGridTable> GetSupplierGridAsync()
        {

            //List<ContactDetails> con1 = new List<ContactDetails>();
            //List<SupplierResource> supplierList1 = new List<SupplierResource>();
            //var supplierList = await _repository.GetAll();
            var supplierList = (await _repository.GetAll()).Select(a => new SupplierResource
            {
                SupplierID = a.SupplierId,
                SupplierName = a.SupplierName,
                BrNo = a.BrNo,
                Address = a.Address,
                Telephone = a.Telephone,
                BillingName = a.BillingName,
                BillingAddress = a.BillingAddress,
                BankID = a.BankId,
                BranchID = a.BranchId,
                AccountTypeID = a.AccountTypeId,
                AccountNo = a.AccountNo,
                AccountName = a.AccountName,
                PaymentMethodID = a.PaymentMethodId,
                SupplierTypeID = a.SupplierTypeId,
                IsActive = a.IsActive,
                Status = a.IsActive == true ? "Active" : "Inactive",
                UserID = a.UserId,
                EntryDateTime = a.EntryDateTime.HasValue ? a.EntryDateTime : Convert.ToDateTime("2000-01-01")






            }).OrderBy(b => b.SupplierName).ToList();

            

            //try
            //{
                
            //    foreach (var x in supplierList)
            //    {
                   


            //        var items = _contactrepository.GetAll().Result.Where(b => b.SupplierId == x.SupplierID).ToList();
            //        foreach (var item in items)
            //        {
            //            ContactDetails con = new ContactDetails();  

            //            con.ContactDetailsId = item.ContactDetailsId;
            //            con.ContactMobile = item.ContactMobile;
            //            con.Email = item.Email;
            //            con.IsDefault = item.IsDefault;
            //            con.ContactName = item.ContactName;
            //            con.SupplierId = item.SupplierId;
            //            //con.Supplier =[]; 
            //            con1.Add(con);


                       

                        


            //        }
            //        supplierList1.Add(x);


            //    }
            //   // supplierList1.Add(supplierList);



            //}
            //catch(Exception ex)
            //{
               
            //}
           

            







            DataTable dtSupplier = CommonGenericService<Supplier>.ToDataTable(supplierList);

            var dataTable = new DataGridTable
            {
                rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                enableSorting = true,
                enableColResize = false,
                suppressSizeToFit = true,
                DataGridColumns = GetSupplierColumnsfromList(dtSupplier),
                DataGridRows = GetSupplierRowsFromList(dtSupplier)
            };

            return dataTable;
        }

        private List<DataGridColumn> GetSupplierColumnsfromList(DataTable dataTable)
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

                if (column.ToString().Equals("SupplierName"))
                {
                    dataTableColumn.headerName = "Supplier Name";
                    dataTableColumn.width = 130;
                }
                if (column.ToString().Equals("BrNo"))
                {
                    dataTableColumn.headerName = "Br No";
                    dataTableColumn.width = 90;
                }
                if (column.ToString().Equals("Address"))
                {
                    dataTableColumn.width = 110;
                }
                if (column.ToString().Equals("Telephone"))
                {
                    dataTableColumn.width = 120;
                }
                if (column.ToString().Equals("BillingName"))
                {
                    dataTableColumn.headerName = "Billing Name";
                    dataTableColumn.width = 130;
                }
                if (column.ToString().Equals("BillingAddress"))
                {
                    dataTableColumn.headerName = "Billing Address";
                    dataTableColumn.width = 140;
                }
                if (column.ToString().Equals("Status"))
                {
                    dataTableColumn.width = 100;
                }


                if (!column.ToString().Equals("SupplierName")
                    && !column.ToString().Equals("BrNo")
                    && !column.ToString().Equals("Address")
                    && !column.ToString().Equals("Telephone")
                    && !column.ToString().Equals("BillingName")
                    && !column.ToString().Equals("UserId")
                          && !column.ToString().Equals("EntryDateTime")
                    && !column.ToString().Equals("BillingAddress")
                    && !column.ToString().Equals("Status")
                    )
                {
                    dataTableColumn.hide = true;
                }
                else
                {
                    dataTableColumn.hide = false;
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

        private static List<Object> GetSupplierRowsFromList(DataTable dataTable)
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

        public async Task<DataGridTable> GetContactDetailsGridAsync(ContactDetails contactDetails)
        {
            try
            {
                var contactDetailsList = (await _contactrepository.GetAll()).Where(c => c.SupplierId == contactDetails.SupplierId).Select(d => new ContactDetailsResource()
                {
                    ContactDetailsID = d.ContactDetailsId,
                    SupplierID = d.SupplierId,
                    ContactMobile = d.ContactMobile,
                    ContactName = d.ContactName,
                    Email = d.Email,
                    IsDefault = d.IsDefault.HasValue ? d.IsDefault.Value : false,
                    Default = d.IsDefault == true? "Yes": "No",
                    
                }).ToList();

                DataTable dtContactDetails = CommonGenericService<ContactDetails>.ToDataTable(contactDetailsList);

                var dataTable = new DataGridTable
                {
                    rowSelection = Enum.GetName(typeof(rowSelection), rowSelection.single),
                    enableSorting = true,
                    enableColResize = false,
                    suppressSizeToFit = true,
                    DataGridColumns = GetcontactDetailsColumnsfromList(dtContactDetails),
                    DataGridRows = GetcontactDetailsRowsFromList(dtContactDetails)
                };

                return dataTable;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        private List<DataGridColumn> GetcontactDetailsColumnsfromList(DataTable dataTable)
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


                if (column.ToString().Equals("ContactName"))
                {
                    dataTableColumn.headerName = "Contact Name";
                    dataTableColumn.width = 135;
                }
                if (column.ToString().Equals("Contact Mobile"))
                {
                    dataTableColumn.headerName = "Contact Mobile";
                    dataTableColumn.width = 135;
                }
                if (column.ToString().Equals("Email"))
                {
                    dataTableColumn.headerName = "E-mail";
                    dataTableColumn.width = 120;
                }
                if (column.ToString().Equals("Default"))
                {
                    dataTableColumn.headerName = "Default?";
                    dataTableColumn.width = 125;
                }

                if (!column.ToString().Equals("ContactDetailsID")
                    && !column.ToString().Equals("SupplierID")
                    && !column.ToString().Equals("IsDefault"))
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


        private static List<Object> GetcontactDetailsRowsFromList(DataTable dataTable)
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


        public async Task<GenericSaveResponse<Supplier>> SaveSupplierAsync(Supplier supplier)
        {
            try
            {
                if (supplier.SupplierId == Guid.Empty)
                {
                    supplier.SupplierId = Guid.NewGuid();
                }

                foreach (ContactDetails conitem in supplier.ContactDetails)
                {
                    conitem.ContactDetailsId = Guid.NewGuid();
                    ContactDetails con = new ContactDetails();
                    con.SupplierId = supplier.SupplierId;
                    conitem.EntryDateTime = DateTime.Now;
                    conitem.UserId = supplier.UserId;

                    await _contactrepository.InsertAsync(conitem); 
                } 

                foreach (SupplierRegisteredItems supplierItm in supplier.SupplierRegisteredItems)
                {
                    supplierItm.SupplierRegisteredItemsId = Guid.NewGuid();
                    SupplierRegisteredItems supItm = new SupplierRegisteredItems();
                    supItm.SupplierId = supplier.SupplierId;
                    supplierItm.EntryDateTime = DateTime.Now;
                    supplierItm.UserId = supplier.UserId;

                    await _suppItmrepository.InsertAsync(supplierItm);
                }

                supplier.EntryDateTime = DateTime.Now;


                await _repository.InsertAsync(supplier);
                await _unitOfWork.CompleteAsync();

                return new GenericSaveResponse<Supplier>(true, "Successfully Saved.", supplier);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Supplier>($"An error occured when saving the Supplier Master :" + (ex.Message ?? ex.InnerException.Message));
            }
        }

        public async Task<GenericSaveResponse<Supplier>> UpdateSupplierAsync(string id, Supplier supplier)
        {
            try
            {
                Supplier existingSupplier = await _repository.GetByIdAsync(supplier.SupplierId);
                //ContactDetails existingContact = await _contactrepository.GetByIdAsync(supplier.SupplierId);
                //SupplierRegisteredItems existingSupItems = await _suppItmrepository.GetByIdAsync(supplier.SupplierId);

                var contactDetList = (await _contactrepository.GetAll()).Select(c => new ContactDetails
                {
                    ContactDetailsId = c.ContactDetailsId,
                    ContactMobile = c.ContactMobile,
                    ContactName = c.ContactName,
                    Email = c.Email,
                    IsDefault = c.IsDefault.HasValue ? c.IsDefault.Value : false,
                    SupplierId = c.SupplierId
                }).Where(d => d.SupplierId == supplier.SupplierId).ToList();



                var supplierRegistereditemsList = (await _suppItmrepository.GetAll()).Select(a => new SupplierRegisteredItems()
                {
                    SupplierRegisteredItemsId = a.SupplierRegisteredItemsId,
                    ItemTypeId = a.ItemTypeId,
                    MinimumItemCapacity = a.MinimumItemCapacity,
                    SupplierLeadTime = a.SupplierLeadTime,
                    SupplierId = a.SupplierId

                    //itemTypeName = _itemtyperepository.GetByIdAsync(a.ItemTypeId).Result.ItemTypeName.ToString(),

                }).Where(d => d.SupplierId == supplier.SupplierId).ToList();

                if (existingSupplier == null)
                {
                    return new GenericSaveResponse<Supplier>($"Supplier Master not found");
                }

                foreach (ContactDetails conitem in contactDetList)
                {
                    
                    ContactDetails con = new ContactDetails();
                    con.SupplierId = conitem.SupplierId;
                    con.ContactDetailsId = conitem.ContactDetailsId;
                    


                    _contactrepository.Delete(con.ContactDetailsId);
                }

                foreach (ContactDetails conitems in supplier.ContactDetails)
                {
                    conitems.ContactDetailsId = Guid.NewGuid();
                    ContactDetails con = new ContactDetails();
                    con.SupplierId = supplier.SupplierId;
                    conitems.EntryDateTime = DateTime.Now;


                    await _contactrepository.InsertAsync(conitems);
                }

                foreach (SupplierRegisteredItems supplierItms in supplierRegistereditemsList)
                {
                    SupplierRegisteredItems supItm = new SupplierRegisteredItems();
                    supItm.SupplierId = supplierItms.SupplierId;
                    supItm.SupplierRegisteredItemsId = supplierItms.SupplierRegisteredItemsId;

                     _suppItmrepository.Delete(supItm.SupplierRegisteredItemsId);
                }

                foreach (SupplierRegisteredItems supplierItm in supplier.SupplierRegisteredItems)
                {
                    supplierItm.SupplierRegisteredItemsId = Guid.NewGuid();
                    SupplierRegisteredItems supItm = new SupplierRegisteredItems();
                    supItm.SupplierId = supplier.SupplierId;
                    supplierItm.EntryDateTime = DateTime.Now;
                    await _suppItmrepository.InsertAsync(supplierItm);
                }


                supplier.EntryDateTime = DateTime.Now;
                ResourceComparer<Supplier> Comparer = new ResourceComparer<Supplier>(supplier, existingSupplier);
                ResourceComparerResult<Supplier> CompareResult = Comparer.GetUpdatedObject();

                if (CompareResult.Updated)
                {
                    _repository.Update(CompareResult.Obj);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<Supplier>(supplier);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Supplier>($"An error occured when updating the Supplier Master :" + (ex.Message ?? ex.InnerException.Message));
            }


        }

        public async Task<GenericSaveResponse<Supplier>> DeleteSupplierAsync(string id, Supplier supplier)
        {
            try
            {
                Supplier existingSupplier = await _repository.GetByIdAsync(supplier.SupplierId);

                if (existingSupplier == null)
                {
                    return new GenericSaveResponse<Supplier>($"Supplier Master not found");
                }
                else
                { 
                
                    _repository.Delete(supplier.SupplierId);
                    await _unitOfWork.CompleteAsync();
                }

                return new GenericSaveResponse<Supplier>(supplier);

            }
            catch (Exception ex)
            {
                return new GenericSaveResponse<Supplier>($"An error occured when updating the Supplier Master :" + (ex.Message ?? ex.InnerException.Message));
            }


        }


    }
}
