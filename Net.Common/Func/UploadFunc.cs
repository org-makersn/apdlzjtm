using Net.Common.Configurations;
using Net.Common.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Func
{
    /// <summary>
    /// 파일 업로드
    /// </summary>
    public class UploadFunc
    {
        FileHelper fileHelper = new FileHelper();

        private static string saveFilePath = ApplicationConfiguration.Instance.FileServerUncPath;

        public string FileUpload(HttpPostedFileBase fileBase, IList<ResizeImg> imgList, string folder, string fileName)
        {
            if (fileBase == null && !(fileBase.ContentLength > 0)) return "";

            string extension = Path.GetExtension(fileBase.FileName).ToLower();

            if (string.IsNullOrEmpty(extension)) return "";

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = Guid.NewGuid().ToString() + extension;
            }

            string[] modelingExt = { ".stl", ".obj" };

            //같이 써도 되는데 혹시나 해서 ...
            string[] etcExt = { ".pdf", ".ppt", "pptx", ".xls", "xlsx", "doc", "docx" };

            string saveDir = string.Empty;

            if (modelingExt.Contains(extension))
            {
                saveDir = saveFilePath + Path.DirectorySeparatorChar + folder + Path.DirectorySeparatorChar;
            }
            else if (etcExt.Contains(extension))
            {
                fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + extension;
                saveDir = saveFilePath + Path.DirectorySeparatorChar + folder + Path.DirectorySeparatorChar;
            }
            else
            {
                saveDir = saveFilePath + Path.DirectorySeparatorChar + folder + Path.DirectorySeparatorChar + "backup" + Path.DirectorySeparatorChar;
            }

            if (fileHelper.FilePathCreater(saveDir))
            {
                fileBase.SaveAs(Path.GetFullPath(saveDir + fileName));
            }

            if (folder == "Banner")
            {
                saveDir = saveFilePath + Path.DirectorySeparatorChar + folder + Path.DirectorySeparatorChar + "fullsize" + Path.DirectorySeparatorChar;

                if (fileHelper.FilePathCreater(saveDir))
                {
                    fileBase.SaveAs(Path.GetFullPath(saveDir + fileName));
                }
            }

            if (imgList != null && imgList.Count > 0)
            {
                foreach (var image in imgList)
                {
                    ResizeImage(fileBase, image.width, image.height, fileName, image.folder);
                }
            }

            return fileName;
        }

        /// <summary>
        /// 이미지 리사이징
        /// </summary>
        /// <param name="file"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="fileName"></param>
        /// <param name="folder"></param>
        private void ResizeImage(HttpPostedFileBase file, int width, int height, string fileName, string folder)
        {
            string thumbnailDir = String.Format(@"{0}{1}{2}{3}", saveFilePath, Path.DirectorySeparatorChar, folder, Path.DirectorySeparatorChar);

            if (fileHelper.FilePathCreater(thumbnailDir))
            {
                // Final path we will save our thumbnail
                string imagePath = String.Format(@"{0}{1}", thumbnailDir, fileName);

                using (FileStream stream = new FileStream(Path.GetFullPath(imagePath), FileMode.OpenOrCreate))
                {
                    Image OrigImage = Image.FromStream(file.InputStream);
                    Bitmap TempBitmap = new Bitmap(width, height);

                    Graphics NewImage = Graphics.FromImage(TempBitmap);
                    NewImage.CompositingQuality = CompositingQuality.HighQuality;
                    NewImage.SmoothingMode = SmoothingMode.HighQuality;
                    NewImage.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    Rectangle imageRectangle = new Rectangle(0, 0, width, height);
                    NewImage.DrawImage(OrigImage, imageRectangle);

                    TempBitmap.Save(stream, OrigImage.RawFormat);

                    NewImage.Dispose();
                    TempBitmap.Dispose();
                    OrigImage.Dispose();

                    stream.Close();
                    stream.Dispose();
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ImageReSize
    {
        /// <summary>
        /// 배너 이미지 사이즈
        /// </summary>
        /// <returns></returns>
        public static IList<ResizeImg> GetBannerResize()
        {
            return new List<ResizeImg>()
            {
                //new ResizeImg(){width = 1000, height = 360, folder = "Banner/fullsize"},
                new ResizeImg(){width = 100, height = 36, folder = @"banner\thumb"}
            };
        }

        /// <summary>
        /// 게시글 이미지
        /// </summary>
        /// <returns></returns>
        public static IList<ResizeImg> GetArticleResize()
        {
            return new List<ResizeImg>()
            {
                new ResizeImg(){width = 630, height = 470, folder = @"design\article_img"},
                new ResizeImg(){width = 160, height = 120, folder = @"design\article_img\thumb"}
            };
        }

        /// <summary>
        /// 상품 이미지
        /// </summary>
        /// <returns></returns>
        public static IList<ResizeImg> GetStoreResize()
        {
            return new List<ResizeImg>()
            {
                new ResizeImg(){width = 630, height = 470, folder = @"store\img-files"},
                new ResizeImg(){width = 160, height = 120, folder = @"store\img-files\thumb"}
            };
        }

        public static IList<ResizeImg> GetMsgFIleResize()
        {
            return new List<ResizeImg>()
            {
                new ResizeImg(){width = 160, height = 90, folder = @"msg_File\thumb"},
            };
        }

        /// <summary>
        /// 프로필 이미지
        /// </summary>
        /// <returns></returns>
        public static IList<ResizeImg> GetProfileResize()
        {
            return new List<ResizeImg>()
            {
                new ResizeImg(){width = 160, height = 120, folder = @"profile\thumb"}
            };
        }

        /// <summary>
        /// Cover 이미지
        /// </summary>
        /// <returns></returns>
        public static IList<ResizeImg> GetCoverResize()
        {
            return new List<ResizeImg>()
            {
                new ResizeImg(){width = 1000, height = 240, folder = @"profile\cover"}
            };
        }

        /// <summary>
        /// 블로그 메인 이미지 사이즈
        /// </summary>
        /// <returns></returns>
        public static IList<ResizeImg> GetBlogMainResize()
        {
            return new List<ResizeImg>()
            {
                //new ResizeImg(){width = 300, height = 169, folder = @"blog\thumb"}
                new ResizeImg(){width = 408, height = 230, folder = @"blog\thumb"}
            };
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ResizeImg
    {
        public int width { get; set; }
        public int height { get; set; }
        public string folder { get; set; }
    }
}
