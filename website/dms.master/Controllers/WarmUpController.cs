using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dms.Master.Controllers
{
    public class WarmUpController : Controller
    {
        [HttpGet]
        public string Index()
        {
            return "ok";
        }
    }
}