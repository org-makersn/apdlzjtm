using System;
using Net.Framework;
using Net.Framework.Helper;
using Net.Framework.StoreModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Net.SqlTools;

namespace Net.Framwork.BizDac
{
    public class StoreShippingDac : DacBase
    {
        #region 전역변수
        private static StoreContext dbContext;
        #endregion

        #region InsertShippingInfo - 배송지 저장
        /// <summary>
        /// 배송지 저장
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal Int64 InsertShippingInfo(StoreShippingAddrT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");
            using (dbContext = new StoreContext())
            {
                dbContext.StoreShippingAddrT.Add(data);
                dbContext.SaveChanges();
                dbContext.Entry(data).GetDatabaseValues();
            }

            return data.No;
        }
        #endregion

        #region GetShippingAddrListByMemberNo - 배송주소록
        /// <summary>
        /// 배송주소록
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        internal List<StoreShippingAddrT> GetShippingAddrListByMemberNo(int memberNo)
        {
            List<StoreShippingAddrT> list = null;
            using (dbContext = new StoreContext())
            {
                list = dbContext.StoreShippingAddrT.Where(m => m.MemberNo == memberNo).ToList();
            }
            return list;
        }
        #endregion
    }
}
