﻿using Net.Framework.Helper;
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
        private ISimpleRepository<BusFaqT> _makerbusFaqRepo = new SimpleRepository<BusFaqT>();
        private ISimpleRepository<BusPartnershipQnaT> _makerbusPartnershipQnaRepo = new SimpleRepository<BusPartnershipQnaT>();
        private ISimpleRepository<BusPartnerT> _makersPartnerRepo = new SimpleRepository<BusPartnerT>();
        private ISimpleRepository<BusHistory> _historyRepo = new SimpleRepository<BusHistory>();
        private ISimpleRepository<BusBlog> _blogRepo = new SimpleRepository<BusBlog>();

        #region AddApplyMakerbus - 메이커버스 신청서 저장
        /// <summary>
        /// 메이커버스 신청서 저장
        /// </summary>
        /// <param name="banner"></param>
        /// <returns></returns>
        public int AddApplyMakerbus(BusApplySchoolT data)
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
        public List<BusApplySchoolT> GetMakerbusList()
        {
            IEnumerable<BusApplySchoolT> state = new List<BusApplySchoolT>();

            state = _makerbusApplyRepo.GetAll();

            return state.ToList();
        }
        #endregion

        #region 진행현황 관리
        /// <summary>
        /// 
        /// </summary>
        /// <param name="useYn"></param>
        /// <returns></returns>
        public IList<BusHistory> getBusHistoryListByUseYn(string useYn)
        {
            var state = _historyRepo.Get(m => m.USE_YN == useYn);
            return state == null ? new List<BusHistory>() : state.OrderByDescending(m => m.NO).ToList();
        }

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
        /// <param name="useYn"></param>
        /// <returns></returns>
        public IList<BusBlog> GetBusBlogListByUseYn(string useYn)
        {
            var state = _blogRepo.Get(m => m.USE_YN == useYn);
            return state == null ? new List<BusBlog>() : state.OrderByDescending(m => m.NO).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="useYn"></param>
        /// <param name="takeNum"></param>
        /// <returns></returns>
        public IList<BusBlog> GetBusBlogListByOption(string useYn, int fromPage, int toPage)
        {
            dbHelper = new SqlDbHelper(connectionString);
            string targetQuery = @"select 
	                                [NO]
	                                , BLOG_TITLE
	                                , BLOG_CONTENTS
	                                , THUMB_RENAME
	                                , VIEW_CNT
	                                , REG_DT
                                from (
	                                select
		                                [NO]
		                                , BLOG_TITLE
		                                , BLOG_CONTENTS
		                                , THUMB_RENAME
		                                , VIEW_CNT
		                                , REG_DT
		                                , ROW_NUMBER() OVER(ORDER BY REG_DT DESC) AS ROW_NUM 
	                                from BUS_BLOG with(nolock)
	                                where USE_YN = @USE_YN) as outQ
                                where ROW_NUM BETWEEN @FROM_PAGE AND @TO_PAGE";

            using (var cmd = new SqlCommand(targetQuery))
            {
                cmd.Parameters.Add("@USE_YN", SqlDbType.VarChar).Value = useYn;
                cmd.Parameters.Add("@FROM_PAGE", SqlDbType.Int).Value = fromPage;
                cmd.Parameters.Add("@TO_PAGE", SqlDbType.Int).Value = toPage;

                var state = dbHelper.ExecuteMultiple<BusBlog>(cmd);
                return state == null ? new List<BusBlog>() : state.OrderByDescending(m => m.NO).ToList();
            }
        }

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

        #region 통계
        /// <summary>
        /// 통계
        /// </summary>
        /// <returns></returns>
        public MakerBusState GetMakerbusState()
        {
            dbHelper = new SqlDbHelper(connectionString);
            string query = @"select
	                            (select sum(cnt) from (
	                            select count(distinct SCHOOL_NAME) as cnt from BUS_APPLY_SCHOOL with(nolock)
	                            where MAKERBUS_YN = 'Y'
	                            group by SCHOOL_NAME) as s) as SchoolCnt
	                            , sum(isnull(PARTICIPATION_COUNT, 0)) as StudentCnt
	                            , sum(isnull(MODELING_COUNT, 0)) as ModelingCnt
	                            , sum(isnull(SUPPORT_PRINTER_COUNT, 0)) as PrinterCnt
                            from BUS_APPLY_SCHOOL with(nolock) where MAKERBUS_YN = 'Y' ";

            using (var cmd = new SqlCommand(query))
            {
                return dbHelper.ExecuteSingle<MakerBusState>(cmd);
            }
        } 
        #endregion

        #region GetMakerbusQnaList - 메이커버스 문의사항 리스트
        /// <summary>
        /// 메이커버스 문의사항 리스트
        /// </summary>
        /// <returns></returns>
        public List<BusQnaT> GetMakerbusQnaList()
        {
            IEnumerable<BusQnaT> state = new List<BusQnaT>();

            state = _makerbusQnaRepo.GetAll();

            return state.ToList();
        }
        #endregion

        #region GetMakersbusFaqList - 메이커버스 자주묻는질문 리스트
        /// <summary>
        /// 메이커버스 자주묻는질문 리스트
        /// </summary>
        /// <returns></returns>
        public List<BusFaqT> GetMakersbusFaqList()
        {
            IEnumerable<BusFaqT> state = new List<BusFaqT>();

            state = _makerbusFaqRepo.GetAll();

            return state.ToList();
        }
        #endregion

        #region GetMakersbusPartnershipQnaList - 메이커버스 파트너쉽 문의사항 리스트
        /// <summary>
        /// 메이커버스 파트너쉽 문의사항 리스트
        /// </summary>
        /// <returns></returns>
        public List<BusPartnershipQnaT> GetMakersbusPartnershipQnaList()
        {
            IEnumerable<BusPartnershipQnaT> state = new List<BusPartnershipQnaT>();

            state = _makerbusPartnershipQnaRepo.GetAll();

            return state.ToList();
        }
        #endregion

        #region GetMakersPartnerList - 메이커스 파트너 리스트
        /// <summary>
        /// 메이커버스 파트너쉽 문의사항 리스트
        /// </summary>
        /// <returns></returns>
        public List<BusPartnerT> GetMakersPartnerList()
        {
            IEnumerable<BusPartnerT> state = new List<BusPartnerT>();

            state = _makersPartnerRepo.GetAll();

            return state.ToList();
        }
        #endregion

    }
}
