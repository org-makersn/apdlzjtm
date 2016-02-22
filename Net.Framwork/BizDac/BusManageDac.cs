using Net.Framework.Helper;
using Net.Framework.Entity;
using Net.SqlTools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Net.Framework.BizDac
{
    public class BusManageDac : DacBase
    {
        private ISimpleRepository<BusApplySchool> _makerbusApplyRepo = new SimpleRepository<BusApplySchool>();

        #region AddApplyMakerbus - 메이커버스 신청서 저장
        /// <summary>
        /// 메이커버스 신청서 저장
        /// </summary>
        /// <param name="banner"></param>
        /// <returns></returns>
        internal int AddApplyMakerbus(BusApplySchool data)
        {
            int identity = 0;
            bool ret = _makerbusApplyRepo.Insert(data);

            if (ret)
            {
                identity = data.NO;
            }
            return identity;
        }
        #endregion

        #region GetApplyMakerbusList - 메이커버스 전체 리스트
        /// <summary>
        /// 메이커버스 전체 리스트
        /// </summary>
        /// <returns></returns>
        internal List<BusApplySchool> GetMakerbusList()
        {
            IEnumerable<BusApplySchool> state = new List<BusApplySchool>();

            state = _makerbusApplyRepo.GetAll();

            return state.ToList();
        }
        #endregion
    }
}
