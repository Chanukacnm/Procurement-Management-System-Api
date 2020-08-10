using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
    public class IssueGridResource
    {
        public Guid ItemRequestID { get; set; }


        public string RequestTitle { get; set; }
        public string ApprovedUserName { get; set; }
        public string ApprovalComment { get; set; }
        public double BalancedQty { get; set; }
        public double ReceivedQty { get; set; }
        public int NoOfUnits { get; set; }
        public string ItemDescription { get; set; }
        public string ItemTypeName { get; set; }
        public string CategoryName { get; set; }
        
        
        
        
        public DateTime RequestedDateTime { get; set; }
        public DateTime? ApprovedDateTime { get; set; }
        public DateTime RequiredDate { get; set; }

        public string MakeName { get; set; }
        public string ModelName { get; set; }

        public string RequestedUserName { get; set; }
        public string Status { get; set; }
        
        
        public string DepartmentName { get; set; }
        
        

        public Guid CategoryID { get; set; }
        public Guid ItemID { get; set; }
        public Guid? MakeID { get; set; }
        public Guid PriorityID { get; set; }
        public bool IsReplaceble { get; set; }
        public string AssetCode { get; set; }
        public string Remark { get; set; }
        public Guid ItemTypeID { get; set; }
        public Guid? ModelID { get; set; }
        
        
        public Guid? ApproverID { get; set; }
        public string Attachment { get; set; }
        public bool IsApproved { get; set; }
        public bool IsIssued { get; set; }

        public bool IsRejected { get; set; }
        
        public Guid RequestedUserID { get; set; }
        public Guid DepartmentID { get; set; }
        public DateTime? UpdatedRequestedDateTime { get; set; }
        public Guid? ApprovedUserID { get; set; }

        
        public string PriorityLevelName { get; set; }
        
        public string ApproverName { get; set; }
        
        public string TotalItemRequests { get; set; }
        public string TotalApprovedItemRequests { get; set; }
        public string TotalRejectedItemRequests { get; set; }
        public string TotalPendingItemRequests { get; set; }
    }
}
