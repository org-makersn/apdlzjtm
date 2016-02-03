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
        internal int SetCartByCartNo(string cartNo)
        {
            if (cartNo == null) throw new ArgumentNullException("The expected Segment data is not here.");

            int ret = 0;
            using (dbContext = new StoreContext())
            {
                List<StoreCartT> originDataList = dbContext.StoreCartT.Where(s => s.CartNo == cartNo).ToList();

                foreach (StoreCartT originData in originDataList)
                {
                    if (originData != null)
                    {
                        try
                        {
                            originData.OrderYn = "Y";
                            dbContext.StoreCartT.Attach(originData);
                            dbContext.Entry<StoreCartT>(originData).State = System.Data.Entity.EntityState.Modified;
                            dbContext.SaveChanges();
                        }
                        catch (Exception)
                        { 
                        }
                    }
                    else
                    {
                        ret = -2;
                        throw new NullReferenceException("The expected original Segment data is not here.");
                    }
                }
            }
            return ret;
        }
        #endregion

        #region DeleteCartByCondition - 장바구니 물건 삭제
        /// <summary>
        /// DeleteCartByCondition - 장바구니 물건 삭제
        /// </summary>
        /// <param name="memberNo"></param>
        /// <param name="productDetailNo"></param>
        /// <returns></returns>
        internal int DeleteCartByCondition(int memberNo, Int64 productDetailNo)
        {
            if (productDetailNo == null) throw new ArgumentNullException("The expected Segment data is not here.");

            int ret = 0;
            using (dbContext = new StoreContext())
            {
                StoreCartT originData = dbContext.StoreCartT.SingleOrDefault(s => s.No == productDetailNo && s.MemberNo == memberNo);
                if (originData != null)
                {
                    try
                    {
                        originData.No = productDetailNo;
                        dbContext.StoreCartT.Remove(originData);
                        ret = dbContext.SaveChanges();
                    }
                    catch (Exception)
                    {
                        ret = -1;
                    }
                }
                else
                {
                    ret = -2;
                    throw new NullReferenceException("The expected original Segment data is not here.");
                }
            }
            return ret;
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
            List<int> newCartNo;

            string query = DacHelper.GetSqlCommand("StoreCart.GetMaxCartNo");

            using (dbContext = new StoreContext())
            {
                newCartNo = dbContext.Database.SqlQuery<int>(query).ToList();
            }

            return newCartNo[0].ToString();
        }
        #endregion
    }
}
