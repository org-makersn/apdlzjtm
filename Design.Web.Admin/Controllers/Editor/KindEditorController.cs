using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Design.Web.Admin.Controllers.Editor
{
    public class KindEditorController : Controller
    {
        //you can specify an absolute path，
        //private static string rootPath = AppDomain.CurrentDomain.BaseDirectory + "/FileUpload/KindEditor/attached/";
        private static string rootPath = "/FileUpload/KindEditor/attached/";
        //file extension
        private static string[] fileTypes = { "gif", "jpg", "jpeg", "png", "bmp" };

        //private HttpContext context;

        [HttpPost]
        public ActionResult UploadImage()
        {
            //String projectUrl = Request.Path.Substring(0, Request.Path.LastIndexOf("/") + 1);

            string savePath = rootPath;
            string saveUrl = rootPath;

            Hashtable extFileHash = new Hashtable();
            extFileHash.Add("image", "gif,jpg,jpeg,png,bmp");
            extFileHash.Add("flash", "swf,flv");
            extFileHash.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
            extFileHash.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");

            int maxSize = 1000000;

            Hashtable hashResult = new Hashtable();

            HttpPostedFileBase imgFile = Request.Files["imgFile"];
            if (imgFile == null)
            {
                hashResult = new Hashtable();
                hashResult["error"] = 1;
                hashResult["message"] = "Please select the file";
                return Json(hashResult, JsonRequestBehavior.AllowGet);
            }

            string dirPath = Server.MapPath(savePath);
            if (!Directory.Exists(dirPath))
            {
                hashResult = new Hashtable();
                hashResult["error"] = 1;
                hashResult["message"] = "Upload directory does not exist";
                //return Json(hashResult, JsonRequestBehavior.AllowGet);
            }

            string dirName = Request.QueryString["dir"];
            if (String.IsNullOrEmpty(dirName))
            {
                dirName = "image";
            }
            if (!extFileHash.ContainsKey(dirName))
            {
                hashResult = new Hashtable();
                hashResult["error"] = 1;
                hashResult["message"] = "Upload directory does not exist";
                //return Json(hashResult, JsonRequestBehavior.AllowGet);
            }

            string fileName = imgFile.FileName;
            string fileExt = Path.GetExtension(fileName).ToLower();

            if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
            {
                hashResult = new Hashtable();
                hashResult["error"] = 1;
                hashResult["message"] = "Upload file size exceeds the limit";
                return Json(hashResult, JsonRequestBehavior.AllowGet);
            }

            if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extFileHash[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                hashResult = new Hashtable();
                hashResult["error"] = 1;
                hashResult["message"] = "Upload file extension is not allowed extensions";
                return Json(hashResult, JsonRequestBehavior.AllowGet);
            }

            //Create Directory
            dirPath += dirName + "/";
            saveUrl += dirName + "/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            string ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
            dirPath += ymd + "/";
            saveUrl += ymd + "/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            string newFileName = Guid.NewGuid().ToString() + fileExt;
            string filePath = dirPath + newFileName;

            imgFile.SaveAs(Path.GetFullPath(filePath));
            
            //imgFile.SaveAs(filePath);

            string fileUrl = saveUrl + newFileName;

            hashResult = new Hashtable();
            hashResult["error"] = 0;
            hashResult["url"] = fileUrl;

            return Json(hashResult, "text/html;charset=UTF-8", JsonRequestBehavior.AllowGet);
        }

        public ActionResult FileManager()
        {
            //string aspxUrl = Request.Path.Substring(0, Request.Path.LastIndexOf("/") + 1);

            string rootUrl = rootPath;
            string fileTypes = "gif,jpg,jpeg,png,bmp";

            string currentPath = "";
            string currentUrl = "";
            string currentDirPath = "";
            string moveupDirPath = "";

            string dirPath = Server.MapPath(rootPath);
            string dirName = Request.QueryString["dir"];
            if (!String.IsNullOrEmpty(dirName))
            {
                if (Array.IndexOf("image,flash,media,file".Split(','), dirName) == -1)
                {
                    Response.Write("Invalid Directory name.");
                    Response.End();
                }
                dirPath += dirName + "/";
                //rootUrl += dirName + "/";
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
            }

            //根据path参数，设置各路径和URL
            string path = Request.QueryString["path"];
            path = String.IsNullOrEmpty(path) ? "" : path;
            if (path == "")
            {
                currentPath = Server.MapPath(rootPath);
                currentUrl = rootUrl;
                currentDirPath = "";
                moveupDirPath = "";
            }
            else
            {
                currentPath = Server.MapPath(rootPath) + path;
                currentUrl = rootUrl + path;
                currentDirPath = path;
                moveupDirPath = Regex.Replace(currentDirPath, @"(.*?)[^\/]+\/$", "$1");
            }

            //排序形式，name or size or type
            string order = Request.QueryString["order"];
            order = String.IsNullOrEmpty(order) ? "" : order.ToLower();

            //不允许使用..移动到上一级目录
            if (Regex.IsMatch(path, @"\.\."))
            {
                Response.Write("Access is not allowed.");
                Response.End();
            }
            //最后一个字符不是/
            if (path != "" && !path.EndsWith("/"))
            {
                Response.Write("Parameter is not valid.");
                Response.End();
            }
            //目录不存在或不是目录
            if (!Directory.Exists(currentPath))
            {
                Response.Write("Directory does not exist.");
                Response.End();
            }

            //遍历目录取得文件信息
            string[] dirList = Directory.GetDirectories(currentPath);
            string[] fileList = Directory.GetFiles(currentPath);

            switch (order)
            {
                case "size":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new SizeSorter());
                    break;
                case "type":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new TypeSorter());
                    break;
                case "name":
                default:
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new NameSorter());
                    break;
            }

            Hashtable result = new Hashtable();
            result["moveup_dir_path"] = moveupDirPath;
            result["current_dir_path"] = currentDirPath;
            result["current_url"] = currentUrl;
            result["total_count"] = dirList.Length + fileList.Length;
            List<Hashtable> dirFileList = new List<Hashtable>();
            result["file_list"] = dirFileList;
            for (int i = 0; i < dirList.Length; i++)
            {
                DirectoryInfo dir = new DirectoryInfo(dirList[i]);
                Hashtable hash = new Hashtable();
                hash["is_dir"] = true;
                hash["has_file"] = (dir.GetFileSystemInfos().Length > 0);
                hash["filesize"] = 0;
                hash["is_photo"] = false;
                hash["filetype"] = "";
                hash["filename"] = dir.Name;
                hash["datetime"] = dir.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }
            for (int i = 0; i < fileList.Length; i++)
            {
                FileInfo file = new FileInfo(fileList[i]);
                Hashtable hash = new Hashtable();
                hash["is_dir"] = false;
                hash["has_file"] = false;
                hash["filesize"] = file.Length;
                hash["is_photo"] = (Array.IndexOf(fileTypes.Split(','), file.Extension.Substring(1).ToLower()) >= 0);
                hash["filetype"] = file.Extension.Substring(1);
                hash["filename"] = file.Name;
                hash["datetime"] = file.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }

            return Json(result, "text/html;charset=UTF-8", JsonRequestBehavior.AllowGet);
        }

        public class NameSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                FileInfo xInfo = new FileInfo(x.ToString());
                FileInfo yInfo = new FileInfo(y.ToString());

                return xInfo.FullName.CompareTo(yInfo.FullName);
            }
        }

        public class SizeSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                FileInfo xInfo = new FileInfo(x.ToString());
                FileInfo yInfo = new FileInfo(y.ToString());

                return xInfo.Length.CompareTo(yInfo.Length);
            }
        }

        public class TypeSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                FileInfo xInfo = new FileInfo(x.ToString());
                FileInfo yInfo = new FileInfo(y.ToString());

                return xInfo.Extension.CompareTo(yInfo.Extension);
            }
        }
    }
}