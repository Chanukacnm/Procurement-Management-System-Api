using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ProcMgt_Reference_Core.Models;

namespace ProcMgt_Reference_Infrastructure.Models
{
    public partial class ReferenceContext : DbContext
    {
        public ReferenceContext()
        {
        }

        public ReferenceContext(DbContextOptions<ReferenceContext> options)
            : base(options)
        { 
        }

        public virtual DbSet<AccountType> AccountType { get; set; }
        public virtual DbSet<ApprovalFlowManagement> ApprovalFlowManagement { get; set; }
        public virtual DbSet<ApprovalPatternType> ApprovalPatternType { get; set; }
        public virtual DbSet<Approver> Approver { get; set; }
        public virtual DbSet<Arndetail> Arndetail { get; set; }
        public virtual DbSet<Arnheader> Arnheader { get; set; }
        public virtual DbSet<Bank> Bank { get; set; }
        public virtual DbSet<BankBranch> BankBranch { get; set; }
        public virtual DbSet<BusinessUnitType> BusinessUnitType { get; set; }
        public virtual DbSet<BusinessUnits> BusinessUnits { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<CompanyGroupCompany> CompanyGroupCompany { get; set; }
        public virtual DbSet<ContactDetails> ContactDetails { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Designation> Designation { get; set; }
        public virtual DbSet<DesignationBusinessUnit> DesignationBusinessUnit { get; set; }
        public virtual DbSet<GroupCompany> GroupCompany { get; set; }
        public virtual DbSet<IssueDetails> IssueDetails { get; set; }
        public virtual DbSet<IssueHeader> IssueHeader { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<ItemRequest> ItemRequest { get; set; }
        public virtual DbSet<ItemType> ItemType { get; set; }
        public virtual DbSet<Make> Make { get; set; }
        public virtual DbSet<MeasurementUnits> MeasurementUnits { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<MinimumCapacity> MinimumCapacity { get; set; }
        public virtual DbSet<Model> Model { get; set; }
        public virtual DbSet<Module> Module { get; set; }
        public virtual DbSet<ModuleMenu> ModuleMenu { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethod { get; set; }
        public virtual DbSet<Podetail> Podetail { get; set; }
        public virtual DbSet<Poheader> Poheader { get; set; }
        public virtual DbSet<Priority> Priority { get; set; }
        public virtual DbSet<QuotationRequestDetails> QuotationRequestDetails { get; set; }
        public virtual DbSet<QuotationRequestHeader> QuotationRequestHeader { get; set; }
        public virtual DbSet<QuotationRequestStatus> QuotationRequestStatus { get; set; }
        public virtual DbSet<RoleMenu> RoleMenu { get; set; }
        public virtual DbSet<Stock> Stock { get; set; }
        public virtual DbSet<Supplier> Supplier { get; set; }
        public virtual DbSet<SupplierRegisteredItems> SupplierRegisteredItems { get; set; }
        public virtual DbSet<SupplierType> SupplierType { get; set; }
        public virtual DbSet<Tax> Tax { get; set; }
        public virtual DbSet<UploadFile> UploadFile { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server = cicra.database.windows.net; Database= PROK_CICRA_DEV; User Id= cicra; Password= !hbs@123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<AccountType>(entity =>
            {
                entity.Property(e => e.AccountTypeId).ValueGeneratedNever();

                entity.Property(e => e.AccountTypeName).IsUnicode(false);
            });

            modelBuilder.Entity<ApprovalFlowManagement>(entity =>
            {
                entity.Property(e => e.ApprovalFlowManagementId).ValueGeneratedNever();

                entity.Property(e => e.ApprovalSequenceNo).IsUnicode(false);

                entity.HasOne(d => d.ApprovalPatternType)
                    .WithMany(p => p.ApprovalFlowManagement)
                    .HasForeignKey(d => d.ApprovalPatternTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalFlowManagement_ApprovalPatternType");

                entity.HasOne(d => d.Designation)
                    .WithMany(p => p.ApprovalFlowManagement)
                    .HasForeignKey(d => d.DesignationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalFlowManagement_Designation");
            });

            modelBuilder.Entity<ApprovalPatternType>(entity =>
            {
                entity.Property(e => e.ApprovalPatternTypeId).ValueGeneratedNever();

                entity.Property(e => e.Code).IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.PatternName).IsUnicode(false);
            });

            modelBuilder.Entity<Approver>(entity =>
            {
                entity.Property(e => e.ApproverId).ValueGeneratedNever();

                entity.Property(e => e.ApproverName).IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Approver)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Approver_Category");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Approver)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Approver_User");
            });

            modelBuilder.Entity<Arndetail>(entity =>
            {
                entity.Property(e => e.ArndetailId).ValueGeneratedNever();

                entity.Property(e => e.Remark).IsUnicode(false);

                entity.HasOne(d => d.Arnheader)
                    .WithMany(p => p.Arndetail)
                    .HasForeignKey(d => d.ArnheaderId)
                    .HasConstraintName("FK_ARNDetail_ARNHeader");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.Arndetail)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_ARNDetail_Item");
            });

            modelBuilder.Entity<Arnheader>(entity =>
            {
                entity.Property(e => e.ArnheaderId).ValueGeneratedNever();

                entity.Property(e => e.Arnnumber).IsUnicode(false);

                entity.Property(e => e.Arnremark).IsUnicode(false);

                entity.Property(e => e.InvoiceNo).IsUnicode(false);

                entity.HasOne(d => d.Poheader)
                    .WithMany(p => p.Arnheader)
                    .HasForeignKey(d => d.PoheaderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ARNHeader_POHeader");
            });

            modelBuilder.Entity<Bank>(entity =>
            {
                entity.Property(e => e.BankId).ValueGeneratedNever();

                entity.Property(e => e.BankName).IsUnicode(false);
            });

            modelBuilder.Entity<BankBranch>(entity =>
            {
                entity.HasKey(e => e.BranchId)
                    .HasName("PK_Branch");

                entity.Property(e => e.BranchId).ValueGeneratedNever();

                entity.Property(e => e.BranchName).IsUnicode(false);

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.BankBranch)
                    .HasForeignKey(d => d.BankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BankBranch_Bank");
            });

            modelBuilder.Entity<BusinessUnitType>(entity =>
            {
                entity.Property(e => e.BusinessUnitTypeId).ValueGeneratedNever();

                entity.Property(e => e.BusinessUnitTypeName).IsUnicode(false);

                entity.HasOne(d => d.Designation)
                    .WithMany(p => p.BusinessUnitType)
                    .HasForeignKey(d => d.DesignationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BusinessUnitType_Designation");
            });

            modelBuilder.Entity<BusinessUnits>(entity =>
            {
                entity.Property(e => e.BusinessUnitsId).ValueGeneratedNever();

                entity.Property(e => e.BusinessUnitsName).IsUnicode(false);

                entity.HasOne(d => d.BusinessUnitType)
                    .WithMany(p => p.BusinessUnits)
                    .HasForeignKey(d => d.BusinessUnitTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BusinessUnits_BusinessUnitType");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).ValueGeneratedNever();

                entity.Property(e => e.CategoryCode).IsUnicode(false);

                entity.Property(e => e.CategoryName).IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.CompanyId).ValueGeneratedNever();

                entity.Property(e => e.CompanyAddressLine1).IsUnicode(false);

                entity.Property(e => e.CompanyAddressLine2).IsUnicode(false);

                entity.Property(e => e.CompanyAddressLine3).IsUnicode(false);

                entity.Property(e => e.CompanyAddressLine4).IsUnicode(false);

                entity.Property(e => e.CompanyCode).IsUnicode(false);

                entity.Property(e => e.CompanyFax).IsUnicode(false);

                entity.Property(e => e.CompanyName).IsUnicode(false);

                entity.Property(e => e.CompanyRegistrationNo).IsUnicode(false);

                entity.Property(e => e.CompanyWeb).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.VatRegistrationNo).IsUnicode(false);
            });

            modelBuilder.Entity<CompanyGroupCompany>(entity =>
            {
                entity.Property(e => e.CompanyGroupCompanyId).ValueGeneratedNever();

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanyGroupCompany)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyGroupCompany_Company");

                entity.HasOne(d => d.GroupCompany)
                    .WithMany(p => p.CompanyGroupCompany)
                    .HasForeignKey(d => d.GroupCompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyGroupCompany_GroupCompany");
            });

            modelBuilder.Entity<ContactDetails>(entity =>
            {
                entity.Property(e => e.ContactDetailsId).ValueGeneratedNever();

                entity.Property(e => e.ContactName).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.IsDefault).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.ContactDetails)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContactDetails_Supplier");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(e => e.DepartmentId).ValueGeneratedNever();

                entity.Property(e => e.Code).IsUnicode(false);

                entity.Property(e => e.DepartmentName).IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Department)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_Department_Company");
            });

            modelBuilder.Entity<Designation>(entity =>
            {
                entity.Property(e => e.DesignationId).ValueGeneratedNever();

                entity.Property(e => e.BusinessUnitTypeName).IsUnicode(false);

                entity.Property(e => e.DesignationCode).IsUnicode(false);

                entity.Property(e => e.DesignationName).IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<DesignationBusinessUnit>(entity =>
            {
                entity.Property(e => e.DesignationBusinessUnitId).ValueGeneratedNever();

                entity.HasOne(d => d.BusinessUnitType)
                    .WithMany(p => p.DesignationBusinessUnit)
                    .HasForeignKey(d => d.BusinessUnitTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DesignationBusinessUnit_BusinessUnitType");

                entity.HasOne(d => d.BusinessUnits)
                    .WithMany(p => p.DesignationBusinessUnit)
                    .HasForeignKey(d => d.BusinessUnitsId)
                    .HasConstraintName("FK_DesignationBusinessUnit_BusinessUnits");

                entity.HasOne(d => d.Designation)
                    .WithMany(p => p.DesignationBusinessUnit)
                    .HasForeignKey(d => d.DesignationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DesignationBusinessUnit_Designation");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DesignationBusinessUnit)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_DesignationBusinessUnit_User");
            });

            modelBuilder.Entity<GroupCompany>(entity =>
            {
                entity.Property(e => e.GroupCompanyId).ValueGeneratedNever();

                entity.Property(e => e.GcompanyEmail).IsUnicode(false);

                entity.Property(e => e.GcompanyFax).IsUnicode(false);

                entity.Property(e => e.GcompanyRegistrationNo).IsUnicode(false);

                entity.Property(e => e.GcompanyWeb).IsUnicode(false);

                entity.Property(e => e.GroupCompanyAddressLine1).IsUnicode(false);

                entity.Property(e => e.GroupCompanyAddressLine2).IsUnicode(false);

                entity.Property(e => e.GroupCompanyAddressLine3).IsUnicode(false);

                entity.Property(e => e.GroupCompanyAddressLine4).IsUnicode(false);

                entity.Property(e => e.GroupCompanyCode).IsUnicode(false);

                entity.Property(e => e.GroupCompanyName).IsUnicode(false);

                entity.Property(e => e.VatRegistrationNo).IsUnicode(false);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.GroupCompany)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_GroupCompany_Company");
            });

            modelBuilder.Entity<IssueDetails>(entity =>
            {
                entity.Property(e => e.IssueDetailId).ValueGeneratedNever();

                entity.HasOne(d => d.IssuedHeader)
                    .WithMany(p => p.IssueDetails)
                    .HasForeignKey(d => d.IssuedHeaderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IssueDetails_IssueHeader");
            });

            modelBuilder.Entity<IssueHeader>(entity =>
            {
                entity.HasKey(e => e.IssuedHeaderId)
                    .HasName("PK_Issue");

                entity.Property(e => e.IssuedHeaderId).ValueGeneratedNever();

                entity.Property(e => e.Comment).IsUnicode(false);

                entity.HasOne(d => d.IssuedUser)
                    .WithMany(p => p.IssueHeader)
                    .HasForeignKey(d => d.IssuedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Issue_User");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(e => e.ItemId).ValueGeneratedNever();

                entity.Property(e => e.CurrentQty).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.ItemCode).IsUnicode(false);

                entity.HasOne(d => d.ItemType)
                    .WithMany(p => p.Item)
                    .HasForeignKey(d => d.ItemTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Item_Item");
            });

            modelBuilder.Entity<ItemRequest>(entity =>
            {
                entity.Property(e => e.ItemRequestId).ValueGeneratedNever();

                entity.Property(e => e.ApprovalComment).IsUnicode(false);

                entity.Property(e => e.AssetCode).IsUnicode(false);

                entity.Property(e => e.Attachment).IsUnicode(false);

                entity.Property(e => e.Remark).IsUnicode(false);

                entity.Property(e => e.RequestTitle).IsUnicode(false);

                entity.HasOne(d => d.ApprovedUser)
                    .WithMany(p => p.ItemRequestApprovedUser)
                    .HasForeignKey(d => d.ApprovedUserId)
                    .HasConstraintName("FK_ItemRequest_User1");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.ItemRequest)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemRequest_Category");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.ItemRequest)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemRequest_Department");

                entity.HasOne(d => d.IssuedUser)
                    .WithMany(p => p.ItemRequestIssuedUser)
                    .HasForeignKey(d => d.IssuedUserId)
                    .HasConstraintName("FK_ItemRequest_User2");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemRequest)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemRequest_Item");

                entity.HasOne(d => d.ItemType)
                    .WithMany(p => p.ItemRequest)
                    .HasForeignKey(d => d.ItemTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemRequest_ItemType");

                entity.HasOne(d => d.Make)
                    .WithMany(p => p.ItemRequest)
                    .HasForeignKey(d => d.MakeId)
                    .HasConstraintName("FK_ItemRequest_Make");

                entity.HasOne(d => d.Model)
                    .WithMany(p => p.ItemRequest)
                    .HasForeignKey(d => d.ModelId)
                    .HasConstraintName("FK_ItemRequest_Model");

                entity.HasOne(d => d.Priority)
                    .WithMany(p => p.ItemRequest)
                    .HasForeignKey(d => d.PriorityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemRequest_Priority");

                entity.HasOne(d => d.RequestedUser)
                    .WithMany(p => p.ItemRequestRequestedUser)
                    .HasForeignKey(d => d.RequestedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemRequest_User");
            });

            modelBuilder.Entity<ItemType>(entity =>
            {
                entity.Property(e => e.ItemTypeId).ValueGeneratedNever();

                entity.Property(e => e.DepreciationRate).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsDisposable).HasDefaultValueSql("((1))");

                entity.Property(e => e.ItemTypeCode).IsUnicode(false);

                entity.Property(e => e.ItemTypeName).IsUnicode(false);

                entity.HasOne(d => d.ApprovalPatternType)
                    .WithMany(p => p.ItemType)
                    .HasForeignKey(d => d.ApprovalPatternTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemType_ApprovalPatternType");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.ItemType)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemType_ItemCategory");

                entity.HasOne(d => d.MeasurementUnit)
                    .WithMany(p => p.ItemType)
                    .HasForeignKey(d => d.MeasurementUnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemType_MeasurementUnits");
            });

            modelBuilder.Entity<Make>(entity =>
            {
                entity.Property(e => e.MakeId).ValueGeneratedNever();

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.MakeCode).IsUnicode(false);

                entity.Property(e => e.MakeName).IsUnicode(false);

                entity.HasOne(d => d.ItemType)
                    .WithMany(p => p.Make)
                    .HasForeignKey(d => d.ItemTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Make_ItemType");
            });

            modelBuilder.Entity<MeasurementUnits>(entity =>
            {
                entity.Property(e => e.MeasurementUnitId).ValueGeneratedNever();

                entity.Property(e => e.Code).IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.MeasurementUnitName).IsUnicode(false);
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.Property(e => e.Icon).IsUnicode(false);

                entity.Property(e => e.MenuIdhtml).IsUnicode(false);

                entity.Property(e => e.MenuName).IsUnicode(false);

                entity.Property(e => e.Url).IsUnicode(false);
            });

            modelBuilder.Entity<MinimumCapacity>(entity =>
            {
                entity.Property(e => e.MinimumItemsCapacityId).ValueGeneratedNever();

                entity.Property(e => e.MinimumItemsCapacityName).IsUnicode(false);
            });

            modelBuilder.Entity<Model>(entity =>
            {
                entity.Property(e => e.ModelId).ValueGeneratedNever();

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.ModelCode).IsUnicode(false);

                entity.Property(e => e.ModelName).IsUnicode(false);

                entity.HasOne(d => d.ItemType)
                    .WithMany(p => p.Model)
                    .HasForeignKey(d => d.ItemTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Model_ItemType");

                entity.HasOne(d => d.Make)
                    .WithMany(p => p.Model)
                    .HasForeignKey(d => d.MakeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Model_Make");

                entity.HasOne(d => d.UploadFile)
                    .WithMany(p => p.Model)
                    .HasForeignKey(d => d.UploadFileId)
                    .HasConstraintName("FK_Model_UploadFile");
            });

            modelBuilder.Entity<Module>(entity =>
            {
                entity.Property(e => e.ModuleId).ValueGeneratedNever();

                entity.Property(e => e.ModuleName).IsUnicode(false);
            });

            modelBuilder.Entity<ModuleMenu>(entity =>
            {
                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.ModuleMenu)
                    .HasForeignKey(d => d.MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ModuleMenu_Menu");

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.ModuleMenu)
                    .HasForeignKey(d => d.ModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ModuleMenu_Module");
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.Property(e => e.PaymentMethodId).ValueGeneratedNever();

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.PaymentMethodCode).IsUnicode(false);

                entity.Property(e => e.PaymentMethodName).IsUnicode(false);
            });

            modelBuilder.Entity<Podetail>(entity =>
            {
                entity.Property(e => e.PodetailId).ValueGeneratedNever();

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.Podetail)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PODetail_Item");

                entity.HasOne(d => d.Poheader)
                    .WithMany(p => p.Podetail)
                    .HasForeignKey(d => d.PoheaderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PODetail_POHeader");
            });

            modelBuilder.Entity<Poheader>(entity =>
            {
                entity.Property(e => e.PoheaderId).ValueGeneratedNever();

                entity.Property(e => e.Ponumber).IsUnicode(false);

                entity.HasOne(d => d.PaymentMethod)
                    .WithMany(p => p.Poheader)
                    .HasForeignKey(d => d.PaymentMethodId)
                    .HasConstraintName("FK_POHeader_PaymentMethod");

                entity.HasOne(d => d.QuotationRequestHeader)
                    .WithMany(p => p.Poheader)
                    .HasForeignKey(d => d.QuotationRequestHeaderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PurchaseOrder_QuotationRequestHeader");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Poheader)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_POHeader_User");
            });

            modelBuilder.Entity<Priority>(entity =>
            {
                entity.Property(e => e.PriorityId).ValueGeneratedNever();

                entity.Property(e => e.PriorityLevelName).IsUnicode(false);
            });

            modelBuilder.Entity<QuotationRequestDetails>(entity =>
            {
                entity.Property(e => e.QuotationRequestDetailId).ValueGeneratedNever();

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.QuotationRequestDetails)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuotationRequestDetails_Item");

                entity.HasOne(d => d.Make)
                    .WithMany(p => p.QuotationRequestDetails)
                    .HasForeignKey(d => d.MakeId)
                    .HasConstraintName("FK_QuotationRequestDetails_Make");

                entity.HasOne(d => d.MeasurementUnit)
                    .WithMany(p => p.QuotationRequestDetails)
                    .HasForeignKey(d => d.MeasurementUnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuotationRequestDetails_MeasurementUnits");

                entity.HasOne(d => d.Model)
                    .WithMany(p => p.QuotationRequestDetails)
                    .HasForeignKey(d => d.ModelId)
                    .HasConstraintName("FK_QuotationRequestDetails_Model");

                entity.HasOne(d => d.QuotationRequestHeader)
                    .WithMany(p => p.QuotationRequestDetails)
                    .HasForeignKey(d => d.QuotationRequestHeaderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuotationRequestDetails_QuotationRequestHeader");
            });

            modelBuilder.Entity<QuotationRequestHeader>(entity =>
            {
                entity.Property(e => e.QuotationRequestHeaderId).ValueGeneratedNever();

                entity.Property(e => e.ApprovalComment).IsUnicode(false);

                entity.Property(e => e.IsEnteringCompleted).HasDefaultValueSql("((1))");

                entity.Property(e => e.QuotationNumber).IsUnicode(false);

                entity.HasOne(d => d.QuotationRequestStatus)
                    .WithMany(p => p.QuotationRequestHeader)
                    .HasForeignKey(d => d.QuotationRequestStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuotationRequestHeader_QuotationRequestStatus");
            });

            modelBuilder.Entity<QuotationRequestStatus>(entity =>
            {
                entity.Property(e => e.QuotationRequestStatusId).ValueGeneratedNever();

                entity.Property(e => e.QuotationRequestStatus1).IsUnicode(false);
            });

            modelBuilder.Entity<RoleMenu>(entity =>
            {
                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.RoleMenu)
                    .HasForeignKey(d => d.MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleMenu_Menu");

                entity.HasOne(d => d.UserRole)
                    .WithMany(p => p.RoleMenu)
                    .HasForeignKey(d => d.UserRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleMenu_UserRole");
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.Property(e => e.StockId).ValueGeneratedNever();

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.Stock)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Stock_Item");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.Property(e => e.SupplierId).ValueGeneratedNever();

                entity.Property(e => e.AccountName).IsUnicode(false);

                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.BillingAddress).IsUnicode(false);

                entity.Property(e => e.BillingName).IsUnicode(false);

                entity.Property(e => e.BrNo).IsUnicode(false);

                entity.Property(e => e.SupplierName).IsUnicode(false);

                entity.HasOne(d => d.AccountType)
                    .WithMany(p => p.Supplier)
                    .HasForeignKey(d => d.AccountTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Supplier_AccountType");

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.Supplier)
                    .HasForeignKey(d => d.BankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Supplier_Bank");

                entity.HasOne(d => d.Branch)
                    .WithMany(p => p.Supplier)
                    .HasForeignKey(d => d.BranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Supplier_BankBranch");

                entity.HasOne(d => d.PaymentMethod)
                    .WithMany(p => p.Supplier)
                    .HasForeignKey(d => d.PaymentMethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Supplier_PaymentMethod");

                entity.HasOne(d => d.SupplierType)
                    .WithMany(p => p.Supplier)
                    .HasForeignKey(d => d.SupplierTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Supplier_SupplierType");
            });

            modelBuilder.Entity<SupplierRegisteredItems>(entity =>
            {
                entity.Property(e => e.SupplierRegisteredItemsId).ValueGeneratedNever();

                entity.Property(e => e.MinimumItemCapacity).IsUnicode(false);

                entity.HasOne(d => d.ItemType)
                    .WithMany(p => p.SupplierRegisteredItems)
                    .HasForeignKey(d => d.ItemTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SupplierRegisteredItems_ItemType");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.SupplierRegisteredItems)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SupplierRegisteredItems_Supplier");
            });

            modelBuilder.Entity<SupplierType>(entity =>
            {
                entity.Property(e => e.SupplierTypeId).ValueGeneratedNever();

                entity.Property(e => e.SupplierTypeName).IsUnicode(false);
            });

            modelBuilder.Entity<Tax>(entity =>
            {
                entity.Property(e => e.TaxId).ValueGeneratedNever();

                entity.Property(e => e.TaxCode).IsUnicode(false);

                entity.Property(e => e.TaxName).IsUnicode(false);
            });

            modelBuilder.Entity<UploadFile>(entity =>
            {
                entity.Property(e => e.UploadFileId).ValueGeneratedNever();

                entity.Property(e => e.FileExtension).IsUnicode(false);

                entity.Property(e => e.UploadFileName).IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.EmployeeNo).IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.UserName).IsUnicode(false);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Department");

                entity.HasOne(d => d.Designation)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.DesignationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Designation");

                entity.HasOne(d => d.UserRole)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.UserRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_UserRole");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.Property(e => e.UserRoleId).ValueGeneratedNever();

                entity.Property(e => e.Code).IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.UserRoleName).IsUnicode(false);
            });
        }
    }
}
