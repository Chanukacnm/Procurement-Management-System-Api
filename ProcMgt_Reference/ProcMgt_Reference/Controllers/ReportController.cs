using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Services.Interfaces;
using System.IO;
using Microsoft.AspNetCore.Hosting.Server;
//using AspNetCore.Reporting;



namespace ProcMgt_Reference.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {

        private readonly IItemRequestServices _itemRequestServices;
        private readonly IReportServices _reportServices;

        private readonly IMapper _mapper;

        public ReportController(IItemRequestServices itemRequestServices, IReportServices reportServices, IMapper mapper)
        {
            this._itemRequestServices = itemRequestServices;
            this._reportServices = reportServices;
            this._mapper = mapper;
        }


        //[HttpGet, Route("RenderReport")]
        //public async Task<FileStream> RenderReport(string fileName)
        //{
        //    try
        //    {
        //        //var httpClientHandler = new HttpClientHandler()
        //        //{
        //        //    Credentials = new System.Net.NetworkCredential("userName", "Password", "Domain"),
        //        //};

        //        //var httpClient = new HttpClient(httpClientHandler);
        //        //httpClient.DefaultRequestHeaders.Accept.Clear();
        //        //httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        //        ////var result = await httpClient.GetStringAsync("http://lp003/ReportServer/?/testRpt2&param1=123654&rs:Command=Render&rs:Format=PDF");

        //        ////var url = "http://lp003/ReportServer/?/testRpt2&param1=123654&rs:Command=Render&rs:Format=PDF";
        //        ////using (var client = new HttpClient())
        //        ////{

        //        //    using (var result = await httpClient.GetAsync("http://lp003/ReportServer/?/testRpt2&param1=123654&rs:Command=Render&rs:Format=PDF")) //; await client.GetAsync(url))
        //        //    {
        //        //        if (result.IsSuccessStatusCode)
        //        //        {
        //        //            return await result.Content.ReadAsByteArrayAsync();
        //        //        }

        //        //    }
        //        ////}
        //        //return null;




        //        //var id = 111;
        //        //ReportWriter reportWriter = new ReportWriter();
        //        //reportWriter.ReportPath = "E:\\Chathura\\WebITPro\\ProcurementMgtServices\\ProcMgt_Reference\\ProcMgt_Reference\\testRpt2.rdl";
        //        //reportWriter.ReportProcessingMode = Syncfusion.ReportWriter.ProcessingMode.Remote;
        //        //List<Syncfusion.Report.ReportParameter> parameters = new List<Syncfusion.Report.ReportParameter>();
        //        //parameters.Add(new Syncfusion.Report.ReportParameter() { Name = "param1" });
        //        //reportWriter.SetParameters(parameters);
        //        //var format = WriterFormat.PDF;
        //        //MemoryStream ms = new MemoryStream();
        //        ////To save the report as memory stream
        //        //reportWriter.Save(ms, format);
        //        ////Converts memory stream into base64 string
        //        //string base64 = "data:application/pdf;base64," + Convert.ToBase64String(ms.ToArray());
        //        //var json = new { data = base64 };
        //        //return base64;

        //        //**************************************************************//
        //        string basePath = "E:\\Chathura\\WebITPro\\ProcurementMgtServices\\ProcMgt_Reference\\ProcMgt_Reference\\testRpt3.rdl";
        //        // Here, we have loaded the sample report report from application the folder wwwroot. 
        //        // Invoice.rdl should be there in wwwroot application folder.
        //        //FileStream inputStream = new FileStream(basePath, FileMode.Open, FileAccess.Read);
        //        //Syncfusion.ReportWriter.ReportWriter writer = new Syncfusion.ReportWriter.ReportWriter(inputStream);

        //        //string fileNamee = null;
        //        //Syncfusion.ReportWriter.WriterFormat format;

        //        //FileStream fss = new FileStream("E:\\Chathura\\WebITPro\\ProcurementMgtServices\\ProcMgt_Reference\\ProcMgt_Reference\\testRpt3.rdl", FileMode.Open, FileAccess.Read);

        //        //Syncfusion.ReportWriter.ReportWriter reportWriter = new Syncfusion.ReportWriter.ReportWriter(fss);
        //        ////reportWriter.ReportPath = basePath;
        //        //reportWriter.ReportProcessingMode = Syncfusion.ReportWriter.ProcessingMode.Local;

        //        //fileName = "test.pdf";
        //        //format = Syncfusion.ReportWriter.WriterFormat.PDF;

        //        //FileStream fs = new FileStream("E:\\Chathura\\WebITPro\\ProcurementMgtServices\\ProcMgt_Reference\\ProcMgt_Reference\\test.pdf", FileMode.CreateNew, FileAccess.ReadWrite);
        //        //reportWriter.Save(fs, Syncfusion.ReportWriter.WriterFormat.PDF);



        //        List<object> _dataSourceList = new List<object>();
        //        string _dataSourceName = "DS2";

        //        System.Data.SqlClient.SqlConnection sq = new System.Data.SqlClient.SqlConnection();
        //        sq.ConnectionString = "data source=lp003;initial catalog=EBET_OLD;user id=sa;password=sql2017@@@;";


        //        _dataSourceList.Add(sq);

        //        string mimtype = "application/pdf";
        //        int extension = 1;
        //        Dictionary<string, string> Parameters = new Dictionary<string, string>();

        //        IReportServerCredentials rsc;


        //        LocalReport localReport = new LocalReport(basePath);
        //        localReport.AddDataSource(_dataSourceName, _dataSourceList);

        //        //if (Parameters != null && Parameters.Count > 0)// if you use parameter in report
        //        //{
        //        //    List<ReportParameter> reportparameter = new List<ReportParameter>();
        //        //    foreach (var record in Parameters)
        //        //    {
        //        //        reportparameter.Add(new ReportParameter());
        //        //    }

        //        //}
        //        var result = localReport.Execute(RenderType.Image, extension, Parameters);    /// **** Mime types

        //        byte[] bytes = result.MainStream;

        //        FileStream fs = new FileStream("E:\\Chathura\\WebITPro\\ProcurementMgtServices\\ProcMgt_Reference\\ProcMgt_Reference\\test.pdf", FileMode.CreateNew);

        //        fs.Write(bytes, 0, bytes.Length);

        //        fs.Close();


        //        // Assigning the report parameter based on selected value from user.
        //        //string invoiceID = this.HttpContext.Request.Form["invoiceId"].ToString();

        //        //List<Syncfusion.Report.ReportParameter> parameters = new List<Syncfusion.Report.ReportParameter>();
        //        //Syncfusion.Report.ReportParameter param = new Syncfusion.Report.ReportParameter();
        //        //param.Name = "param1";
        //        //param.Values = new List<string>() { "123" };
        //        //parameters.Add(param);
        //        //writer.SetParameters(parameters);

        //        // Steps to generate PDF report using Report Writer.
        //        //MemoryStream memoryStream = new MemoryStream();
        //        //writer.Save(memoryStream, Syncfusion.ReportWriter.WriterFormat.PDF);

        //        // Download the generated from client.
        //        //FileStream fs = new FileStream("E:\\Chathura\\WebITPro\\ProcurementMgtServices\\ProcMgt_Reference\\ProcMgt_Reference\\test.pdf",FileMode.CreateNew,FileAccess.ReadWrite);

        //        //memoryStream.Position = 0;
        //        //memoryStream.CopyTo(fs);
        //        //FileStreamResult fileStreamResult = new FileStreamResult(memoryStream, "application/pdf");
        //        //fileStreamResult.FileDownloadName = "Invoice.pdf";
        //        //return fileStreamResult;


        //        //**************************************************************//

        //        return new FileStream("E:\\Chathura\\WebITPro\\ProcurementMgtServices\\ProcMgt_Reference\\ProcMgt_Reference\\test.pdf", FileMode.Open, FileAccess.Read); ;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //}

        // Reporting

        //  rajitha----------------------------------------------------------
        [HttpPost, Route("GetAllSuppliers")]
        public async Task<IEnumerable<SupplierResource>> GetAllSuppliers()
        {
           
            var suppliername = await _reportServices.GetAllSuppliersAsync();

            var resources = _mapper.Map<IEnumerable<Supplier>, IEnumerable<SupplierResource>>(suppliername);

            return resources;
        }

        //[HttpPost, Route("GetSuppliersPONumbers")]
        //public async Task<IEnumerable<QuotationRequestHeaderResource>> GetSuppliersPONumbers()
        //{

        //    var supplierpo = await _reportServices.GetPONumbersAsync();

        //    var resources = _mapper.Map<IEnumerable<QuotationRequestHeader>, IEnumerable<QuotationRequestHeaderResource>>(supplierpo);

        //    return resources;
        //}


       //end-------------------------------------------------------------------------

        [HttpGet, Route("GetDataRequestHistoryRpt")]
        public async Task<dynamic> GetDataRequestHistoryRpt()
        {
            var itemRequest = await _itemRequestServices.GetAllAsync();
            //var itemRequest = await _reportServices.GetItemReqData();
            var resources = _mapper.Map<IEnumerable<ItemRequest>, IEnumerable<ItemRequestResource>>(itemRequest);

            return resources;

        }



        [HttpPost, Route("GetDataRequestHistoryReport")]
        public async Task<dynamic> GetDataRequestHistoryReport( [FromBody]ReportResource resource)
        {
           // var fromDate = resource.FromDate.ToString();
            //var toDate = resource.ToDate.ToString();
            
            var itemRequest = await _reportServices.GetItemReqData(resource.FromDate, resource.ToDate);
            var resources = _mapper.Map<IEnumerable<ItemRequestResource>, IEnumerable<ItemRequestResource>>(itemRequest);

            return resources;
        }


        [HttpPost, Route("GetDataApprovedHistoryReport")]
        public async Task<dynamic> GetDataApprovedHistoryReport([FromBody]ReportResource resource)
        {
            // var fromDate = resource.FromDate.ToString();
            //var toDate = resource.ToDate.ToString();

            var ApprroveditemRequest = await _reportServices.GetApprovedItemReqData(resource.FromDate, resource.ToDate);
            var resources = _mapper.Map<IEnumerable<ItemRequestResource>, IEnumerable<ItemRequestResource>>(ApprroveditemRequest);

            return resources;
        }

        [HttpPost, Route("GetDataGRNdHistoryReport")]
        public async Task<dynamic> GetDataGRNdHistoryReport([FromBody]ReportResource resource)
        {
            
            var grnData = await _reportServices.GetGRNData(resource.FromDate, resource.ToDate);
            var resources = _mapper.Map<IEnumerable<ArnheaderResource>, IEnumerable<ArnheaderResource>>(grnData);

            return resources;
        }

        [HttpPost, Route("GetDataReconciliationHistoryReport")]
        public async Task<dynamic> GetDataReconciliationHistoryReport([FromBody]ReportResource resource)
        {

            var recoData = await _reportServices.GetReconciliationData(resource.FromDate, resource.ToDate);

            //try
            //{
            //    var resources = _mapper.Map<IEnumerable<IGrouping<Guid, ItemRequestResource>>, IEnumerable<IGrouping<Guid, ItemRequestResource>>>(recoData);
            //    return resources;
            //}
            //catch(Exception ex)
            //{
            //    return null;
            //}

            return recoData;
           
        }
    }
}