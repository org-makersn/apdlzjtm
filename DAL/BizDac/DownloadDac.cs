using Makersn.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.BizDac
{
    public class DownloadDac
    {
        public void InsertDownloadCnt(DownloadT download)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                DownloadT chk = session.QueryOver<DownloadT>().Where(w => w.MemberNo == download.MemberNo && w.ArticleNo == download.ArticleNo).Take(1).SingleOrDefault<DownloadT>();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    if (chk == null)
                    {
                        session.Save(download);
                    }
                    else
                    {
                        chk.Cnt++;
                        session.Update(chk);
                    }
                    transaction.Commit();
                    session.Flush();
                }
            }
        }
    }
}
