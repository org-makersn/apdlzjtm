using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StoreCartBiz
    {
        #region InsertCart - 장바구니 담기
        /// <summary>
        /// InsertCart - 장바구니 담기
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int InsertCart(StoreCartT data)
        {
            // 장바구니에 미 주문 상품이 있는지 체크
            List<StoreCartInfo> storeCartList = new List<StoreCartInfo>();
            storeCartList = new StoreCartDac().GetStoreCartByMemberNo(data.MemberNo);
            int cartGoodsCnt = storeCartList.Count;
            int result = 0;

            // 미 주문 상품이 없으면 장바구니번호 생성
            if (cartGoodsCnt == 0)
            {
                data.CartNo = new StoreCartDac().GetCreateCartNo();
            }
            else
            {
                data.CartNo = storeCartList[0].CART_NO;
            }

            result = new StoreCartDac().InsertStoreCart(data);

            return result;
        }
        #endregion

        #region GetStoreCartListByMemberNo - 장바구니 리스트 Get
        /// <summary>
        /// GetStoreCartListByMemberNo - 장바구니 리스트 Get
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        public List<StoreCartInfo> GetStoreCartListByMemberNo(int memberNo)
        {
            List<StoreCartInfo> data = new List<StoreCartInfo>();

            data = new StoreCartDac().GetStoreCartByMemberNo(memberNo);

            return data;
        }
        #endregion

        #region DeleteCartByProductDetailNo - 장바구니 삭제
        /// <summary>
        /// 장바구니 삭제
        /// </summary>
        /// <param name="memberNo"></param>
        /// <param name="productDetailNo"></param>
        /// <returns></returns>
        public int DeleteCartByCondition(int memberNo, Int64 productDetailNo)
        {
            return new StoreCartDac().DeleteCartByCondition(memberNo, productDetailNo);
        }
        #endregion

        #region 장바구니 수정
        #endregion
    }

}
