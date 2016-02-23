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
        private ISimpleRepository<BusApplySchoolT> _makerbusApplyRepo = new SimpleRepository<BusApplySchoolT>();
        private ISimpleRepository<BusQnaT> _makerbusQnaRepo = new SimpleRepository<BusQnaT>();
        private ISimpleRepository<BusHistory> _historyRepo = new SimpleRepository<BusHistory>();
        private ISimpleRepository<BusBlog> _blogRepo = new SimpleRepository<BusBlog>();

        #region AddApplyMakerbus - 메이커버스 신청서 저장
        /// <summary>
        /// 메이커버스 신청서 저장
        /// </summary>
        /// <param name="banner"></param>
        /// <returns></returns>
        internal int AddApplyMakerbus(BusApplySchoolT data)
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
        internal List<BusApplySchoolT> GetMakerbusList()
        {
            IEnumerable<BusApplySchoolT> state = new List<BusApplySchoolT>();

            state = _makerbusApplyRepo.GetAll();

            return state.ToList();
        }
        #endregion

        #region 진행현황 관리
        /// <summary>
        /// get all
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public IList<BusHistory> GetBusHistoryList()
        {
            var state = _historyRepo.GetAll();
            return state == null ? new List<BusHistory>() : state.OrderByDescending(m => m.NO).ToList();
        }

        /// <summary>
        /// get
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public BusHistory GetBusHistoryByNo(int no)
        {
            return _historyRepo.First(m => m.NO == no);
        }

        /// <summary>
        /// 진행현황 추가
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        public int AddHistory(BusHistory history)
        {
            int identity = 0;
            bool ret = _historyRepo.Insert(history);

            if (ret)
            {
                identity = history.NO;
            }
            return identity;
        }

        /// <summary>
        /// 진행현황 수정
        /// </summary>
        /// <param name="banner"></param>
        /// <returns></returns>
        public bool UpdateHistory(BusHistory history)
        {
            return _historyRepo.Update(history);
        } 
        #endregion

        #region 블로그 관리
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<BusBlog> GetBusBlogList()
        {
            var state = _blogRepo.GetAll();
            return state == null ? new List<BusBlog>() : state.OrderByDescending(m => m.NO).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public BusBlog GetBlogByNo(long no)
        {
            return _blogRepo.First(m => m.NO == no);
        }

        /// <summary>
        /// 블로그 추가
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        public long AddBlog(BusBlog blog)
        {
            long identity = 0;
            bool ret = _blogRepo.Insert(blog);

            if (ret)
            {
                identity = blog.NO;
            }
            return identity;
        }

        /// <summary>
        /// 블로그 수정
        /// </summary>
        /// <param name="banner"></param>
        /// <returns></returns>
        public bool UpdateBlog(BusBlog blog)
        {
            return _blogRepo.Update(blog);
        }
        #endregion

        #region AddMakerBusQna - 메이커버스 문의사항 저장
        /// <summary>
        /// 메이커버스 문의사항 저장
        /// </summary>
        /// <param name="banner"></param>
        /// <returns></returns>
        internal int AddMakerBusQna(BusQnaT data)
        {
            int identity = 0;
            bool ret = _makerbusQnaRepo.Insert(data);

            if (ret)
            {
                identity = data.NO;
            }
            return identity;
        }
        #endregion

    }
}
