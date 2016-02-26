using Makersn.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.BizDac
{
    public class PrinterFileDac
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainImgNo"></param>
        /// <returns></returns>
        public string GetMainImg(int mainImgNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                PrinterFileT PrinterFile = session.QueryOver<PrinterFileT>().Where(w => w.No == mainImgNo).SingleOrDefault<PrinterFileT>();
                string mainImg = PrinterFile.Name == null ? PrinterFile.ReName : PrinterFile.Name;
                return mainImg;
            }
        }

    }
}
