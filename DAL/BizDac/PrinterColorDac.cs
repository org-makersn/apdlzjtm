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

namespace Makersn.BizDac
{
    public class PrinterColorDac : BizDacHelper
    {
        public IList<PrinterColorT> GetPrinterColorByPrinterMaterialNo(int printerMaterialNo) {
            using(ISession session = NHibernateHelper.OpenSession()){
                IList<PrinterColorT> list = session.QueryOver<PrinterColorT>().Where(w => w.PrinterMaterialNo == printerMaterialNo).List<PrinterColorT>();
                return list;
            }
 
        }
        public void RemovePrinterColorWithPrinterMaterialNo(int printerMaterialNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<PrinterColorT> colorList =  session.QueryOver<PrinterColorT>().Where(w => w.PrinterMaterialNo == printerMaterialNo).List<PrinterColorT>();
                if (colorList != null)
                {
                    foreach(PrinterColorT color in colorList){
                        session.Delete(color);
                    }
                    //session.Flush();
                }
            }
        }
        public void RemovePrinterColorWithPrinterMaterialNo(int printerMaterialNo,ISession session)
        {
          
                IList<PrinterColorT> colorList = session.QueryOver<PrinterColorT>().Where(w => w.PrinterMaterialNo == printerMaterialNo).List<PrinterColorT>();
                if (colorList != null)
                {
                    foreach (PrinterColorT color in colorList)
                    {
                        session.Delete(color);
                    }
                    session.Flush();
                }
            }
       
    }
}
