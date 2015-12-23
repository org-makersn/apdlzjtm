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
        public string GetMainImg(int? mainImgNo, int printerNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                PrinterFileT PrinterFile = session.QueryOver<PrinterFileT>().Where(w => w.Seq == mainImgNo && w.PrinterNo == printerNo).SingleOrDefault<PrinterFileT>();
                string mainImg = PrinterFile.Name == null ? PrinterFile.Rename : PrinterFile.Name;
                return mainImg;
            }
        }

        public int InsertPrinterFile(PrinterFileT printerFile)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                int printerFileNo = (Int32)session.Save(printerFile);
                session.Flush();

                return printerFileNo;
            }
        }
        public void InsertPrinterFileByList(List<PrinterFileT> fileList)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                foreach (PrinterFileT file in fileList)
                {
                    session.Save(file);
                }
                session.Flush();
            }
        }
        public void InsertPrinterFileByList(List<PrinterFileT> fileList,ISession session)
        {
            
                foreach (PrinterFileT file in fileList)
                {
                    session.Save(file);
                }
                session.Flush();
 
        }


        public IList<PrinterFileT> GetFileList(int printerNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<PrinterFileT> files = session.QueryOver<PrinterFileT>().Where(w => w.PrinterNo == printerNo && w.FileGubun == "prt_img").OrderBy(o => o.No).Asc.List<PrinterFileT>();
                return files;
            }
        }

        public IList<PrinterFileT> GetPrinterFilesByTemp(string temp)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<PrinterFileT> files = session.QueryOver<PrinterFileT>().Where(w => w.Temp == temp).OrderBy(o => o.No).Asc.List<PrinterFileT>();
                return files;
            }
        }
        public void RemovePrinterFileWithPrtNo(int printerNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                string query = "DELETE FROM PRINTER_FILE WHERE PRINTER_NO = " + printerNo;
                session.CreateSQLQuery(query).ExecuteUpdate();
                session.Flush();

                //IList<PrinterFileT> files = session.QueryOver<PrinterFileT>().Where(w => w.PrinterNo == printerNo).List<PrinterFileT>();
                //if (files != null)
                //{
                //    foreach (PrinterFileT file in files)
                //    {
                //        session.Delete(file);
                //    }
                //    //session.Flush();
                //}
            }
        }
        public void RemovePrinterFileWithPrtNo(int printerNo,ISession session)
        {
            string query = "DELETE FROM PRINTER_FILE WHERE PRINTER_NO = :printerNo ";
            IQuery queryObj = session.CreateSQLQuery(query);
            queryObj.SetParameter("printerNo", printerNo);

            queryObj.ExecuteUpdate();
            session.Flush();
                //IList<PrinterFileT> files = session.QueryOver<PrinterFileT>().Where(w => w.PrinterNo == printerNo).List<PrinterFileT>();
                //if (files != null)
                //{
                //    foreach (PrinterFileT file in files)
                //    {
                //        session.Delete(file);
                //    }
                //    session.Flush();
                //}
            }
        

    }
}
