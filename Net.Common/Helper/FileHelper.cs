using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Net.Common.Helper
{
    public class FileHelper
    {
        public class ResizeImg
        {
            public int width { get; set; }
            public int height { get; set; }
            public string foldName { get; set; }
        }

        private string __errMessage;

        public string GetErrorMsg()
        {
            return __errMessage;
        }

        public static char DirSeparator = Path.DirectorySeparatorChar;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="list"></param>
        /// <param name="folder"></param>
        /// <param name="fileNm"></param>
        /// <returns></returns>
        public string UploadFile(HttpPostedFileBase file, IList<ResizeImg> list, string folder, string fileNm)
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
                savePath = folder;
            }
            else
            {
                savePath = folder + "\\backup";
            }

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            string path = savePath + "\\" + fileNm;

            file.SaveAs(Path.GetFullPath(path));

            if (folder == "Banner")
            {
                savePath = folder;

                path = savePath + "\\fullsize\\" + fileNm;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="fileName"></param>
        /// <param name="folderName"></param>
        public void ResizeImage(HttpPostedFileBase file, int width, int height, string fileName, string folderName)
        {
            string thumbnailDirectory = folderName;

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

        /// <summary>
        /// 파일 존재 여부 체크
        /// </summary>
        /// <param name="filePath">파일경로(파일명 포함)</param>
        /// <returns>존재:true, 비존재:false</returns>
        public bool FileExist(string filePath)
        {
            if (File.Exists(filePath))
            {
                return true;    //file exist
            }
            else
            {
                return false;   //file not exist
            }
        }

        /// <summary>
        /// 폴더 존재 여부 체크
        /// </summary>
        /// <param name="filePath">파일경로를 제거한 순수한 폴더패스</param>
        /// <returns>존재:true, 비존재:false</returns>
        public bool FolderExist(string filePath)
        {
            if (Directory.Exists(filePath))
            {
                return true;    //file exist
            }
            else
            {
                return false;   //file not exist
            }
        }

        /// <summary>
        /// 파일복사
        /// </summary>
        /// <param name="srcPath">원본 파일</param>
        /// <param name="tgtPath">대상 파일</param>
        /// <returns>성공여부 true, false</returns>
        public bool FileCopy(string srcPath, string tgtPath)
        {
            try
            {
                File.Copy(srcPath, tgtPath, true);
                return true;
            }
            catch (Exception e)
            {
                this.__errMessage = e.ToString();
                return false;
            }
        }

        /// <summary>
        /// 파일 삭제
        /// </summary>
        /// <param name="filePath">파일경로(파일명 포함)</param>
        public void FileDelete(string filePath)
        {
            string fullpath = Path.GetFullPath(filePath);
            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
            }
        }

        /// <summary>
        /// 파일 이동
        /// </summary>
        /// <param name="srcPath">원본 파일</param>
        /// <param name="tgtPath">대상 파일</param>
        /// <returns>성공여부 true, false</returns>
        public bool FileMove(string srcPath, string tgtPath)
        {
            try
            {
                File.Move(srcPath, tgtPath);
                return true;
            }
            catch (Exception e)
            {
                this.__errMessage = e.ToString();
                return false;
            }
        }

        /// <summary>
        /// 파일 읽기
        /// </summary>
        /// <param name="filePath">//파일 이동</param>
        /// <returns>파일내용 text</returns>
        public string FileRead(string filePath)
        {
            string strBuff = "";
            string strBuffTemp = "";

            try
            {
                using (StreamReader sr = File.OpenText(filePath))
                {
                    while ((strBuffTemp = sr.ReadLine()) != null)
                    {
                        strBuff += strBuffTemp + "\r\n";
                    }
                    return strBuff;
                }
            }
            catch (Exception e)
            {
                this.__errMessage = e.ToString();
                return strBuff;
            }
        }

        /// <summary>
        /// 폴더가 없으면 생성 시켜줌(긴 폴더중 없으면 찾아서 모두 생성)
        /// </summary>
        /// <param name="filePath">파일경로(파일명 비포함)</param>
        /// <returns>성공여부 true, false</returns>
        public bool FilePathCreater(string filePath)
        {

            if (filePath == "" || filePath.Length == 0) return false;

            if (!FolderExist(filePath))
            {
                int splitIndex = filePath.LastIndexOf('\\');
                if (splitIndex == -1) return false;

                string childFilePath = filePath.Substring(0, splitIndex);
                FilePathCreater(childFilePath);

                try
                {
                    Directory.CreateDirectory(filePath);
                    return true;
                }
                catch (Exception e)
                {
                    this.__errMessage = e.ToString();
                    return false;
                }
            }

            return true;

        }

        /// <summary>
        /// 파일경로 + 파일명에서 실질적인 filepath만 분리해서 리턴해줌
        /// </summary>
        /// <param name="filePath">파일경로(파일명 포함)</param>
        /// <returns>파일경로(파일명 비포함)</returns>
        public string GetOriginalFilePath(string filePath)
        {
            string orgFilePath = string.Empty;

            filePath = filePath.Replace('/', '\\');

            int splitIndex = filePath.LastIndexOf('\\');
            if (splitIndex == -1)
                orgFilePath = filePath;
            else
                orgFilePath = filePath.Substring(0, splitIndex);

            return orgFilePath;
        }

        /// <summary>
        /// 파일 내용 쓰기
        /// </summary>
        /// <param name="filePath">파일경로(파일명 포함)</param>
        /// <param name="strBuff">파일에 쓸 내용</param>
        /// <returns>성공여부 true, false</returns>
        public bool FileWrite(string filePath, string strBuff)
        {
            bool IsFile;
            string orgFilePath;

            try
            {
                IsFile = FileExist(filePath);

                if (!IsFile) //파일이 존재하지 않는다면 폴더와 파일을 생성 
                {
                    orgFilePath = GetOriginalFilePath(filePath);

                    //폴더를 만듦
                    FilePathCreater(orgFilePath);

                    //파일을 생성함.
                    using (FileStream fs = File.Create(filePath)) { }
                }

                using (StreamWriter fs = File.AppendText(filePath))
                {
                    fs.WriteLine(strBuff);
                }
                return true;
            }
            catch (Exception e)
            {
                this.__errMessage = e.ToString();
                return false;
            }
        }

        /// <summary>
        /// 파일 내용 쓰기
        /// </summary>
        /// <param name="filePath">파일경로(파일명 포함)</param>
        /// <param name="strBuff">파일에 쓸 내용</param>
        /// <returns>성공여부 true, false</returns>
        public bool FileOverWrite(string filePath, string strBuff)
        {
            bool IsFile;
            string orgFilePath;

            try
            {
                IsFile = FileExist(filePath);

                if (!IsFile) //파일이 존재하지 않는다면 폴더와 파일을 생성 
                {
                    orgFilePath = GetOriginalFilePath(filePath);

                    FilePathCreater(orgFilePath);

                    using (FileStream fs = File.Create(filePath)) { }
                }

                using (StreamWriter fs = File.CreateText(filePath))
                {
                    fs.WriteLine(strBuff);
                }
                return true;
            }
            catch (Exception e)
            {
                this.__errMessage = e.ToString();
                return false;
            }
        }

        /// <summary>
        /// json 격식으로 파일 쓰기
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="jsonContents"></param>
        /// <returns></returns>
        public bool FileWriteAllText(string filePath, string strBuff)
        {
            bool IsFile;
            string orgFilePath;
            try
            {
                IsFile = FileExist(filePath);

                if (!IsFile)
                {
                    orgFilePath = GetOriginalFilePath(filePath);

                    FilePathCreater(orgFilePath);

                    File.WriteAllText(filePath, strBuff, Encoding.UTF8);
                }
                return true;
            }
            catch (Exception e)
            {
                this.__errMessage = e.ToString();
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public StreamReader OpenTextFile(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);

            return sr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        public string GetFileReadLine(StreamReader sr)
        {
            string strBuff = null;

            strBuff = sr.ReadLine();

            return strBuff;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sr"></param>
        public void CloseTextFile(StreamReader sr)
        {
            if (sr != null) sr.Close();
        }

        /// <summary>
        /// 확장자 변경
        /// </summary>
        /// <param name="path"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        public string ChangeExt(string path, string ext)
        {
            string temp = path.Substring(0, path.LastIndexOf('.'));
            return temp + "." + ext;
        }
    }
}