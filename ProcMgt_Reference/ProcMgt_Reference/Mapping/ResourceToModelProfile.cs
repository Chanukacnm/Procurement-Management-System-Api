using ProcMgt_Reference_Core.Resources;
using AutoMapper;
using ProcMgt_Reference_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcMgt_Reference.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CategoryMasterResource, Category>(); 
            CreateMap<ItemTypeMasterResource, ItemType>();
            CreateMap<ApprovalPatternTypeResource, ApprovalPatternType>();
            CreateMap<DepartmentResource, Department>();
            CreateMap<MeasurementUnitResource, MeasurementUnits>();
            CreateMap<PaymentMethodResource, PaymentMethod>();
            CreateMap<UserRoleResource, UserRole>();
            CreateMap<CompanyResource, Company>();
            CreateMap<SupplierResource, Supplier>();
            CreateMap<ModelResource, Model>();
            CreateMap<BankResource, Bank>();
            CreateMap<BankBranchResource,BankBranch>();
            CreateMap<AccoutTypeResource, AccountType>(); 
            CreateMap<SupplierRegisteredItemsResource, SupplierRegisteredItems>();
            CreateMap<MinimumCapacityResource, MinimumCapacity>();
            CreateMap<SupplierTypeResources, SupplierType>();
            CreateMap<TaxResource, Tax>();
            //CreateMap<ItemCategoryResource, ItemCategory>();
            //CreateMap<ReOrderLevelResource, ReOrderLevel>();
            CreateMap<MakeResource, Make>();
            CreateMap<ContactDetailsResource, ContactDetails>();
            CreateMap<PriorityResource, Priority>();
            CreateMap<ApproverResource, Approver>();
            CreateMap<ItemRequestResource, ItemRequest>();
            CreateMap<ApprovalFlowManagementResource, ApprovalFlowManagement>();
            CreateMap<UserResource, User>();
            CreateMap<DesignationResource, Designation>();
            CreateMap<ApprovalScreenResource, ItemRequest>();
            CreateMap<UserResource, ItemRequest>();
            CreateMap<UserResource, Menu>();
            CreateMap<UserResource, RoleMenu>();
            CreateMap<AuthenticatorResource, User>();
            CreateMap<AuthenticatorResource, Menu>();
            CreateMap<ItemResource, Item>();
            CreateMap<PoHeaderResource, Poheader>();
            CreateMap<PoDetailResource, Podetail>();
            CreateMap<QuotationRequestHeaderResource, QuotationRequestHeader>();
            CreateMap<QuotationRequestDetailsResource, QuotationRequestDetails>();
            CreateMap<ArndetailResource, Arndetail>();
            CreateMap<ArnheaderResource, Arnheader>();
            CreateMap<PoDetailResource, Item>();
            CreateMap<PoHeaderResource, Item>();
            CreateMap<QuotationApprovalResource, QuotationRequestHeader>();
            CreateMap<MenuResource, Menu>();
            CreateMap<RoleMenuResource, RoleMenu>();
            //CreateMap<RoleMenuResource, Menu>();
            CreateMap<MenuResource, RoleMenu>();
            CreateMap<ChangePwResource, User>();
            CreateMap<UploadFileResource, UploadFile>();
            CreateMap<IssueHeaderResource, IssueHeader>();
            CreateMap<IssueDetailsResource, IssueDetails>();
            CreateMap<StockResource, Stock>();
            CreateMap<IssueGridResource, ItemRequest>();
            CreateMap<ItemViewResource, Item>();
            CreateMap<ItemViewResource, Stock>();
            CreateMap<BusinessUnitsResource, BusinessUnits>();
            CreateMap<BusinessUnitTypeResource, BusinessUnitType>();
            CreateMap<DesignationBusinessUnitResource, DesignationBusinessUnit>();
            CreateMap<ModuleResource, Module>();
            CreateMap<ModuleMenuResource, ModuleMenu>();
            CreateMap<GroupCompanyResource, GroupCompany>();
            CreateMap<CompanyGroupCompanyResource, CompanyGroupCompany>();
            CreateMap<CompanyGroupCompanyResource, Company>();
            CreateMap<CompanyGroupCompanyResource, GroupCompany>();




        }
    }
}
