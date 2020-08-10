using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
    public class QuotationRequestDetailsResource
    {
        public string itemDescription { get; set; }
        public string measurementUnitName { get; set; }
        public string makeName { get; set; }
        public string modelName { get; set; }


        public Guid QuotationRequestDetailID { get; set; }
        public Guid QuotationRequestHeaderID { get; set; }
        public Guid? MakeID { get; set; }
        public Guid? ModelID { get; set; }
        public Guid ItemID { get; set; }
        public Guid MeasurementUnitID { get; set; }
        public double? Quantity { get; set; }
        public DateTime? QuotationValidDate { get; set; }
        public decimal? UnitPrice { get; set; }        
        public decimal? GrossAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? NetAmount { get; set; }
              
        public string Attachment { get; set; }

         
       
        
        

        
    }
}
