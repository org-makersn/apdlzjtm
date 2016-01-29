using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StoreShippingBiz
    {
        #region InsertShippingInfo - 배송지 저장
        /// <summary>
        /// 배송지 저장
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Int64 InsertShippingInfo(StoreShippingAddrT data)
        {
            return new StoreShippingDac().InsertShippingInfo(data);
        }
        #endregion

        #region GetShippingAddrListByMemberNo - 배송주소록
        /// <summary>
        /// 배송주소록
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        public List<StoreShippingAddrT> GetShippingAddrListByMemberNo(int memberNo)
        {
            return new StoreShippingDac().GetShippingAddrListByMemberNo(memberNo);
        }
        #endregion
    }
}
