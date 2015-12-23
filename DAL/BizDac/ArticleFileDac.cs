using Makersn.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.BizDac
{
    public class ArticleFileDac
    {
        #region 파일 리스트
        /// <summary>
        /// 파일 리스트
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public IList<ArticleFileT> GetFileList(int articleNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<ArticleFileT> files = session.QueryOver<ArticleFileT>().Where(w => w.ArticleNo == articleNo && w.FileGubun == "article").OrderBy(o => o.Seq).Asc.List<ArticleFileT>();
                return files;
            }
        }
        #endregion

        #region get article files cnt by no
        /// <summary>
        /// get article files cnt by no
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        public int GetArticleFileCntByNo(int articleNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                int cnt = session.QueryOver<ArticleFileT>().Where(w => w.FileGubun == "article" && w.ArticleNo == articleNo).List<ArticleFileT>().Count();
                return cnt;
            }
        }
        #endregion

        #region get article files by temp
        /// <summary>
        /// get article files by temp
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        public IList<ArticleFileT> GetArticleFilesByTemp(string temp)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<ArticleFileT> files = session.QueryOver<ArticleFileT>().Where(w => w.FileGubun != "DELETE" && w.Temp == temp).OrderBy(o => o.Seq).Asc.ThenBy(o => o.RegDt).Asc.List<ArticleFileT>();
                return files;
            }
        }
        #endregion

        #region get article file by no
        /// <summary>
        /// get article file by no
        /// </summary>
        /// <param name="fileNo"></param>
        /// <returns></returns>
        public ArticleFileT GetArticleFileByNo(int fileNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ArticleFileT file = session.QueryOver<ArticleFileT>().Where(w => w.No == fileNo).Take(1).SingleOrDefault<ArticleFileT>();
                return file;
            }
        }
        #endregion

        #region insert article file
        /// <summary>
        /// insert article file
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int InsertArticleFile(ArticleFileT data)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                int articleFileNo = (Int32)session.Save(data);
                session.Flush();

                return articleFileNo;
            }
        }
        #endregion

        #region update article file
        /// <summary>
        /// update article file
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool UpdateArticleFile(ArticleFileT data)
        {
            if (data == null) new ArgumentException("객체가 Null임");

            using (ISession session = NHibernateHelper.OpenSession())
            {
                try
                {
                    session.Update(data);
                    session.Flush();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        public bool UploadCancle(string temp)
        {
            //string query = string.Format(@"DELETE FROM ARTICLE_FILE WHERE TEMP = '{0}'", temp);
            string query = @"UPDATE ARTICLE_FILE SET FILE_GUBUN='DELETE' WHERE TEMP = :temp)";
            bool result = false;
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    IQuery queryObj = session.CreateSQLQuery(query);
                    queryObj.SetParameter("temp",temp);
                    queryObj.ExecuteUpdate();
                    transaction.Commit();
                    session.Flush();
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainImgNo"></param>
        /// <returns></returns>
        public string GetMainImg(int mainImgNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ArticleFileT articleFile = session.QueryOver<ArticleFileT>().Where(w => w.No == mainImgNo).SingleOrDefault<ArticleFileT>();
                string mainImg = articleFile.ImgName == null ? articleFile.Rename : articleFile.ImgName;
                return mainImg;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seqArray"></param>
        public void UpdateArticleFileSeq(string[] seqArray)
        {
            string query = "";
            for (int i = 0; i < seqArray.Length; i++)
            {
                //query += " UPDATE ARTICLE_FILE SET SEQ=" + i + " WHERE NO = " + seqArray[i];              
                query += " UPDATE ARTICLE_FILE SET SEQ=" + i + " WHERE NO = ? ";
            }
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    IQuery queryObj = session.CreateSQLQuery(query);
                    for (int i = 0; i < seqArray.Length; i++)
                    {
                        queryObj.SetParameter(i, seqArray[i]);
                    }
                    queryObj.ExecuteUpdate();
                    transaction.Commit();
                    session.Flush();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<ArticleFileT> GetArticleFileTargetClean()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    IList<ArticleFileT> list = session.QueryOver<ArticleFileT>().Where(w => (w.ArticleNo == 0 || w.FileGubun == "DELETE") && w.RegDt < DateTime.Now.AddDays(-1)).List();
                    return list;
                }
            }
        }

        #region STL 파일 리스트
        /// <summary>
        /// STL 파일 리스트
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public IList<ArticleFileT> GetSTLFileList(int articleNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<ArticleFileT> files = session.QueryOver<ArticleFileT>().Where(w => w.ArticleNo == articleNo && w.FileType == "stl" && w.FileGubun == "article").OrderBy(o => o.Seq).Asc.List<ArticleFileT>();
                return files;
            }
        }

        public ArticleFileT GetArticleFileByType(string type)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<ArticleFileT>().Where(w => w.FileGubun == type).SingleOrDefault<ArticleFileT>();
            }
        }
        #endregion

        public IList<ArticleFileT> TestGetArticleFileList()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<ArticleFileT>().Where(w => (w.FileType == "stl" || w.FileType == "obj") && w.FileGubun == "article").List<ArticleFileT>();
                //return session.QueryOver<ArticleFileT>().Where(w => (w.ArticleNo == 7713 || w.ArticleNo == 7712) && w.FileGubun == "article").List<ArticleFileT>();
            }
        }
        public bool TestUpdatePrintVolume(IList<ArticleFileT> data)
        {
            bool result = false;
            try
            {
                foreach (ArticleFileT file in data)
                {
                    using (ISession session = NHibernateHelper.OpenSession())
                    {
                        session.Update(file);
                        session.Flush();
                    }
                }
                result = true;
            }
            catch
            {

            }
            return result;
        }
    }
}
