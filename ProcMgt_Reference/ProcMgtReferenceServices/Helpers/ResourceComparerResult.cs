using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Services.Helpers
{
    public class ResourceComparerResult<T> where T : class
    {
        public bool Updated { get; protected set; }
        public bool Deleted { get; protected set; }
        public T Obj { get; protected set; }

        public ResourceComparerResult(T obj, bool updated)
        {
            Obj = obj;
            Updated = updated;
        }

    }
}
