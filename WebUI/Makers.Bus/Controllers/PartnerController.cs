using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Common.Func;
using Net.Common.Filter;
using Net.Common.Define;
using Net.Common.Helper;
using Net.Common.Model;
using Net.Framework.Entity;
using Net.Framework.BizDac;

namespace Makers.Bus.Controllers
{
    public class PartnerController : BaseController
    {
        //
        // GET: /Partner/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Qna()
        {
            return View();
        }

        [HttpPost]
        public void AddMakerBusPartnershipQna(string email, string title, string contents)
        {
            BusManageBiz biz = new BusManageBiz();
            BusPartnershipQnaT data = new BusPartnershipQnaT();

            data.EMAIL = email;
            data.TITLE = title;
            data.CONTENTS = contents;
            data.REG_DT = DateTime.Now;
            data.REG_ID = email;

            Int64 result = biz.AddMakerBusPartnershipQna(data);

            if (result > 0)
            {
                Response.Write("<script type='text/javascript'>alert('정상적으로 파트너쉽 문의사항을 보냈습니다.')</script>");
                Response.Redirect("Qna");
            }
        }

    }
}
