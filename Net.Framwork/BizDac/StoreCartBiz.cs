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
            List<StoreCartT> storeCartList = new List<StoreCartT>();
            storeCartList = new StoreCartDac().GetStoreCartByMemberNo(data.MEMBER_NO);
            int cartGoodsCnt = storeCartList.Count;
            int result = 0;

            // 미 주문 상품이 없으면 장바구니번호 생성
            if (cartGoodsCnt == 0)
            {
                data.CART_NO = new StoreCartDac().GetCreateCartNo();
            }

            return result;
        }
        #endregion

        #region GetStoreCartListByMemberNo - 카트리스트 Get
        /// <summary>
        /// GetStoreCartListByMemberNo - 카트리스트 Get
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        public List<StoreCartT> GetStoreCartListByMemberNo(int memberNo)
        {
            List<StoreCartT> data = new List<StoreCartT>();

            data = new StoreCartDac().GetStoreCartByMemberNo(memberNo);

            return data;
        }
        #endregion
    }

}
