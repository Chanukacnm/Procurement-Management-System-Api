using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProcMgt_Reference_Core;
using ProcMgt_Reference_Core.GenericRepoInter;
using ProcMgt_Reference_Infrastructure;
using ProcMgt_Reference_Infrastructure.Models;
using ProcMgt_Reference_Services;
using ProcMgt_Reference_Services.Implementations;
using ProcMgt_Reference_Services.Interfaces;
using Microsoft.EntityFrameworkCore.Proxies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ProcMgt_Reference
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ResloveServiceTypes(services);


            //services.AddDbContext(options = &gt; options.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("Data Source=203.143.42.105,1443;Initial Catalog=EbetOutletTestDev;User ID=sa;Password=ebetOLD@123")));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = "http://localhost:5000",
                        ValidAudience = "http://localhost:5000",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"))
                    };
                });

            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials().Build();
                });
            });

            //configuring efcore proxies
            //temporarily used
            services.AddDbContext<DbContext>(options =>
             options.UseLazyLoadingProxies()
            .UseSqlServer(Configuration.GetConnectionString("Data Source=203.143.42.105,1443;Initial Catalog=EbetOutletTestDev;User ID=sa;Password=ebetOLD@123")));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            services.AddAutoMapper(typeof(Startup));
        }
    
        private void ResloveServiceTypes(IServiceCollection services)
        {
            services.AddScoped(typeof(IUnitOfWorks), typeof(UnitOfWork));
            services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepository<>));

            services.AddScoped(typeof(IAuthServices), typeof(AuthService));
            services.AddScoped(typeof(ICategoryMasterServices), typeof(CategoryMasterService));
            services.AddScoped(typeof(IItemTypeMasterServices), typeof(ItemTypeMasterService));
            services.AddScoped(typeof(IApprovalPatternTypeServices), typeof(ApprovalPatternTypeService));
            services.AddScoped(typeof(IDepartmentServices), typeof(DepartmentService));
            services.AddScoped(typeof(IMeasurementUnitServices), typeof(MeasurementUnitsService));
            services.AddScoped(typeof(IPaymentMethodServices), typeof(PaymentMethodService));
            services.AddScoped(typeof(IUserRoleServices), typeof(UserRoleService));
            services.AddScoped(typeof(ICompanyServices), typeof(CompanyService));
            services.AddScoped(typeof(ISupplierServices), typeof(SupplierService));
            services.AddScoped(typeof(IModelServices), typeof(ModelService));
            services.AddScoped(typeof(IBankServices), typeof(BankService));
            services.AddScoped(typeof(IBankBranchServices), typeof(BankBranchService));
            services.AddScoped(typeof(IAccountTypeServices), typeof(AccountTypeService));
            services.AddScoped(typeof(ISupplierRegisteredItemsServices), typeof(SupplierRegisteredItemsService));
            services.AddScoped(typeof(IMinimumCapacityServices), typeof(MinimumCapacityService));
            services.AddScoped(typeof(ISupplierTypeService), typeof(SupplierTypeService));
            services.AddScoped(typeof(ITaxServices), typeof(TaxService));
            //services.AddScoped(typeof(IItemCategoryServices), typeof(ItemCategoryService));
            //services.AddScoped(typeof(IReOrderLevelServices), typeof(ReOrderLevelService));
            services.AddScoped(typeof(IMakeServices), typeof(MakeService));
            services.AddScoped(typeof(IConatcDetailsServices), typeof(ContactDetailsService));
            services.AddScoped(typeof(IPriorityServices), typeof(PriorityService));
            services.AddScoped(typeof(IApproverServices), typeof(ApproverService));
            services.AddScoped(typeof(IApprovalFlowManagementServices), typeof(ApprovalFlowManagementService));
            services.AddScoped(typeof(IItemRequestServices), typeof(ItemRequestService));
            services.AddScoped(typeof(IUserServices), typeof(UserServices));
            services.AddScoped(typeof(IDesignationServices), typeof(DesignationService));
            services.AddScoped(typeof(IApprovalScreenServices), typeof(ApprovalScreenService));
            services.AddScoped(typeof(IItemServices), typeof(ItemService));
            services.AddScoped(typeof(IArnEntryServices), typeof(ArnEntryService));
            services.AddScoped(typeof(IPurchaseOrderServices), typeof(PurchaseOrderService));
            services.AddScoped(typeof(IQuotationRequestHeaderServices), typeof(QuotationRequestHeaderService));
            services.AddScoped(typeof(IQuotationRequestDetailsServices), typeof(QuotationRequestDetailsService));
            services.AddScoped(typeof(IQuotationEnterServices), typeof(QuotationEnterService));
            services.AddScoped(typeof(IQuotationApprovalServices), typeof(QuotationApprovalService));
            services.AddScoped(typeof(IMenuServices), typeof(MenuService));
            services.AddScoped(typeof(IRoleMenuServices), typeof(RoleMenuService));
            services.AddScoped(typeof(IUploadFileServices), typeof(UploadFileService));
            services.AddScoped(typeof(IReportServices), typeof(ReportService));
            services.AddScoped(typeof(IIssueServices), typeof(IssueService));
            services.AddScoped(typeof(IBusinessUnitTypeService), typeof(BusinessUnitTypeService));
            services.AddScoped(typeof(IBusinessUnitsService), typeof(BusinessUnitsService));
            services.AddScoped<ReferenceContext>();









        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseCors("EnableCORS");



            //app.UseStaticFiles();
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
            //    RequestPath = new PathString("/Resources")
            //});



            app.UseMvc();
        }
    }
}
