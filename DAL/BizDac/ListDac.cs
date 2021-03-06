﻿using Makersn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Makersn.BizDac
{
    public class ListDac
    {
        
        public IList<ListT> GetListNames(int memberNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<ListT>().Where(w => w.MemberNo == memberNo).OrderBy(w=>w.RegDt).Desc.List<ListT>();
            }
        }

        public bool InsertListArticle(ListArticleT data)
        {
            bool result = false;
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ListArticleT chk = session.QueryOver<ListArticleT>().Where(w => w.ListNo == data.ListNo && w.ArticleNo == data.ArticleNo && w.MemberNo == data.MemberNo).Take(1).SingleOrDefault<ListArticleT>();
                if (chk == null)
                {
                    using (ITransaction transection = session.BeginTransaction())
                    {
                        session.Save(data);
                        transection.Commit();
                        session.Flush();
                        result = true;
                    }
                }
            }
            return result;
        }

        public IList<ListArticleT> GetListItem(int memberNo)
        {

            //SqlConnection con = new SqlConnection(conStr);
            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = "GET_LIST_ITEM_FRONT";
            //cmd.Parameters.Add("@MEMBER_NO", SqlDbType.Int).Value = memberNo;
            //cmd.Connection = con;

            //IList<ListArticleT> list = new List<ListArticleT>();

            //try
            //{
            //    con.Open();
            //    SqlDataReader sr = cmd.ExecuteReader();
            //    while (sr.Read())
            //    {
            //        ListArticleT item = new ListArticleT();
            //        item.ListNo = (int)sr["LIST_NO"];
            //        item.ArticleNo = (int)sr["ARTICLE_NO"];
            //        item.ImgName = (string)sr["IMG_NAME"];
            //        list.Add(item);
            //    }
            //    sr.Close();
            //    cmd.Connection.Close();
            //    cmd.Dispose();

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    con.Close();
            //    con.Dispose();
            //}

            //return list;


            IList<ListArticleT> list = new List<ListArticleT>();
            string query = @"SELECT LA.LIST_NO, LA.ARTICLE_NO, ISNULL(AF.IMG_NAME, AF.RENAME) AS IMG_NAME
                                    FROM LIST_ARTICLE LA INNER JOIN ARTICLE A
				                                    ON LA.ARTICLE_NO = A.NO
				                                    AND LA.MEMBER_NO = :memberNo
                                                    AND A.VISIBILITY = 'Y'
				                                    INNER JOIN ARTICLE_FILE AF
				                                    ON LA.ARTICLE_NO = AF.ARTICLE_NO
				                                    AND A.MAIN_IMAGE = AF.NO
                                    ORDER BY LA.REG_DT DESC";
            using (ISession session = NHibernateHelper.OpenSession())
            {

                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("memberNo", memberNo);

                IList<object[]> results = queryObj.List<object[]>();

                session.Flush();

                foreach (object[] row in results)
                {
                    ListArticleT item = new ListArticleT();
                    item.ListNo = (int)row[0];
                    item.ArticleNo = (int)row[1];
                    item.ImgName = (string)row[2];
                    list.Add(item);
                }
            }
            return list;
        }

        public IList<ArticleT> GetMemberListItems(string memberNo, int visitorNo, int listNo)
        {
            //SqlConnection con = new SqlConnection(conStr);
            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = "GET_MEMBER_LIST_ITEMS_FRONT";
            //cmd.Parameters.Add("@MEMBER_NO", SqlDbType.Int).Value = memberNo;
            //cmd.Parameters.Add("@VISITOR_NO", SqlDbType.Int).Value = visitorNo;
            //cmd.Parameters.Add("@LIST_NO", SqlDbType.Int).Value = listNo;
            //cmd.Connection = con;

            //IList<ArticleT> list = new List<ArticleT>();

            //try
            //{
            //    con.Open();
            //    SqlDataReader sr = cmd.ExecuteReader();
            //    while (sr.Read())
            //    {
            //        ArticleT article = new ArticleT();
            //        article.No = (int)sr["NO"];
            //        article.ImageName = (string)sr["MAIN_IMAGE"];
            //        article.Title = (string)sr["TITLE"];
            //        //article.Category = (string)sr["CATEGORY"];
            //        article.MemberName = (string)sr["MEMBER_NAME"];
            //        article.RegDt = (DateTime)sr["REG_DT"];
            //        article.Visibility = (string)sr["VISIBILITY"];
            //        article.Path = (string)sr["PATH"] + article.ImageName;
            //        article.ViewCnt = (int)sr["VIEWCNT"];
            //        article.CommentCnt = (int)sr["COMMENT_CNT"];
            //        article.LikeCnt = (int)sr["LIKE_CNT"];
            //        article.MemberNo = (int)sr["MEMBER_NO"];
            //        article.chkLikes = (int)sr["CHK_LIKES"];
            //        list.Add(article);
            //    }
            //    sr.Close();
            //    cmd.Connection.Close();
            //    cmd.Dispose();

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    con.Close();
            //    con.Dispose();
            //}

            //return list;


            string query = @"SELECT A.NO, ISNULL(AF.IMG_NAME, AF.RENAME) AS MAIN_IMAGE, A.TITLE, C.NAME AS CATEGORY, M.NAME AS MEMBER_NAME , A.REG_DT, A.VISIBILITY, AF.PATH, A.VIEWCNT,
                            (SELECT count(0) 
                                       FROM   ARTICLE_COMMENT B 
                                       WHERE  A.NO = B.ARTICLE_NO) AS COMMENT_CNT,
                            (SELECT count(0) 
                                    FROM   LIKES B 
                                    WHERE  A.NO = B.ARTICLE_NO) AS LIKE_CNT,
                            A.MEMBER_NO,
                            (SELECT count(0) 
                                    FROM   LIKES B 
                                    WHERE   
                                        A.NO = B.ARTICLE_NO AND B.MEMBER_NO = :visitorNo ) AS CHK_LIKES

                            FROM ARTICLE A WITH(NOLOCK) INNER JOIN ARTICLE_FILE AS AF WITH(NOLOCK)
					                            ON A.MAIN_IMAGE = AF.NO
					                            LEFT JOIN CODE AS C WITH(NOLOCK)
					                            ON A.CODE_NO = C.NO
					                            LEFT JOIN MEMBER AS M WITH(NOLOCK)
					                            ON A.MEMBER_NO = M.NO 
                                                INNER JOIN LIST_ARTICLE LA
                                                ON A.NO = LA.ARTICLE_NO
												AND LA.LIST_NO = :listNo
                            WHERE (M.DEL_FLAG != 'Y' OR M.DEL_FLAG IS NULL)
                                                AND A.VISIBILITY = 'Y'  AND LA.MEMBER_NO = :memberNo ORDER BY A.REG_DT DESC";



            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("visitorNo", visitorNo);
                queryObj.SetParameter("memberNo", memberNo);
                queryObj.SetParameter("listNo", listNo);

                IList<object[]> results = queryObj.List<object[]>();


                session.Flush();

                IList<ArticleT> list = new List<ArticleT>();
                foreach (object[] row in results)
                {
                    ArticleT article = new ArticleT();
                    article.No = (int)row[0];
                    article.ImageName = (string)row[1];
                    article.Title = (string)row[2];
                    article.Category = (string)row[3];
                    article.MemberName = (string)row[4];
                    article.RegDt = (DateTime)row[5];
                    article.Visibility = (string)row[6];
                    article.Path = (string)row[7] + article.ImageName;
                    article.ViewCnt = (int)row[8];
                    article.CommentCnt = (int)row[9];
                    article.LikeCnt = (int)row[10];
                    article.MemberNo = (int)row[11];
                    article.chkLikes = (int)row[12];
                    list.Add(article);
                }

                return list;
            }
        }

        public int GetListARticleCntByArticleNo(int articleNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<ListArticleT>().Where(w => w.ArticleNo== articleNo).RowCount();
            }
        }

        public ListT GetSingleListbyListNo(int listNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<ListT>().Where(w => w.No == listNo).Take(1).SingleOrDefault<ListT>();
            }
        }

        public bool UpdateListName(ListT data)
        {
            bool result = false;
            using (ISession session = NHibernateHelper.OpenSession()) 
            {
                ListT list = session.QueryOver<ListT>().Where(w => w.No == data.No && w.MemberNo==data.MemberNo).Take(1).SingleOrDefault<ListT>();
                if (list != null)
                {
                    list.ListName = data.ListName;
                    session.Update(list);
                    session.Flush();
                    result = true;
                }
            }
            return result;
        }

        public bool DeleteList(ListT data)
        {
            bool result = false;
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ListT list = session.QueryOver<ListT>().Where(w => w.No == data.No && w.MemberNo == data.MemberNo).Take(1).SingleOrDefault<ListT>();
                if (list != null)
                {
                    IList<ListArticleT> listArticle = session.QueryOver<ListArticleT>().Where(w => w.ListNo == list.No).List<ListArticleT>();
                    session.Delete(list);
                    foreach (ListArticleT item in listArticle)
                    {
                        session.Delete(item);
                    }
                    session.Flush();
                    result = true;
                }
            }
            return result;
        }

        public bool DeleteArticleInList(ListArticleT data)
        {
            bool result = false;
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ListArticleT listArticle = session.QueryOver<ListArticleT>().Where(w => w.MemberNo == data.MemberNo && w.ArticleNo == data.ArticleNo && w.ListNo == data.ListNo).Take(1).SingleOrDefault<ListArticleT>();
                if (listArticle != null)
                {
                    session.Delete(listArticle);
                    session.Flush();
                    result = true;
                }
            }
            return result;
        }
    }
}
