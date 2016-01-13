using Makers.Admin.Models;
using Makersn.Util;
using Net.Framework.StoreModel;
using Net.Framwork.BizDac;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Makers.Admin.Controllers
{
    public class StoreController : BaseController
    {
        private MenuModel menuModel = new MenuModel();
        public MenuModel MenuModel(int subIndex)
        {
            menuModel.Group = "_Store";
            menuModel.MainIndex = 4;
            menuModel.SubIndex = subIndex;
            return menuModel;
        }

        /// <summary>
        /// Store Managing Default Page
        /// This is a Order Listing Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(0);
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult ProductCertificateList(string query = "", int status = (int)MakersnEnumTypes.ProductCertificateStatus.Request, int page = 1)
        {

            if(Profile.UserLevel < 50) { return Redirect("/account/logon"); }
            ViewData["Group"] = MenuModel(1);

            List<StoreProductT> productList = new StoreProductBiz().searchProductWithCertification(status, query);
            ViewBag.ProductList = productList;

            ViewData["certificateType"] = status;
            ViewData["query"] = query;
            ViewData["cnt"] = productList.Count;

            return View(productList.OrderByDescending(p => p.RegDt).ToPagedList(page, 20));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="no"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JsonResult ChangeCertiFicateStatus(int no,int status) {
            StoreProductT storeProduct = new StoreProductBiz().getStoreProductById(no);
            storeProduct.CertiFicateStatus = status;
            new StoreProductBiz().setStoreProduct(storeProduct);
            return Json(new { result = 1 });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult PrinterList(string query = "",  int page = 1)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }
            ViewData["Group"] = MenuModel(2);

            IList<StorePrinterT> printerList = new StorePrinterBiz().getSearchedStorePrinter(query);
            ViewData["query"] = query;
            ViewData["cnt"] = printerList.Count;

            return View(printerList.OrderByDescending(p => p.RegDt).ToPagedList(page, 20));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize, HttpGet]
        public ActionResult PrinterInsert()
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }
            ViewData["Group"] = MenuModel(2);
            IList<StoreMaterialT> matList = new StoreMaterialBiz().getAllStoreMaterial();
            ViewBag.matList = matList;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PrinterName"></param>
        /// <param name="PrintingCompanyNo"></param>
        /// <param name="SizeX"></param>
        /// <param name="SizeY"></param>
        /// <param name="SizeZ"></param>
        /// <param name="matNoInfo"></param>
        /// <param name="matUseInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PrinterInsert(string PrinterName, int PrintingCompanyNo, int SizeX, int SizeY, int SizeZ, string matNoInfo, string matUseInfo)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }
            ViewData["Group"] = MenuModel(2);
            StorePrinterT storePrinter = new StorePrinterT();
            storePrinter.PrinterName = PrinterName;
            storePrinter.SizeX = SizeX;
            storePrinter.SizeY = SizeY;
            storePrinter.SizeZ = SizeZ;
            storePrinter.PrintingCompanyNo = PrintingCompanyNo;
            storePrinter.RegId = Profile.UserId;
            storePrinter.RegDt= DateTime.Now;

            int printerNo = new StorePrinterBiz().add(storePrinter);

            StorePrinterMaterialBiz materialBiz = new StorePrinterMaterialBiz();
            string[] matNoList = matNoInfo.Split(';');
            string[] matUseList = matUseInfo.Split(';');
            for (int i = 0; i < matNoList.Length; i++) { 
                if(matUseList[i].Equals("Y")){
                    StorePrinterMaterialT storePrinterMat = new StorePrinterMaterialT();
                    storePrinterMat.PrinterNo = printerNo;
                    storePrinterMat.MaterialNo = Convert.ToInt32(matNoList[i]);
                    storePrinterMat.RegDt = DateTime.Now;
                    storePrinterMat.RegId = Profile.UserId;
                    storePrinterMat.UpdDt = DateTime.Now;
                    storePrinterMat.UpdId = Profile.UserId;
                    materialBiz.add(storePrinterMat);
                }
            }

            return Redirect("PrinterList");
        }

    }
}
