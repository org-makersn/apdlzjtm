using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Net.Framework.Helper;
using Net.Framework.Entity;

namespace Net.Framework.BizDac
{
    public class BusManageBiz
    {
        /// <summary>
        /// 메이커버스 신청 저장
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int AddApplyMakerBus(BusApplySchoolT data)
        {
            return new BusManageDac().AddApplyMakerbus(data);
        }

        /// <summary>
        /// 메이커버스 문의사항 저장
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int AddMakerBusQna(BusQnaT data)
        {
            return new BusManageDac().AddMakerBusQna(data);
        }

        /// <summary>
        /// 메이커버스 파트너쉽 문의사항 저장
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int AddMakerBusPartnershipQna(BusPartnershipQnaT data)
        {
            return new BusManageDac().AddMakerBusPartnershipQna(data);
        }
    }
}
