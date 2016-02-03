using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StorePaymentBiz
    {
        #region InsertStorePaymentHistory - 결제이력 저장
        /// <summary>
        /// 결제이력 저장
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int InsertStorePaymentHistory(StorePaymentHistoryT data)
        {
            return new StorePaymentDac().InsertStorePaymentHistory(data);
        }
        #endregion
    }
}
