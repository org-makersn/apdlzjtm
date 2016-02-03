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
    public class StorePaymentDac : DacBase
    {
        #region 전역변수
        private static StoreContext dbContext;
        #endregion

        #region InsertStorePaymentHistory - 결제이력 저장
        /// <summary>
        /// 결제이력 저장
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int InsertStorePaymentHistory(StorePaymentHistoryT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");
            int ret = 0;
            using (dbContext = new StoreContext())
            {
                dbContext.StorePaymentHistoryT.Add(data);
                ret = dbContext.SaveChanges();
            }
            return ret;
        }
        #endregion


    }
}
