using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Design.Web.Front.Controllers
{
    public class ReviewController : BaseController
    {
        //
        // GET: /Review/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListProductReview()
        {
            Net.Framwork.BizDac.StoreReviewBiz _reviewBiz = new Net.Framwork.BizDac.StoreReviewBiz();
            List<Net.Framework.StoreModel.StoreReviewT> _review = new List<Net.Framework.StoreModel.StoreReviewT>();

            _review = _reviewBiz.SelectAllStoreReview().Where(m => m.VisibilityYn == "Y" && m.Depth == 0).ToList<Net.Framework.StoreModel.StoreReviewT>();
           
            return PartialView(_review);
        }

        public JsonResult AddProductReview(string comment,int no = 0,int depth = 0)
        {
            bool result = false;
            string message = "";

            Net.Framwork.BizDac.StoreReviewBiz _reviewBiz = new Net.Framwork.BizDac.StoreReviewBiz();
            Net.Framework.StoreModel.StoreReviewT _review = new Net.Framework.StoreModel.StoreReviewT();

            if (no == 0)
            {
                //첫글
                try
                {
                    _review.ProductNo = 1234;
                    _review.Comment = comment;
                    _review.Score = 100;
                    _review.ImageName = "test";
                    _review.VisibilityYn = "Y";
                    _review.MemberNo = 1234;
                    _review.RegIp = "127.0.0.1";
                    _review.RegDt = DateTime.Now;
                    _review.RegId = Profile.UserId;

                    _reviewBiz.add(_review);

                    result = true;
                    message = "성공하였습니다.";
                }
                catch
                {
                    result = false;
                    message = "실패하였습니다.";
                }
            }
            else
            {
                //댓글
                try
                {
                    _review.ProductNo = 1234;
                    _review.Comment = comment;
                    _review.Score = 100;
                    _review.ImageName = "test";
                    _review.VisibilityYn = "Y";
                    _review.MemberNo = 1234;
                    _review.RegIp = "127.0.0.1";
                    _review.RegDt = DateTime.Now;
                    _review.RegId = Profile.UserId;
                    _review.Depth = depth;
                    _review.ParentNo = no;

                    _reviewBiz.add(_review);

                    result = true;
                    message = "성공하였습니다.";
                }
                catch
                {
                    result = false;
                    message = "실패하였습니다.";
                }
            }
            return Json(new { Success = result ,Message = message});
        }

        public JsonResult UpdateProductReview(int no,string comment)
        {
            bool result = false;
            string message = "";

            Net.Framwork.BizDac.StoreReviewBiz _reviewBiz = new Net.Framwork.BizDac.StoreReviewBiz();
            Net.Framework.StoreModel.StoreReviewT _review = new Net.Framework.StoreModel.StoreReviewT();

            _review = _reviewBiz.getStoreReviewById(no);
            if (_review != null)
            {
                _review.Comment = comment;
                _reviewBiz.upd(_review);

                result = true;
                message = "성공하였습니다.";
            }
            else
            {
                result = false;
                message = "실패하였습니다.";
            }

            return Json(new { Success = result, Message = message });
        }

        public JsonResult DeleteProductReview(int no)
        {
            bool result = false;
            string message = "";

            Net.Framwork.BizDac.StoreReviewBiz _reviewBiz = new Net.Framwork.BizDac.StoreReviewBiz();
            Net.Framework.StoreModel.StoreReviewT _review = new Net.Framework.StoreModel.StoreReviewT();

            _review = _reviewBiz.getStoreReviewById(no);
            if (_review != null)
            {
                _review.VisibilityYn = "N";
                _reviewBiz.upd(_review);

                result = true;
                message = "성공하였습니다.";
            }
            else
            {
                result = false;
                message = "실패하였습니다.";
            }

            return Json(new { Success = result, Message = message });
        }

        public ActionResult ViewProductReviewDepth(string no)
        {
            Net.Framwork.BizDac.StoreReviewBiz _reviewBiz = new Net.Framwork.BizDac.StoreReviewBiz();
            List<Net.Framework.StoreModel.StoreReviewT> _review = new List<Net.Framework.StoreModel.StoreReviewT>();

            _review = _reviewBiz.getStoreReviewTByParentId(Convert.ToInt32(no));

            return PartialView(_review);
        }
    }
}
