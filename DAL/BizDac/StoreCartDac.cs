using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Makersn.Models;
using NHibernate;
using Makersn.Util;
using System.Globalization;

namespace Makersn.BizDac
{
    public class StoreCartDac
    {
        #region 전역변수
        
        #endregion

        #region GetStoreCartByMemberNo - 장바구니 리스트
        /// <summary>
        /// GetStoreCartByMemberNo - 장바구니 리스트
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        public List<StoreCartT> GetStoreCartByMemberNo(int memberNo)
        {
            List<StoreCartT> storeCartList = new List<StoreCartT>();

            // 장바구니 get
            string query = @"
                            SELECT 
                             SC.NO,
                             SC.CART_NO,
                             SC.MEMBER_NO,
                             M.NAME,
                             SP.PRODUCT_NAME,
                             SPD.TOTAL_PRICE,
                             SC.ORDER_YN,
                             SC.PRODUCT_CNT 
                             FROM STORE_CART AS SC WITH(NOLOCK)
                             LEFT JOIN STORE_PRODUCT_DETAIL AS SPD WITH(NOLOCK) ON SC.PRODUCT_DETAIL_NO=SPD.NO
                             INNER JOIN STORE_PRODUCT AS SP WITH(NOLOCK) ON SPD.PRODUCT_NO = SP.NO
                             INNER JOIN MEMBER AS M WITH(NOLOCK) ON SC.MEMBER_NO=M.NO
                            WHERE SC.MEMBER_NO :memberNo
                            AND ORDER_YN IS NULL ";
            
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("memberNo", memberNo);

                IList<object[]> result = queryObj.List<object[]>();

                foreach (object[] row in result)
                {
                    StoreCartT storeCart = new StoreCartT();
                    storeCart.No = (Int64)row[0];
                    storeCart.CartNo = (string)row[1];
                    storeCart.MemberNo = (int)row[2];
                    storeCart.Name = (string)row[3];
                    storeCart.ProductName = (string)row[4];
                    storeCart.TotalPrice = (int)row[5];
                    storeCart.OrderYn = (string)row[6];
                    storeCart.ProductCnt = (int)row[7];
                    storeCartList.Add(storeCart);
                }

            }
            return storeCartList;
        }
        #endregion 

        #region InsertCart - 장바구니 담기
        /// <summary>
        /// InsertCart - 장바구니 담기
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int InsertCart(StoreCartT data)
        {
            // 장바구니에 미 주문 상품이 있는지 체크
            List<StoreCartT> storeCartList = new List<StoreCartT>();
            storeCartList = GetStoreCartByMemberNo(data.MemberNo);
            int cartGoodsCnt = storeCartList.Count;

            // 미 주문 상품이 없으면 장바구니번호 생성
            if (cartGoodsCnt == 0)
            {
                data.CartNo = GetCreateCartNo(data.MemberNo);
            }

            using (ISession session = NHibernateHelper.OpenSession())
            {                
                int result = (Int32)session.Save(data);
                session.Flush();

                return result;
            }
        }
        #endregion

        #region updateCartByCondition - 장바구니 수정
        /// <summary>
        /// updateCartByCondition - 장바구니 수정
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public int updateCartByCondition(List<StoreCartT> dataList)
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
        public void DeleteCartByCondition(int memberNo, Int64 productDetailNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        StoreCartT storeCart = new StoreCartT();
                        storeCart.No = productDetailNo;

                        session.Delete(storeCart);

                        transaction.Commit();
                        session.Flush();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }
        #endregion

        #region GetCreateCartNo - 새로운 카트번호 생성
        /// <summary>
        /// GetCreateCartNo - 새로운 카트번호 생성
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        public string GetCreateCartNo(int memberNo)
        {
            string maxCartNo = "";
            string newCartNo = "";
            
            using (ISession session = NHibernateHelper.OpenSession())
            {
                string query = @"
                            SELECT 
	                            MAX(CART_NO) AS MAX_CART_NO
                            FROM STORE_CART WITH(NOLOCK) ";

                IQuery queryObj = session.CreateSQLQuery(query);
                IList<object[]> result = queryObj.List<object[]>();
                

                foreach (object[] row in result)
                {                   
                    maxCartNo = (string)row[0];                    
                }

                newCartNo = (maxCartNo + 1).ToString();                
            }

            return newCartNo;
        }
        #endregion
    }
}
