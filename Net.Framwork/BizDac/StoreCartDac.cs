using Net.Framework;
using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Net.Framwork.BizDac
{
    public class StoreCartDac
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
        internal List<StoreCartT> GetStoreCartByMemberNo(int memberNo)
        {
            // 장바구니 get
            List<StoreCartT> storeCartList = new List<StoreCartT>();
            memberNo = 1;

            using (dbContext = new StoreContext())
            {

                string query = @"
                            SELECT 
                             SC.NO,
                             SC.CART_NO,
                             SC.MEMBER_NO,
                             M.NAME,
                             SP.NAME AS PRODUCT_NAME,
                             SPD.TOTAL_PRICE,
                             SPD.NO AS PRODUCT_DETAIL_NO,
                             SC.ORDER_YN,
                             SC.PRODUCT_CNT,
                             SC.REG_DT,
                             SC.REG_ID,
                             SC.UPD_DT,
                             SC.UPD_ID   
                             FROM STORE_CART AS SC WITH(NOLOCK)
                             LEFT JOIN STORE_PRODUCT_DETAIL AS SPD WITH(NOLOCK) ON SC.PRODUCT_DETAIL_NO=SPD.NO
                             INNER JOIN STORE_PRODUCT AS SP WITH(NOLOCK) ON SPD.PRODUCT_NO = SP.NO
                             INNER JOIN MEMBER AS M WITH(NOLOCK) ON SC.MEMBER_NO=M.NO
                            WHERE SC.MEMBER_NO = @memberNo
                            AND ORDER_YN IS NULL ";

                using (dbContext = new StoreContext())
                {                    
                    IEnumerable<StoreCartT> data = dbContext.Database.SqlQuery<StoreCartT>(query,
                        new SqlParameter("memberNo", memberNo));
                    storeCartList = data.ToList();
                }

            }

            return storeCartList;
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

            using (dbContext = new StoreContext())
            {
                string query = @"
                                SELECT 
	                                MAX(CART_NO) + 1 AS MAX_CART_NO
                                FROM STORE_CART WITH(NOLOCK) ";

                newCartNo = dbContext.Database.SqlQuery<Int64>(query).ToString();
            }

            return newCartNo;
        }
        #endregion
    }
}
