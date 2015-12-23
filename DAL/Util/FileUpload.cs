using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Makersn.Util
{
    public class ResizeImg
    {
        public int width { get; set; }
        public int height { get; set; }
        public string foldName { get; set; }
    }

    public class FileUpload
    {
        public static char DirSeparator = Path.DirectorySeparatorChar;
        public static string FilesPath = AppDomain.CurrentDomain.BaseDirectory + "/FileUpload/";
        //public static string FilesPath =  "/FileUpload/";

        public static string UploadFile(HttpPostedFileBase file, IList<ResizeImg> list, string folder, string fileNm)
        {
            if (null == file) return "";

            if (!(file.ContentLength > 0)) return "";


            string extension = Path.GetExtension(file.FileName).ToLower();

            if (extension == null) return "";

            if (string.IsNullOrEmpty(fileNm))
            {
                fileNm = Guid.NewGuid().ToString() + extension;
            }

            string savePath = string.Empty;

            string[] extType = { ".stl", ".obj" };
            if (extType.Contains(extension))
            {
                savePath = FilesPath + folder;
            }
            else
            {
                savePath = FilesPath + folder + "/backup";
            }

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            string path = savePath + "/" + fileNm;

            file.SaveAs(Path.GetFullPath(path));

            if (folder == "Banner")
            {
                savePath = FilesPath + folder;

                path = savePath + "/fullsize/" + fileNm;

                file.SaveAs(Path.GetFullPath(path));
            }

            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    ResizeImage(file, item.width, item.height, fileNm, item.foldName);
                }
            }

            return fileNm;
        }

        public static string UploadFileImageFlash(HttpPostedFileBase file)
        {
            if (null == file) return "";
            if (!(file.ContentLength > 0)) return "";

            string fileName = Guid.NewGuid().ToString() + "." + file.ContentType.Split('/')[1];
            string fileExt = Path.GetExtension(file.FileName);

            if (null == fileExt) return "";

            if (!Directory.Exists(FilesPath))
            {
                Directory.CreateDirectory(FilesPath);
            }

            string path = FilesPath + fileName;

            file.SaveAs(Path.GetFullPath(path));

            return fileName;
        }

        public static string DeleteFile(string fileName, IList<ResizeImg> list)
        {
            if (fileName == null || fileName.Length == 0) return "";

            if (fileName != "default_image.png")
            {
                string path = FilesPath + DirSeparator + fileName;

                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        string filePath = FilesPath + DirSeparator + item.foldName + DirSeparator + fileName;
                        RemoveFile(filePath);
                    }
                }

                RemoveFile(path);
            }
            return fileName;
        }

        public static void ResizeImage(HttpPostedFileBase file, int width, int height, string fileName, string folderName)
        {
            string thumbnailDirectory = String.Format(@"{0}{1}{2}", FilesPath, DirSeparator, folderName);

            if (!Directory.Exists(thumbnailDirectory))
            {
                Directory.CreateDirectory(thumbnailDirectory);
            }

            // Final path we will save our thumbnail
            string imagePath = String.Format(@"{0}{1}{2}", thumbnailDirectory, DirSeparator, fileName);
            FileStream stream = new FileStream(Path.GetFullPath(imagePath), FileMode.OpenOrCreate);

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

        private static void RemoveFile(string path)
        {
            if (File.Exists(Path.GetFullPath(path)))
            {
                File.Delete(Path.GetFullPath(path));
            }
        }

        public void deletefile(DirectoryInfo path)
        {
            foreach (DirectoryInfo d in path.GetDirectories())
            {
                deletefile(d);
            }
            foreach (FileInfo f in path.GetFiles())
            {
                f.Delete();
            }
        }
    }
}
