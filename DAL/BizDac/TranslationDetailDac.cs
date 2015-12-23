﻿using Makersn.Models;
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

        //public void UpdateTranslantionDetail(TranslationDetailT tran)
        //{
        //    using (ISession session = NHibernateHelper.OpenSession())
        //    {
        //        session.Update(tran);
        //    }
        //}

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
    }
}
