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
    public class PrinterMaterialDac : BizDacHelper
    {
        public void InsertWithColorByStr(int printerNo, string memberName, string str)
        {
            PrinterMaterialT printerMaterial = new PrinterMaterialT();
            PrinterColorT printerColor = new PrinterColorT();
            string[] temp = str.Split(',');
            printerMaterial.MaterialNo = temp[0] != "" ? System.Convert.ToInt32(temp[0]) : -1;
            printerMaterial.PrinterNo = printerNo;
            //printerMaterial.UnitPrice = temp[1]!=""?System.Convert.ToInt32(temp[1]):-1;
            printerMaterial.RegId = memberName;
            printerMaterial.RegDt = System.DateTime.Now;

            printerColor.ColorNo = temp[2] != "" ? System.Convert.ToInt32(temp[2]) : -1;
            printerColor.PrinterNo = printerNo;
            printerColor.UnitPrice = temp[1] != "" ? System.Convert.ToInt32(temp[1]) : -1;
            printerColor.RegId = memberName;
            printerColor.RegDt = System.DateTime.Now;

            using (ISession session = NHibernateHelper.OpenSession())
            {

                int printerMaterialNo = checkPrinterMaterial(printerNo, printerMaterial.MaterialNo);
                if (printerMaterialNo == 0)
                    printerMaterialNo = (int)session.Save(printerMaterial);

                printerColor.PrinterMaterialNo = printerMaterialNo;
                session.Save(printerColor);
                session.Flush();
            }
        }
        public void InsertWithColorByStr(int printerNo, string memberName, string str, ISession session)
        {
            PrinterMaterialT printerMaterial = new PrinterMaterialT();
            PrinterColorT printerColor = new PrinterColorT();
            string[] temp = str.Replace(" ","" ).Split(',');
            printerMaterial.MaterialNo = temp[0] != "" ? System.Convert.ToInt32(temp[0]) : -1;
            printerMaterial.PrinterNo = printerNo;
            //printerMaterial.UnitPrice = temp[1]!=""?System.Convert.ToInt32(temp[1]):-1;
            printerMaterial.RegId = memberName;
            printerMaterial.RegDt = System.DateTime.Now;

            printerColor.ColorNo = temp[2] != "" ? System.Convert.ToInt32(temp[2]) : -1;
            printerColor.PrinterNo = printerNo;
            printerColor.UnitPrice = temp[1] != "" ? System.Convert.ToInt32(temp[1]) : -1;
            printerColor.RegId = memberName;
            printerColor.RegDt = System.DateTime.Now;
            int printerMaterialNo = checkPrinterMaterial(printerNo, printerMaterial.MaterialNo,session);
            if (printerMaterialNo == 0)
            {
                printerMaterialNo = (int)session.Save(printerMaterial);
                session.Flush();
            }
            printerColor.PrinterMaterialNo = printerMaterialNo;
            session.Save(printerColor);
            session.Flush();

        }

        public PrinterMaterialT GetPrinterMaterialByNo(int printerMaterialNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<PrinterMaterialT>().Where(w => w.No == printerMaterialNo).Take(1).SingleOrDefault<PrinterMaterialT>();
            }
        }

        private int checkPrinterMaterial(int printerNo, int materialNo)
        {
            string query = "SELECT NO FROM PRINTER_MATERIAL WHERE PRINTER_NO = :printerNo AND MATERIAL_NO= :materialNo";
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("printerNo", printerNo);
                queryObj.SetParameter("materialNo", materialNo);

                IList<object[]> results = queryObj.List<object[]>();

                if (results.Count >0)
                    return System.Convert.ToInt32(results[0][0]);
            }
            return 0;
        }
        private int checkPrinterMaterial(int printerNo, int materialNo,ISession session)
        {
            string query = "SELECT COUNT(*) FROM PRINTER_MATERIAL PM WHERE PRINTER_NO = :printerNo AND MATERIAL_NO= :materialNo ";
            IQuery queryObj = session.CreateSQLQuery(query);
            queryObj.SetParameter("printerNo", printerNo);
            queryObj.SetParameter("materialNo", materialNo);
           
                int count = (int)queryObj.UniqueResult();

                if (count > 0)
                {
                    query = "SELECT PM.NO FROM PRINTER_MATERIAL PM WHERE PRINTER_NO = :printerNo AND MATERIAL_NO= :materialNo ";
                    IQuery queryObj2 = session.CreateSQLQuery(query);
                    queryObj.SetParameter("printerNo", printerNo);
                    queryObj.SetParameter("materialNo", materialNo);


                    return (int)session.CreateSQLQuery(query).UniqueResult<object>();
                }
            return 0;
        }
        public IList<PrinterMaterialT> GetPrinterMaterialByPrinterNo(int printerNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<PrinterMaterialT> materialList = session.QueryOver<PrinterMaterialT>().Where(w => w.PrinterNo == printerNo).List<PrinterMaterialT>();
                foreach (PrinterMaterialT material in materialList)
                {
                    material.MaterialColorList = new PrinterColorDac().GetPrinterColorByPrinterMaterialNo(material.No);
                    material.MaterialName = new MaterialDac().getMaterialNameByNo(material.MaterialNo);
                }
                return materialList;
            }
        }
        public void RemovePrinterMaterialAndColorWithPrtNo(int printerNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                string query = "DELETE FROM PRINTER_MATERIAL WHERE PRINTER_NO = :printerNo ; ";
                query += "DELETE FROM PRINTER_COLOR WHERE PRINTER_MATERIAL_NO NOT IN (SELECT NO FROM PRINTER_MATERIAL);";

                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("printerNo", printerNo);

                queryObj.ExecuteUpdate();
                session.Flush();
                //IList<PrinterMaterialT> materialList = session.QueryOver<PrinterMaterialT>().Where(w => w.PrinterNo == printerNo).List<PrinterMaterialT>();
                //foreach (PrinterMaterialT material in materialList)
                //{
                //    new PrinterColorDac().RemovePrinterColorWithPrinterMaterialNo(material.No);
                //}

                //if (materialList != null)
                //{
                //    foreach (PrinterMaterialT material in materialList)
                //    {
                //        session.Delete(material);
                //    }
                //    //session.Flush();
                //}
            }

        }
        public void RemovePrinterMaterialAndColorWithPrtNo(int printerNo, ISession session)
        {
            string query = "DELETE FROM PRINTER_MATERIAL WHERE PRINTER_NO = :printerNo ;";
            query += "DELETE FROM PRINTER_COLOR WHERE PRINTER_MATERIAL_NO NOT IN (SELECT NO FROM PRINTER_MATERIAL);";

            IQuery queryObj = session.CreateSQLQuery(query);
            queryObj.SetParameter("printerNo", printerNo);

            queryObj.ExecuteUpdate();
            session.Flush();
                //IList<PrinterMaterialT> materialList = session.QueryOver<PrinterMaterialT>().Where(w => w.PrinterNo == printerNo).List<PrinterMaterialT>();
                //foreach (PrinterMaterialT material in materialList)
                //{
                //    new PrinterColorDac().RemovePrinterColorWithPrinterMaterialNo(material.No, session);
                //}

                //if (materialList != null)
                //{
                //    foreach (PrinterMaterialT material in materialList)
                //    {
                //        session.Delete(material);
                //    }
                //    session.Flush();
                //}
            

        }

        public void Todo(PrinterT insert, string remove, string insert1, string remove1, string insert2)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (var transation = session.BeginTransaction())
                {

                    transation.Commit();
                }
            }
        }
    }
}
