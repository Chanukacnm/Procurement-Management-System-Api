using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ProcMgt_Reference_Core.Models;

namespace ProcMgt_Reference_Core.Resources
{
    public class DataGridTable
    {
        public string rowSelection { get; set; }
        public bool enableSorting { get; set; }
        public bool enableFilter { get; set; }
        public bool enableColResize { get; set; }
        public bool suppressSizeToFit { get; set; }
       
        public List<DataGridColumn> DataGridColumns { get; set; }
        public List<object> DataGridRows { get; set; }

    }

    public class DataGridColumn
    {
        public string headerName { get; set; }
        public string field { get; set; }
        public bool hide { get; set; }
        public string type { get; set; }
        public string filter { get; set; }
        public bool editable { get; set; }
        public int width { get; set; }
        //public string cellStyle { get; set; }



        public void nameof(Guid? companyId)
        {
            throw new NotImplementedException();
        }

        public void nameof(ItemType itemType)
        {
            
            throw new NotImplementedException();
        }

      
    }

    public enum rowSelection
    {
        single,
        multiple
    }



}
