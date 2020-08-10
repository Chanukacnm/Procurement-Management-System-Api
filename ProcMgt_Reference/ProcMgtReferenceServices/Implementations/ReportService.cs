using ProcMgt_Reference_Core;
using ProcMgt_Reference_Core.GenericRepoInter;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ProcMgt_Reference_Core.Resources;

namespace ProcMgt_Reference_Services.Implementations
{
    public class ReportService : IReportServices
    {
        private IGenericRepo<ItemRequest> _ItemReqrepository = null;
        private IGenericRepo<Item> _itemrepository = null;
        private IGenericRepo<Category> _categoryrepository = null;
        private IGenericRepo<User> _userrepository = null;
        private IGenericRepo<Approver> _approverrepository = null;
        private IGenericRepo<Arnheader> _arnHeaderrepository = null;
        private IGenericRepo<Arndetail> _arnDetrepository = null;
        private IGenericRepo<Department> _departmentrepository = null;
        private IGenericRepo<ItemType> _itemtyperepository = null;
        private IUnitOfWorks _unitOfWork;
     //   private IGenericRepo<QuotationRequestHeader> _QuotationRequestHeaderrepository = null; //rajitha
        private IGenericRepo<Supplier> _supplierrepository = null; //rajitha
      //  private IGenericRepo<Poheader> _poheaderrepository = null; //rajitha


        public ReportService(IGenericRepo<Arndetail> arnDetrepository, IGenericRepo<ItemType> itemtyperepository, IGenericRepo<Department> departmentrepository, IGenericRepo<Approver> approverrepository, IGenericRepo<Arnheader> arnHeaderrepository, IGenericRepo<ItemRequest> itemReqrepository, IGenericRepo<User> userrepository, IGenericRepo<Item> itemrepository, IGenericRepo<Category> categoryrepository, IUnitOfWorks unitfwork, IGenericRepo<QuotationRequestHeader> QuotationRequestHeaderrepository, IGenericRepo<Supplier> supplierrepository, IGenericRepo<Poheader> _poheaderrepository) 
        {
            this._ItemReqrepository = itemReqrepository;
            this._categoryrepository = categoryrepository;
            this._userrepository = userrepository;
            this._itemrepository = itemrepository;
            this._departmentrepository = departmentrepository;
            this._approverrepository = approverrepository;
            this._arnDetrepository = arnDetrepository;
            this._arnHeaderrepository = arnHeaderrepository;
            this._itemtyperepository = itemtyperepository;
            this._unitOfWork = unitfwork;
           // this._QuotationRequestHeaderrepository = QuotationRequestHeaderrepository; //rajitha
            this._supplierrepository = supplierrepository; //rajitha
           // this._poheaderrepository = _poheaderrepository; // rajitha
        }

        public async Task<IEnumerable<ItemRequestResource>> GetItemReqData(DateTime fromDate, DateTime toDate)
        {
            var itemRequestList = (await _ItemReqrepository.GetAll()).Select(a => new ItemRequestResource()
            {

                ItemRequestID = a.ItemRequestId,
                RequestTitle = a.RequestTitle,
                CategoryID = a.CategoryId,
                AssetCode = a.AssetCode,
                NoOfUnits = a.NoOfUnits,
                IsApproved = a.IsApproved,
                Status = a.IsApproved == true ? "Approved" : "Rejected",
                RequestedDateTime = a.RequestedDateTime,
                RequestedUserID = a.RequestedUserId,

                CategoryName = _categoryrepository.GetByIdAsync(a.CategoryId).Result.CategoryName.ToString(),
                RequestedUserName = _userrepository.GetByIdAsync(a.RequestedUserId).Result.UserName.ToString(),






            }).Where(d => (d.RequestedDateTime >= fromDate) && (d.RequestedDateTime <= toDate)).ToList();

            return itemRequestList;
        }

        //rajitha------------------------------------------------------------------------------------------------------------------

        public async Task<IEnumerable<Supplier>> GetAllSuppliersAsync()
        {
            var getallSuppliers = (await _supplierrepository.GetAll()).Select(c => new Supplier
            {
                SupplierId = c.SupplierId,
                SupplierName = c.SupplierName,
                IsActive = c.IsActive



            }).Where(d => d.IsActive == true).OrderBy(e => e.SupplierName);

            return getallSuppliers;


        }

        //ponumbers-------------------------------------------------------------------------------------


        //public async Task<IEnumerable<QuotationRequestHeaderResource>> GetPONumbersAsync(string suppllierID)
        //{
        //    try
        //    {
        //        var getallQuotationRequestHeaderID = (await _QuotationRequestHeaderrepository.GetAll()).Select(c => new QuotationRequestHeaderResource
        //        {

        //            QuotationRequestHeaderID = c.QuotationRequestHeaderId,
        //            SupplierID = c.SupplierId,
        //            SupplierName = _supplierrepository.GetByIdAsync(c.SupplierId).Result.SupplierName.ToString(),

        //        }).ToList();

        //        //-------------------
        //        foreach (var q in getallQuotationRequestHeaderID)
        //        {
        //            var getsupplierponumber = (await _poheaderrepository.GetAll()).Select(b => new Poheader()
        //            {
        //                QuotationRequestHeaderId = b.QuotationRequestHeaderId,
        //                PoheaderId = b.PoheaderId


        //            }).Where(d => d.QuotationRequestHeaderId == q.QuotationRequestHeaderId).ToList();
        //        }
        //        //-------------------


        //        return getsupplierponumber;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //}


        // end----------------------------------------------------------------------------------------

        public async Task<IEnumerable<ItemRequestResource>> GetApprovedItemReqData(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var ApproveditemRequestList = (await _ItemReqrepository.GetAll()).Select(a => new ItemRequestResource()
                {

                    ItemRequestID = a.ItemRequestId,
                    RequestTitle = a.RequestTitle,
                    ItemID = a.ItemId,
                    NoOfUnits = a.NoOfUnits,
                    IsApproved = a.IsApproved,                    
                    IsRejected = a.IsRejected,
                    Status = a.IsApproved == true ? "Approved" : a.IsRejected == true ? "Rejected" : "Pending",
                    RequestedDateTime = a.RequestedDateTime,
                    RequestedUserID = a.RequestedUserId,
                    ApprovedDateTime = a.ApprovedDateTime.HasValue ? a.ApprovedDateTime : Convert.ToDateTime("1900-01-01"),
                    ApproverID = a.ApproverId,
                    ApprovedUserID = a.ApprovedUserId == null ? null : a.ApprovedUserId,
                    ApprovalComment = a.ApprovalComment,

                    ItemDescription = _itemrepository.GetByIdAsync(a.ItemId).Result.ItemDescription.ToString(),
                    RequestedUserName = _userrepository.GetByIdAsync(a.RequestedUserId).Result.UserName.ToString(),
                    ApprovedUserName = a.ApprovedUserId == null ? "" : _userrepository.GetByIdAsync(a.ApprovedUserId).Result.UserName.ToString(),




                }).Where(d => ((d.RequestedDateTime >= fromDate) && (d.RequestedDateTime <= toDate)) && (d.IsApproved == true)).ToList();

                return ApproveditemRequestList;
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }

        public async Task<IEnumerable<ArnheaderResource>> GetGRNData(DateTime fromDate, DateTime toDate)
        {

            var grnList = (await _arnHeaderrepository.GetAll()).Select(a => new ArnheaderResource()
            {
                ArnheaderID = a.ArnheaderId,
                Arnnumber = a.Arnnumber,
                RecivedDate = a.RecivedDate,
                InvoiceNo = a.InvoiceNo,
                PoheaderID = a.PoheaderId,
                //Arndetail =a.Arndetail

            }).Where(d => ((d.RecivedDate >= fromDate) && (d.RecivedDate <= toDate))).ToList();

            foreach (ArnheaderResource arnheader in grnList)
            {
                Arnheader existingArnheader = await _arnHeaderrepository.GetByIdAsync(arnheader.ArnheaderID);

                IEnumerable<ArndetailResource> grnDetList = (await _arnDetrepository.GetAll()).Select(b => new ArndetailResource()
                {
                    ArndetailID = b.ArndetailId,
                    ItemID = b.ItemId,
                    InvoiceQty = b.InvoiceQty,
                    RecivedQty = b.RecivedQty,
                    RejectedQty = b.RejectedQty,
                    ArnheaderID = b.ArnheaderId,

                    ItemDescription = _itemrepository.GetByIdAsync(b.ItemId).Result.ItemDescription.ToString(),

                }).Where(c => c.ArnheaderID == existingArnheader.ArnheaderId).ToList();

                arnheader.ArndetailResource = grnDetList;
            }
            // ArnheaderResource gh = new ArnheaderResource();


            return grnList;
        }

        public async Task<IEnumerable<ItemRequestResource>> GetReconciliationData(DateTime fromDate, DateTime toDate)
        {

            var getReconsilationData = (await _ItemReqrepository.GetAll()).Where(d => (d.RequestedDateTime >= fromDate) && (d.RequestedDateTime <= toDate)).GroupBy(x => new { x.DepartmentId , x.ItemId , x.ItemTypeId }) .Select(a => new ItemRequestResource()
                {

                    
                    ItemID = a.Key.ItemId,
                    ItemTypeID = a.Key.ItemTypeId,
                    NoOfUnits = a.Sum(x =>  x.NoOfUnits),
                    DepartmentID = a.Key.DepartmentId,
                    TotalItemRequests = a.Count().ToString(),
                    TotalApprovedItemRequests =  a.Count(x => x.IsApproved == true).ToString(),
                    TotalRejectedItemRequests = a.Count(x => x.IsRejected == true).ToString(),
                    TotalPendingItemRequests = ((a.Count()) - ((a.Count(x => x.IsApproved == true))+(a.Count(x => x.IsRejected == true)))).ToString(),
                    
                    DepartmentName = _departmentrepository.GetByIdAsync(a.Key.DepartmentId).Result.DepartmentName.ToString(),
                    ItemTypeName = _itemtyperepository.GetByIdAsync(a.Key.ItemTypeId).Result.ItemTypeName.ToString(),
                    ItemDescription = _itemrepository.GetByIdAsync(a.Key.ItemId).Result.ItemDescription.ToString(),


                }).ToList();

            //return null;

            return getReconsilationData;
        }
    }
}
