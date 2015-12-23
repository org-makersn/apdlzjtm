using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Makersn.Models;
using NHibernate;
using Makersn.Util;

namespace Makersn.BizDac
{
    public class ContactDac
    {
        public IList<ContactT> GetContactList()
        {
            string query = @"
                                    SELECT A.NO, A.TITLE, A.EMAIL, A.CODE_NO, B.NAME AS CODE_NAME, A.STATE, A.COMMENT, A.REG_DT
                                    FROM CONTACT A INNER JOIN CODE B
				                                    ON A.CODE_NO = B.NO
                                    ";
            IList<ContactT> list = new List<ContactT>();

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<object[]> results = session.CreateSQLQuery(query).List<object[]>();
                session.Flush();

                if (results == null) { return list; }
                foreach (object[] row in results)
                {
                    ContactT c = new ContactT();
                    c.No = (int)row[0];
                    c.Title = (string)row[1];
                    c.Email = (string)row[2];
                    c.CodeNo = (int)row[3];
                    c.CodeName = (string)row[4];
                    c.StrState = Enum.GetName(typeof(MakersnEnumTypes.ContactState), (int)row[5]);
                    c.Comment = (string)row[6];
                    c.RegDt = (DateTime)row[7];
                    list.Add(c);
                }
            }
            return list;
        }

        public ContactT GetContactListByNo(int no)
        {
            string query = @"
                                    SELECT A.NO, A.TITLE, A.EMAIL, A.CODE_NO, B.NAME AS CODE_NAME, A.STATE, A.COMMENT, A.REG_DT, A.REPLY
                                    FROM CONTACT A INNER JOIN CODE B
				                                    ON A.CODE_NO = B.NO
                                    WHERE A.NO = :no ";
            ContactT c = new ContactT();

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("no", no);
                IList<object[]> results = queryObj.List<object[]>();
                session.Flush();

                if (results == null) { return c; }
                foreach (object[] row in results)
                {
                    c.No = (int)row[0];
                    c.Title = (string)row[1];
                    c.Email = (string)row[2];
                    c.CodeNo = (int)row[3];
                    c.CodeName = (string)row[4];
                    c.State = (int)row[5];
                    c.StrState = Enum.GetName(typeof(MakersnEnumTypes.ContactState), (int)row[5]);
                    c.Comment = (string)row[6];
                    c.RegDt = (DateTime)row[7];
                    c.Reply = (string)row[8];
                }
            }
            return c;
        }

        public ContactT UpdateContact(ContactT c)
        {
            ContactT contact = new ContactT();
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    contact = session.QueryOver<ContactT>().Where(a => a.No == c.No).SingleOrDefault<ContactT>();
                    if (contact != null)
                    {
                        contact.Reply = c.Reply;
                        contact.UpdDt = c.UpdDt;
                        contact.UpdId = c.UpdId;
                        contact.State = c.State;
                        session.Update(contact);
                    }
                    transaction.Commit();
                    session.Flush();
                }
            }
            return contact;
        }

        public IList<ContactT> GetContactListBySearch(int cate, string gubun, string text, string status)
        {
            string query = @"
                                    SELECT A.NO, A.TITLE, A.EMAIL, A.CODE_NO, B.NAME AS CODE_NAME, A.STATE, A.COMMENT, A.REG_DT
                                    FROM CONTACT A INNER JOIN CODE B
				                                    ON A.CODE_NO = B.NO
                                    WHERE 1 = 1 ";

            string[] textList = text.Split(' ');

            if (gubun == "sfl1" && text != "")
            {
                for (int i = 0; i < textList.Length; i++)
                {
                    query += " AND ( A.TITLE LIKE ? OR A.COMMENT LIKE ? ) ";
                }
            }
            else if (gubun == "sfl2" && text != "")
            {
                for (int i = 0; i < textList.Length; i++)
                {
                    query += " AND A.TITLE LIKE ? ";
                }
            }
            else if (gubun == "sfl3" && text != "")
            {
                for (int i = 0; i < textList.Length; i++)
                {
                    query += " AND A.COMMENT LIKE ? ";
                }
            }
            if (cate != 0) { query += " AND A.CODE_NO = :cate "; }
            if (status != "") { query += " AND A.STATE = :status "; }


            IList<ContactT> list = new List<ContactT>();

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                if (query.Contains(":cate"))
                {
                    queryObj.SetParameter("cate", cate);
                }
                if (query.Contains(":status"))
                {
                    queryObj.SetParameter("status", status);
                }
                for (int i = 0; i < textList.Length; i++)
                {
                    if (gubun == "sfl1" && text != "")
                    {

                        queryObj.SetParameter(i * 2, "%" + textList[i] + "%");
                        queryObj.SetParameter(i * 2 + 1, "%" + textList[i] + "%");
                    }
                    else if ((gubun == "sfl2" || gubun == "sfl3") && text != "")
                    {
                        queryObj.SetParameter(i, "%" + textList[i] + "%");
                    }
                }

                IList<object[]> results = queryObj.List<object[]>();

                if (results == null) { return list; }
                foreach (object[] row in results)
                {
                    ContactT c = new ContactT();
                    c.No = (int)row[0];
                    c.Title = (string)row[1];
                    c.Email = (string)row[2];
                    c.CodeNo = (int)row[3];
                    c.CodeName = (string)row[4];
                    c.StrState = Enum.GetName(typeof(MakersnEnumTypes.ContactState), (int)row[5]);
                    c.Comment = (string)row[6];
                    c.RegDt = (DateTime)row[7];
                    list.Add(c);
                }
            }
            return list;
        }

        public IList<CodeT> GetContactCodeList()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<CodeT> list = session.QueryOver<CodeT>().Where(c => c.CodeGbn == "CONTACT").OrderBy(o => o.No).Desc.List();
                session.Flush();
                return list;
            }
        }

        #region 문의사항 등록
        public void InsertQnA(ContactT contact)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(contact);
                    transaction.Commit();
                    session.Flush();
                }
            }
        }
        #endregion
    }
}
