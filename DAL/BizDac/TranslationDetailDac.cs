using Makersn.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.BizDac
{
    public class TranslationDetailDac
    {
        public TranslationDetailT GetTranslationDetailByArticleNoAndLangFlag(int articleNo, string langFlag)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<TranslationDetailT>().Where(w => w.ArticleNo == articleNo && w.LangFlag == langFlag).Take(1).SingleOrDefault<TranslationDetailT>();
            }
        }

        public int SaveOrUpdateTranslationDetail(TranslationDetailT data)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                int translationDetailNo = 0;

                if (data.No > 0)
                {
                    session.Update(data);
                    translationDetailNo = data.No;
                }
                else
                {
                    translationDetailNo = (Int32)session.Save(data);
                }
                session.Flush();
                return translationDetailNo;
            }
        }

        public void UpdateTranslantionDetail(TranslationDetailT tran)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                session.Update(tran);
            }
        }

        public void DeleteTranslationDetail(int translationNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                TranslationDetailT detail = session.QueryOver<TranslationDetailT>().Where(w => w.TranslationNo == translationNo).Take(1).SingleOrDefault<TranslationDetailT>();
                if (detail != null)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Delete(detail);
                        transaction.Commit();
                        session.Flush();
                    }
                }
            }
        }


        public int SaveOrUpdateTranslationDetail(TranslationDetailT data, TranslationT translationData)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                int translationDetailNo = 0;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    if (data.No > 0)
                    {
                        session.Update(data);
                        translationDetailNo = data.No;
                    }
                    else
                    {
                        translationDetailNo = (Int32)session.Save(data);
                    }
                    session.Update(translationData);
                    transaction.Commit();
                    session.Flush();
                }

                return translationDetailNo;
            }
        }

        public void ChkTranslationDetailAndDelete(int articleNo, string langFlag)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                TranslationDetailT chk = session.QueryOver<TranslationDetailT>().Where(w => w.ArticleNo == articleNo && w.LangFlag == langFlag).SingleOrDefault<TranslationDetailT>();
                if (chk != null)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Delete(chk);
                        transaction.Commit();
                        session.Flush();
                    }
                }
            }
        }

    }
}
