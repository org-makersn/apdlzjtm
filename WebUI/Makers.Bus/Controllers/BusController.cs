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
    public class BusController : BaseController
    {
        //
        // GET: /Bus/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Apply()
        {
            return View();
        }

        public ActionResult Qna()
        {
            return View();
        }

        public ActionResult Faq()
        {
            return View();
        }

        public PartialViewResult SubHeader()
        {
            return PartialView();
        }

        [HttpPost]
        public void AddApplyMakerBus(string schoolName, string schoolAddr, string managerName, string managerEmail, string ManagerTel, HttpPostedFileBase applyFile)
        {
            BusManageBiz biz = new BusManageBiz();
            BusApplySchoolT data = new BusApplySchoolT();
            string fileName = "";

            fileName = new UploadFunc().FileUpload(applyFile, null, "Apply", null);

            data.SCHOOL_NAME = schoolName;
            data.SCHOOL_ADDR = schoolAddr;
            data.MANAGER = managerName;
            data.MANAGER_EMAIL = managerEmail;
            data.MANAGER_TEL = ManagerTel;
            data.APPLY_PATH = fileName;
            data.REG_DT = DateTime.Now;
            data.REG_ID = managerName; //profileModel.UserId;

            int result = biz.AddApplyMakerBus(data);
            if (result > 0)
            {
                Response.Write("<script type='text/javascript'>alert('신청되었습니다.')</script>");
                Response.Redirect("Apply");
            }
            
        }

        [HttpPost]
        public void AddMakerBusQna(int category, string email, string title, string contents)
        {
            BusManageBiz biz = new BusManageBiz();
            BusQnaT data = new BusQnaT();

            data.CATEGORY = category;
            data.EMAIL = email;
            data.TITLE = title;
            data.CONTENTS = contents;
            data.REG_DT = DateTime.Now;
            data.REG_ID = email;

            Int64 result = biz.AddMakerBusQna(data);

            if (result > 0)
            {
                Response.Write("<script type='text/javascript'>alert('정상적으로 문의사항을 보냈습니다.')</script>");
                Response.Redirect("Qna");
            }
        }



    }
}
