using Net.Framework.Helper;
using Net.Framework.StoreModel;
using Net.SqlTools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace Net.Framework.BizDac
{
    public class StoreItemDac : DacBase
    {
        private ISimpleRepository<StoreItemT> _itemRepo = new SimpleRepository<StoreItemT>();
        
        /// <summary>
        /// 아이템 조회
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public StoreItemT GetItemByNo(long no)
        {
            return _itemRepo.First(m => m.No == no);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemNo"></param>
        /// <param name="visitorNo"></param>
        /// <returns></returns>
        public StoreItemDetailT GetItemDetailByItemNo(long itemNo, int visitorNo)
        {
            dbHelper = new SqlDbHelper(connectionString);

            string query = DacHelper.GetSqlCommand("StoreItemDac.GetItemDetailByItemNo");
            using (var cmd = new SqlCommand(query))
            {
                cmd.Parameters.Add("@STORE_ITEM_NO", SqlDbType.BigInt).Value = itemNo;
                cmd.Parameters.Add("@VISITOR_NO", SqlDbType.Int).Value = visitorNo;

                return dbHelper.ExecuteSingle<StoreItemDetailT>(cmd);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberNo"></param>
        /// <param name="codeNo"></param>
        /// <returns></returns>
        public int GetTotalCountByOption(int memberNo, int codeNo, string gbn)
        {
            dbHelper = new SqlDbHelper(connectionString);

            string targetCntQuery = string.Empty;
            string whereQuery = string.Empty;
            string addQuery = string.Empty;

            if (gbn == "FEATURED")
            {
                whereQuery += " AND A.FEATURED_YN='Y' ";
            }

            if (codeNo != 0)
            {
                whereQuery += " AND A.CODE_NO = @CODE_NO ";
            }

            targetCntQuery += @"SELECT COUNT(1) 
                                FROM STORE_ITEM A WITH(NOLOCK)  
                                LEFT OUTER JOIN STORE_MEMBER B WITH(NOLOCK) ON B.[NO] = A.STORE_MEMBER_NO  
                                WHERE (B.DEL_YN != 'Y' OR B.DEL_YN IS NULL) AND A.USE_YN = 'Y' "
                                + whereQuery;

            using (var cmd = new SqlCommand(targetCntQuery))
            {
                if (codeNo != 0)
                {
                    cmd.Parameters.Add("@CODE_NO", SqlDbType.Int).Value = codeNo;
                }
                return dbHelper.ExecuteScalar<int>(cmd);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberNo"></param>
        /// <param name="codeNo"></param>
        /// <returns></returns>
        public IList<StoreItemDetailT> GetStoreItemsByOption(int memberNo, int codeNo, string gbn, int fromIndex, int toIndex)
        {

            string targetOptQuery = string.Empty;
            string rowNumQuery = string.Empty;
            string whereQuery = string.Empty;

            switch (gbn.ToUpper())
            {
                case "LATEST":
                    if (codeNo != 0 && codeNo < 10000) { whereQuery += " AND A.CODE_NO like @CODE_LIKE_NO "; }
                    else if (codeNo != 0 && codeNo > 10000) { whereQuery += " AND A.CODE_NO = @CODE_NO "; }
                    rowNumQuery = ", ROW_NUMBER() OVER(ORDER BY REG_DT DESC) AS ROW_NUM ";
                    break;
                case "POPULAR":
                    rowNumQuery = ", ROW_NUMBER() OVER(ORDER BY POP DESC) AS ROW_NUM ";
                    break;
                case "FEATURED":
                    rowNumQuery = ", ROW_NUMBER() OVER(ORDER BY FEATURED_PRIORITY DESC, FEATURED_DT DESC) AS ROW_NUM ";
                    break;
                default:
                    if (codeNo != 0 && codeNo < 10000) { whereQuery += " AND A.CODE_NO  like @codeLikeNo "; }
                    else if (codeNo != 0 && codeNo > 10000) { whereQuery += " AND A.CODE_NO = @codeNo "; }

                    rowNumQuery = ", ROW_NUMBER() OVER(ORDER BY REG_DT DESC) AS ROW_NUM ";
                    break;
            }

            if (gbn == "FEATURED")
            {
                whereQuery += " AND A.FEATURED_YN='Y' ";
            }

            if (codeNo != 0)
            {
                whereQuery += " AND A.CODE_NO = @CODE_NO ";
            }

            targetOptQuery += @" SELECT NO, ItemName, StoreMemberNo, CodeNo, BasePrice, ViewCnt, StoreName, MainImgName, ROW_NUM FROM
                                (SELECT InQ.NO, InQ.ItemName, InQ.StoreMemberNo, InQ.CodeNo, InQ.BasePrice, InQ.ViewCnt, InQ.StoreName, InQ.MainImgName "
                                + rowNumQuery + @" FROM 
                                (SELECT  
	                                A.NO
	                                , A.ITEM_NAME as ItemName
	                                , A.STORE_MEMBER_NO as StoreMemberNo
	                                , A.CODE_NO as CodeNo
	                                , A.BASE_PRICE as BasePrice
	                                , A.VIEW_CNT as ViewCnt
                                    , A.REG_DT
									, A.FEATURED_DT
                                    , A.FEATURED_PRIORITY 
	                                , B.STORE_NAME AS StoreName
                                    , C.RENAME as MainImgName
	                                --, (SELECT count(0) FROM STORE_COMMENT B with(nolock) WHERE A.NO = B.STORE_ITEM_NO) AS COMMENT_CNT
	                                --, (SELECT count(1) FROM STORE_LIKES B with(nolock) WHERE A.NO = B.STORE_ITEM_NO) AS LIKE_CNT
	                                --, (SELECT count(1) FROM STORE_LIKES B with(nolock) WHERE A.NO = B.STORE_ITEM_NO AND B.MEMBER_NO = @MEMBER_NO ) AS IS_LIKES
	                                
                                FROM STORE_ITEM A with(nolock)  
                                LEFT JOIN STORE_MEMBER B with(nolock) on A.STORE_MEMBER_NO = B.[NO]
                                INNER JOIN STORE_ITEM_FILE C with(nolock) on A.MAIN_IMG = C.[NO]  
                                WHERE (B.DEL_YN != 'Y' OR B.DEL_YN IS NULL) AND A.USE_YN = 'Y' "
                                + whereQuery + " ) InQ ) OutQ ";

            targetOptQuery += @" WHERE ROW_NUM BETWEEN @FROM_PAGE AND @TO_PAGE";

            using (var cmd = new SqlCommand(targetOptQuery))
            {
                cmd.Parameters.Add("@CODE_NO", SqlDbType.Int).Value = codeNo;
                cmd.Parameters.Add("@FROM_PAGE", SqlDbType.Int).Value = fromIndex;
                cmd.Parameters.Add("@TO_PAGE", SqlDbType.Int).Value = toIndex;

                var state = dbHelper.ExecuteMultiple<StoreItemDetailT>(cmd);

                return state != null ? state.ToList() : null;
            }
        }

        /// <summary>
        /// insert
        /// </summary>
        /// <param name="data"></param>
        /// <param name="paramDelNo"></param>
        /// <returns></returns>
        public long AddItem(StoreItemT data)
        {
            long identity = 0;
            bool ret = _itemRepo.Insert(data);
            
            if (ret)
            {
                identity = data.No;
            }
            return identity;
        }

        /// <summary>
        /// update
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool UpdateItem(StoreItemT data)
        {
            return _itemRepo.Update(data);
        }

    }
}
