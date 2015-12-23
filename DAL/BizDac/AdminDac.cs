using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;
using Makersn.Models;
using NHibernate;
using Makersn.Util;

namespace Makersn.BizDac
{
    public class AdminDac
    {
        //protected ISession Session
        //{
        //    get { return NHibernateHelper.OpenSession(); }
        //}

        public IList<MemberT> GetAdminList()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<MemberT> list = session.QueryOver<MemberT>().Where(m => (m.Level == 50 || m.Level == 99) && (m.DelFlag != "Y" || m.DelFlag == null)).OrderBy(o => o.No).Asc.List();
                session.Flush();
                return list;
            }
        }

        public MemberT GetAdminBySeq(int no)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                MemberT mem = session.QueryOver<MemberT>().Where(m => m.No == no).SingleOrDefault<MemberT>();
                session.Flush();

                return mem;
            }
        }

        public void EditAdmin(MemberT mem)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        MemberT member = session.QueryOver<MemberT>().Where(m => m.No == mem.No).SingleOrDefault<MemberT>();
                        if (member != null)
                        {
                            member.Name = mem.Name;
                            member.Password = mem.Password;
                            member.Email = mem.Email;

                            session.Update(member);

                            transaction.Commit();
                            session.Flush();
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        public int DeleteMember(MemberT mem)
        {
            int result = 0;
         
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        MemberT member = session.QueryOver<MemberT>().Where(m => m.No == mem.No).SingleOrDefault<MemberT>();
                        if (member != null)
                        {
                            member.DelFlag = "Y";
                            member.DropComment = mem.DropComment;
                            member.DelDt = DateTime.Now;
                            session.Update(member);

                            //DropMemberT dmem = new DropMemberT();
                            //dmem.No = member.No;
                            //dmem.Id = member.Id;
                            //dmem.Level = member.Level;
                            //dmem.Name = member.Name;
                            //dmem.LastLoginIp = member.LastLoginIp;
                            //dmem.LastLoginDt = member.LastLoginDt;
                            //dmem.LoginCnt = member.LoginCnt;
                            //dmem.DelDt = member.DelDt;
                            //dmem.DelFlag = member.DelFlag;
                            //dmem.DropComment = member.DropComment;
                            //dmem.RegDt = member.RegDt;
                            //dmem.RegId = member.RegId;

                            //result = (Int32)session.Save(dmem);
                            result = 1;

                            transaction.Commit();

                            session.Flush();
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    return result;
                }
            }
        }

        public int InsertAdmin(MemberT mem)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                int result = (Int32)session.Save(mem);
                session.Flush();

                return result;
            }
        }

        public int CheckId(string id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                int result = session.QueryOver<MemberT>().Where(m => m.Id == id).RowCount();
                session.Flush();
                return result;
            }
        }
    }
}
