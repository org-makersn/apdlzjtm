using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;
using Makersn.Models;
using NHibernate;
using Makersn.Util;
using NHibernate.Criterion;
using System.Net;
using NHibernate.Transform;

namespace Makersn.BizDac
{
    public class PrinterModelDac : BizDacHelper
    {
        public IList<PrinterModelT> GetPrinterBrandList()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<PrinterModelT>().Where(w => w.ApprYn == "Y").OrderBy(o=>o.Brand).Asc.List<PrinterModelT>();
                //IList<PrinterModelT> list = session.QueryOver<PrinterModelT>().Where(w => w.ApprYn == "Y")
                //    .Fetch(f=>f.Brand)
                //    .Eager
                //    .List<PrinterModelT>();
                //return list;
            }
        }

        public PrinterModelT GetPrinterModelByNo(int no)
        {
            using (ISession sesson = NHibernateHelper.OpenSession())
            {
                return Session.QueryOver<PrinterModelT>().Where(w => w.No == no).SingleOrDefault<PrinterModelT>();
            }
        }

        public IList<PrinterModelT> GetPrinterModelByPrinterModelNo(int no)
        {
            IList<PrinterModelT> list = new List<PrinterModelT>();
            using (ISession session = NHibernateHelper.OpenSession())
            {
                PrinterModelT brand = session.QueryOver<PrinterModelT>().Where(w => w.No == no).Take(1).SingleOrDefault<PrinterModelT>();

                if (brand != null)
                {
                    list = session.QueryOver<PrinterModelT>().Where(w => w.Brand == brand.Brand).OrderBy(o=>o.Model).Asc.List<PrinterModelT>();
                }
            }
            return list;
        }

        public bool AddPrinterModel(PrinterModelT data)
        {
            bool result = false;
            using (ISession session = NHibernateHelper.OpenSession())
            {
                session.Save(data);
                result = true;
            }
            return result;
        }

        public IList<PrinterModelT> GetPrinterModel()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<PrinterModelT>().OrderBy(o=>o.RegDt).Desc.List<PrinterModelT>();
            }
        }

        public bool ApprPrinterModel(int no, string flag)
        {
            bool result = false;
            using (ISession session = NHibernateHelper.OpenSession())
            {
                PrinterModelT model = session.QueryOver<PrinterModelT>().Where(w => w.No == no).Take(1).SingleOrDefault<PrinterModelT>();
                if (model != null)
                {
                    model.ApprYn = flag;
                    session.Update(model);
                    session.Flush();
                    result = true;
                }
            }
            return result;
        }

        public IList<PrinterModelT> GetPendingOrApprovedPrinterModel(string apprYn)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<PrinterModelT> list = session.QueryOver<PrinterModelT>().Where(w => w.ApprYn == apprYn).OrderBy(o=>o.RegDt).Desc.List<PrinterModelT>();
                foreach (PrinterModelT model in list)
                {
                    model.PropMemberName = session.QueryOver<MemberT>().Where(w => w.No == model.PropMemberNo).Take(1).SingleOrDefault<MemberT>().Name;
                }
                return list;
            }
        }

        public bool DeletePrinterModel(int no)
        {
            bool result = false;

            using (ISession session = NHibernateHelper.OpenSession())
            {
                PrinterModelT model = session.QueryOver<PrinterModelT>().Where(w => w.No == no).Take(1).SingleOrDefault<PrinterModelT>();
                session.Delete(model);
                session.Flush();
                result = true;
            }

            return result;
        }

        public void UpdatePrinterModel(PrinterModelT model)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                session.Update(model);
                session.Flush();
            }
        }

    }
}
