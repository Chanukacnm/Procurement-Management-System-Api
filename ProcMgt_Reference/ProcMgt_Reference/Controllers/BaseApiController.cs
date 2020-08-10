using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

using System.Web.Http;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProcMgt_Reference.Controllers
{
    public class BaseApiController : ControllerBase
    {
       
        public string LogError(Exception ex)
        {
            return "";
        }
    }
}
