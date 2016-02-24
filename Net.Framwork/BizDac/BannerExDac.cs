using Net.Framework.Entity;
using Net.Framework.Helper;
using Net.SqlTools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Net.Framework.BizDac
{
    public class BannerExDac : DacBase
    {
        private ISimpleRepository<BannerExT> _bannerRepo = new SimpleRepository<BannerExT>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IList<BannerExT> GetAllBannerList(int type)
        {
            IEnumerable<BannerExT> state = null;
            if (type > 0)
            {
                state = _bannerRepo.Get(m => m.Type == type && m.PublishYn == "y");
            }
            else
            {
                state = _bannerRepo.GetAll();
            }

            return state == null ? new List<BannerExT>() : state.OrderByDescending(m => m.No).ToList();
        }

        /// <summary>
        /// 배너 검색 type = 0 전체, 1 디자인, 2 프린팅, 3 스토어, 4 메이커버스
        /// </summary>
        /// <param name="sfl"></param>
        /// <param name="query"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IList<BannerExT> GetBannerListByQuery(string sfl, string query, int type)
        {
            dbHelper = new SqlDbHelper(connectionString);

            string[] queryList = query.Split(' ');
            string searchQuery = @"select 
	                                    NO, TYPE, TITLE
                                        , PUBLISH_YN as PublishYn
                                        , OPENER_YN as OpenerYn
                                        , LINK, SOURCE, TERM, IMAGE, PRIORITY
                                        , REG_DT as RegDt
                                        , REG_ID as RegId 
                                    from BANNER with(nolock) 
                                    where 1=1 ";
            if (type > 0)
            {
                searchQuery += " AND TYPE = @TYPE ";
            }
            if (sfl == "title")
            {
                searchQuery += " AND (";
                for (int i = 0; i < queryList.Length; i++)
                {
                    if (i == 0) { searchQuery += @" TITLE LIKE @P" + i; }
                    else { searchQuery += " OR TITLE LIKE @P" + i; }
                }
                searchQuery += ")";
            }

            using (var cmd = new SqlCommand(searchQuery))
            {
                if (type > 0)
                {
                    cmd.Parameters.Add("@TYPE", SqlDbType.Int).Value = type;
                }

                for (int i = 0; i < queryList.Length; i++)
                {
                    cmd.Parameters.Add("@P" + i, SqlDbType.VarChar).Value = "%" + queryList[i] + "%";
                }

                var state = dbHelper.ExecuteMultiple<BannerExT>(cmd);

                return state != null ? state.ToList() : new List<BannerExT>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bannerNo"></param>
        /// <returns></returns>
        public BannerExT GetBannerByNo(int bannerNo)
        {
            return _bannerRepo.First(m => m.No == bannerNo);
        }

        /// <summary>
        /// 배너 추가
        /// </summary>
        /// <param name="banner"></param>
        /// <returns></returns>
        public int AddBanner(BannerExT banner)
        {
            int identity = 0;
            bool ret = _bannerRepo.Insert(banner);

            if (ret)
            {
                identity = banner.No;
            }
            return identity;
        }

        /// <summary>
        /// 배너 수정
        /// </summary>
        /// <param name="banner"></param>
        /// <returns></returns>
        public bool UpdateBanner(BannerExT banner)
        {
            return _bannerRepo.Update(banner);
        }

        /// <summary>
        /// 배너 삭제
        /// </summary>
        /// <param name="bannerNo"></param>
        /// <returns></returns>
        public bool DeleteBanner(int bannerNo)
        {
            //BannerExT banner = _bannerRepo.First(m => m.No == bannerNo);
            return _bannerRepo.Delete(bannerNo);
        }
    }
}
