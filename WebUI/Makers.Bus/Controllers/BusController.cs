using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
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
            BusApplySchoolT busApplySchoolT = new BusApplySchoolT();

            string fileDir = Constant.StoreUploadDir.ModelingDir;
            string savePath = string.Format(@"{0}\{1}", instance.PhysicalDir, fileDir);
            string extension = Path.GetExtension(applyFile.FileName);

            busApplySchoolT.SCHOOL_NAME = schoolName;
            busApplySchoolT.SCHOOL_ADDR = schoolAddr;
            busApplySchoolT.MANAGER = managerName;
            busApplySchoolT.MANAGER_EMAIL = managerEmail;
            busApplySchoolT.MANAGER_TEL = ManagerTel;
            busApplySchoolT.APPLY_PATH = savePath;
            busApplySchoolT.REG_DT = DateTime.Now;
            busApplySchoolT.REG_ID = profileModel.UserId;

            biz.AddApplyMakerBus(busApplySchoolT);


        }

    }
}
