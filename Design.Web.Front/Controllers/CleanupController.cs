using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Makersn.BizDac;
using Makersn.Models;
using Makersn.Util;
using Design.Web.Front.Models;

namespace Design.Web.Front.Controllers
{
    [Authorize]
    public class CleanupController : BaseController
    {
        private long TotalSize = 0;
        /// <summary>
        /// 호스팅 공간 정리
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Index()
        {
            AjaxResponseModel response = new AjaxResponseModel();
            response.Success = false;

            if (Profile.UserLevel < 50 )
            {
                return Json(response);
            }

            IList<ArticleFileT> list = new ArticleFileDac().GetArticleFileTargetClean();

            foreach (var file in list)
            {
                switch (file.FileType)
                {
                    case "img":
                        CleanImg(file.Rename);
                        break;
                    case "stl":
                        Clean3D(file.Rename);
                        break;
                }
                CleanBackup(file.Name);
            }

            float m = float.Parse(TotalSize.ToString()) / (float)1024;
            float size = float.Parse(m.ToString("F1"));

            response.Success = true;
            response.Message = size + "MB를 정리하였습니다.";
            return Json(response);
        }

        internal void Clean3D(string fileNm)
        {
            string save3DFolder = "Article/article_3d";
            string saveJSFolder = "Article/article_js";
            string saveIMGFolder = "Article/article_img";

            string file3DPath = string.Format("{0}/FileUpload/{1}/{2}", AppDomain.CurrentDomain.BaseDirectory, save3DFolder, fileNm);
            string fileJsPath = string.Format("{0}/FileUpload/{1}/{2}", AppDomain.CurrentDomain.BaseDirectory, saveJSFolder, fileNm + ".js");
            string fileImgPath = string.Format("{0}/FileUpload/{1}/{2}", AppDomain.CurrentDomain.BaseDirectory, saveIMGFolder, fileNm + ".jpg");
            string fileThumbImgPath = string.Format("{0}/FileUpload/{1}/thumb/{2}", AppDomain.CurrentDomain.BaseDirectory, saveIMGFolder, fileNm);

            RemoveFile(file3DPath);
            RemoveFile(fileJsPath);
            RemoveFile(fileImgPath);
            RemoveFile(fileThumbImgPath);

        }

        internal void CleanImg(string fileNm)
        {
            string saveIMGFolder = "Article/article_img";

            string fileImgPath = string.Format("{0}/FileUpload/{1}/{2}", AppDomain.CurrentDomain.BaseDirectory, saveIMGFolder, fileNm);
            string fileThumbImgPath = string.Format("{0}/FileUpload/{1}/thumb/{2}", AppDomain.CurrentDomain.BaseDirectory, saveIMGFolder, fileNm);

            RemoveFile(fileImgPath);
            RemoveFile(fileThumbImgPath);
        }

        internal void CleanBackup(string fileNm)
        {
            string saveIMGFolder = "Article";

            string fileImgPath = string.Format("{0}/FileUpload/{1}/{2}", AppDomain.CurrentDomain.BaseDirectory, saveIMGFolder, fileNm);

            RemoveFile(fileImgPath);
        }

        private void RemoveFile(string path)
        {
            //TotalSize

            if (System.IO.File.Exists(Path.GetFullPath(path)))
            {
                FileInfo fi = new FileInfo(Path.GetFullPath(path));

                TotalSize += fi.Length;

                System.IO.File.Delete(Path.GetFullPath(path));
            }
        }
    }
}
