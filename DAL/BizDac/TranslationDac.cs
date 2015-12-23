using Makersn.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.BizDac
{
    public class TranslationDac
    {
        TranslationDetailDac _translationDetailDac = new TranslationDetailDac();

        public int InsertTranslation(TranslationT data)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return (int)session.Save(data);
            }
        }

        public TranslationT CheckTranslation(TranslationT data)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<TranslationT>().Where(w => w.ArticleNo == data.ArticleNo && w.LangTo == data.LangTo).Take(1).SingleOrDefault<TranslationT>();
            }
        }

        public TranslationT GetTranslation(int articleNo, string langTo, int transFlag)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<TranslationT>().Where(w => w.ArticleNo == articleNo && w.LangTo == langTo && w.TransFlag == transFlag).Take(1).SingleOrDefault<TranslationT>();
            }
        }

        public void DeleteTranslation(TranslationT data)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    _translationDetailDac.DeleteTranslationDetail(data.No);

                    session.Delete(data);
                    transaction.Commit();
                    session.Flush();
                }
            }
        }

    }
}
