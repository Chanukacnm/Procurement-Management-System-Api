using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Services.Communication
{
    public class GenericSaveResponse<T> : BaseResponse where T : class
    {
        public T Obj { get; private set; }
        public GenericSaveResponse(bool success, string message, T obj) : base(success, message)
        {
            Obj = obj;
        }

        public GenericSaveResponse(T obj) : this(true, string.Empty, obj)
        { }

        public GenericSaveResponse(string message) : this(false, message, null)
        { }
    }
}
