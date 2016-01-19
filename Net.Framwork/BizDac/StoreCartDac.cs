using Net.Framework;
using Net.Framework.StoreModel;
using Net.Framwork.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace Net.Framwork.BizDac
{
    public class StoreCartDac : DacBase
    {
        #region 전역변수
        private static StoreContext dbContext;
        #endregion

        #region GetStoreCartByMemberNo - 장바구니 리스트
        /// <summary>
        /// GetStoreCartByMemberNo - 장바구니 리스트
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        internal List<StoreCartInfo> GetStoreCartByMemberNo(int memberNo)
        {
            // 장바구니 get
            List<StoreCartInfo> list = new List<StoreCartInfo>();
            string query = DacHelper.GetSqlCommand("StoreCart.SelectCartList_S");

            //var states = dbHelper.ExecuteMultiple<StoreCartInfo>(query);

            using (dbContext = new StoreContext())
            {
                list = dbContext.Database.SqlQuery<StoreCartInfo>(query,
                    new SqlParameter("MEMBER_NO", memberNo)).ToList();                
            }

            return list;
        }
        #endregion 

        #region InsertStoreCart - 장바구니 저장
        /// <summary>
        /// InsertStoreCart - 장바구니 저장
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int InsertStoreCart(StoreCartT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");
            int ret = 0;
            using (dbContext = new StoreContext())
            {
                dbContext.StoreCartT.Add(data);
                ret = dbContext.SaveChanges();
            }
            return ret;
        }
        #endregion

        #region updateCartByCondition - 장바구니 수정
        /// <summary>
        /// updateCartByCondition - 장바구니 수정
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        internal int updateCartByCondition(List<StoreCartT> dataList)
        {
            int result = 0;

            return result;
        }
        #endregion

        #region DeleteCartByCondition - 장바구니 물건 삭제
        /// <summary>
        /// DeleteCartByCondition - 장바구니 물건 삭제
        /// </summary>
        /// <param name="memberNo"></param>
        /// <param name="productDetailNo"></param>
        /// <returns></returns>
        internal void DeleteCartByCondition(int memberNo, Int64 productDetailNo)
        {
            //using (ISession session = NHibernateHelper.OpenSession())
            //{
            //    using (ITransaction transaction = session.BeginTransaction())
            //    {
            //        try
            //        {
            //            StoreCartT storeCart = new StoreCartT();
            //            storeCart.No = productDetailNo;

            //            session.Delete(storeCart);

            //            transaction.Commit();
            //            session.Flush();
            //        }
            //        catch (Exception)
            //        {
            //            throw;
            //        }
            //    }
            //}
        }
        #endregion

        #region GetCreateCartNo - 새로운 카트번호 생성
        /// <summary>
        /// GetCreateCartNo - 새로운 카트번호 생성
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        internal string GetCreateCartNo()
        {
            string newCartNo = "";

            string query = DacHelper.GetSqlCommand("StoreCart.SelectCartList_S");

            using (dbContext = new StoreContext())
            {
                newCartNo = dbContext.Database.SqlQuery<string>(query).ToString();
            }

            return newCartNo;
        }
        #endregion
    }
}
