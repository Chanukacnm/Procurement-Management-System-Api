using AutoMapper;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcMgt_Reference.Mapping
{
    public class ModelToResourceProfile: Profile
    {
        public ModelToResourceProfile()
        { 
            CreateMap<Category, CategoryMasterResource>();
            CreateMap<ItemType, ItemTypeMasterResource>();
            CreateMap<ApprovalPatternType, ApprovalPatternTypeResource>();
            CreateMap<Department, DepartmentResource>();
            CreateMap<MeasurementUnits, MeasurementUnitResource>();
            CreateMap<PaymentMethod, PaymentMethodResource>();
            CreateMap<UserRole, UserRoleResource>();
            CreateMap<Company,CompanyResource>();
            CreateMap<Supplier, SupplierResource>();
            CreateMap<Model, ModelResource>();
            CreateMap<Bank,BankResource>();
            CreateMap<BankBranch, BankBranchResource>();
            CreateMap<AccountType, AccoutTypeResource>();
            CreateMap<SupplierRegisteredItems, SupplierRegisteredItemsResource>();
            CreateMap<MinimumCapacity, MinimumCapacityResource>();
            CreateMap<SupplierType, SupplierTypeResources>();
            CreateMap<Tax, TaxResource>();
            //CreateMap<ItemCategory, ItemCategoryResource>();
            //CreateMap<ReOrderLevel, ReOrderLevelResource>();
            CreateMap<Make, MakeResource>();
            CreateMap<ContactDetails, ContactDetailsResource>();
            CreateMap<Priority, PriorityResource>();
            CreateMap<Approver, ApproverResource>();
            CreateMap<ItemRequest, ItemRequestResource>();
            CreateMap<ApprovalFlowManagement, ApprovalFlowManagementResource>();
            CreateMap<User, UserResource>();
            CreateMap<Designation, DesignationResource>();
            CreateMap<ItemRequest, ApprovalScreenResource>();
            CreateMap<User, MenuResource>();
            CreateMap<User, RoleMenuResource>();
            CreateMap<User, AuthenticatorResource>();
            CreateMap<Menu, AuthenticatorResource>();
            CreateMap<Arndetail,ArndetailResource>();
            CreateMap<Arnheader, ArnheaderResource>();
            CreateMap<Item, ItemResource>();
            CreateMap<Poheader, PoHeaderResource>();
            CreateMap<Podetail, PoDetailResource>();
            CreateMap<QuotationRequestHeader, QuotationRequestHeaderResource>();
            CreateMap<QuotationRequestDetails, QuotationRequestDetailsResource>();
            CreateMap<Poheader, ItemResource>();
            CreateMap<Podetail, ItemResource>();
            CreateMap<QuotationRequestHeader, QuotationApprovalResource>();
            CreateMap<Menu, MenuResource>();
            CreateMap<RoleMenu, RoleMenuResource>();
            CreateMap<Menu, RoleMenuResource>();
            //CreateMap<RoleMenu, MenuResource>();
            CreateMap<User, ChangePwResource>();
            CreateMap<UploadFile, UploadFileResource>();
            CreateMap<IssueHeader, IssueHeaderResource>();
            CreateMap<IssueDetails, IssueDetailsResource>();
            CreateMap<Stock, StockResource>();
            CreateMap<ItemRequest, IssueGridResource>();
            CreateMap<Item, ItemViewResource>();
            CreateMap<Stock, ItemViewResource>();
            CreateMap<BusinessUnits, BusinessUnitsResource>();
            CreateMap<BusinessUnitType, BusinessUnitTypeResource>();
            CreateMap<DesignationBusinessUnit, DesignationBusinessUnitResource>();
            CreateMap<Model, ModelResource>();
            CreateMap<ModuleMenu, ModuleMenuResource>();
            CreateMap<Menu, ModuleMenuResource>();
            CreateMap<Module, ModuleMenuResource>(); 
            CreateMap<GroupCompany, GroupCompanyResource>(); 
            CreateMap<CompanyGroupCompany, CompanyGroupCompanyResource>(); 
            CreateMap<CompanyGroupCompany, CompanyResource>(); 
            CreateMap<CompanyGroupCompany, GroupCompanyResource>();





        }
    }

  
}



