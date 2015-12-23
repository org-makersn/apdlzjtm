using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Makersn.Models;
using NHibernate;


namespace Makersn.BizDac
{
    public class OrderDetailDac
    {
        public OrderDetailT GetOrderDetailByNo(int orderDetailNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                OrderDetailT orderDetail = session.QueryOver<OrderDetailT>().Where(a => a.No == orderDetailNo).SingleOrDefault<OrderDetailT>();
                return orderDetail;
            }
        }

        public void InsertWithArtilfiles(int orderNo, IList<ArticleFileT> articleFiles)
        {

            DateTime regDt = DateTime.Now;
            foreach (ArticleFileT articleFile in articleFiles)
            {
                if (articleFile.FileType == "stl" || articleFile.FileType == "obj")
                {
                    OrderDetailT orderDetail = new OrderDetailT();
                    orderDetail.OrderNo = orderNo;
                    orderDetail.FileName = articleFile.Name;
                    orderDetail.FileReName = articleFile.Rename;
                    orderDetail.FileImgRename = articleFile.Rename + ".jpg";
                    orderDetail.FileType = articleFile.FileType;
                    orderDetail.SizeX = articleFile.X;
                    orderDetail.SizeY = articleFile.Y;
                    orderDetail.SizeZ = articleFile.Z;
                    orderDetail.Volume = articleFile.Volume;
                    orderDetail.RegDt = regDt;
                    using (ISession Session = NHibernateHelper.OpenSession())
                    {
                        Session.Save(orderDetail);
                        Session.Flush();
                    }
                }
            }
        }

        public IList<OrderDetailT> GetDetailListByOrderNo(long orderNo)
        {
            using (ISession Session = NHibernateHelper.OpenSession())
            {
                IList<OrderDetailT> orderDetailList = Session.QueryOver<OrderDetailT>().Where(a => a.OrderNo == orderNo).List();
                if (orderDetailList != null)
                {
                    foreach (OrderDetailT orderDetail in orderDetailList)
                    {
                        orderDetail.MaterialName = new MaterialDac().getMaterialNameByNo(orderDetail.MaterialNo);
                    }
                }
                return orderDetailList;
            }
        }


        public int InsertOrderDetail(OrderDetailT data)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                int result = (Int32)session.Save(data);
                session.Flush();
                return result;
            }
        }

        public void DeleteFile(OrderDetailT data)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                OrderDetailT detail = session.QueryOver<OrderDetailT>().Where(w => w.No == data.No).Take(1).SingleOrDefault<OrderDetailT>();
                if (detail != null)
                {
                    if (detail.Temp.Substring(0, detail.Temp.IndexOf('_')) == data.Temp)
                    {
                        session.Delete(detail);
                        session.Flush();
                    }
                }
            }
        }

        #region 주문시 orderDetail update
        /// <summary>
        ///  *주의 개당 가격 로직이 안에 있음(detail.UnitPrice
        ///  volume을 가져와야 해서 이쪽에 있음, 밖으로 detail 정보 가져오는것을 뺄 경우, 안전을 위해 안에서 체크를 한번 더 하면서 두번 가져오게 되서 이렇게 구현
        /// </summary>
        /// <param name="data"></param>
        public void UpdateOrderDetailByOrderReqeust(OrderDetailT data)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                OrderDetailT detail = session.QueryOver<OrderDetailT>().Where(w => w.No == data.No).Take(1).SingleOrDefault<OrderDetailT>();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    detail.ColorNo = data.ColorNo;
                    detail.OrderCount = data.OrderCount;
                    detail.OrderNo = data.OrderNo;
                    detail.MaterialNo = data.MaterialNo;
                    //detail.UnitPrice = (int)(data.UnitPrice * Math.Round(detail.Volume * 0.001, 1) * 0.2 + 9) / 10 * 10; //밀도 20%
                    detail.UnitPrice = (int)(data.UnitPrice * detail.PrintVolume);
                    if (detail.UnitPrice < 100) { detail.UnitPrice = 100; }

                    if (detail.OrderCount != 0)
                    {
                        session.Update(detail);
                    }
                    else
                    {
                        session.Delete(detail);
                    }
                    transaction.Commit();
                    session.Flush();
                }
            }
        }
        #endregion

        public void DeleteOrderDetailByOrder(OrderT data)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<OrderDetailT> list = session.QueryOver<OrderDetailT>().Where(w => w.OrderNo == data.No).List<OrderDetailT>();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    foreach (OrderDetailT detail in list)
                    {
                        session.Delete(detail);
                    }
                    transaction.Commit();
                    session.Flush();
                }
            }
        }
    }
}
