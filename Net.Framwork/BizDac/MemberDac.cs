﻿using Net.Framework;
using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class MemberDac
    {
        private static StoreContext dbContext;

        /// <summary>
        /// select multi data
        /// </summary>
        /// <returns></returns>
        internal List<MemberT> SelectAllMembers()
        {
            List<MemberT> members = null;
            using (dbContext = new StoreContext())
            {
                members = dbContext.MemberT.ToList();

                var data = (from m in dbContext.MemberT
                            join d in dbContext.DetailT
                            on m.MemberId equals d.MemberId into md
                            from x in md.DefaultIfEmpty()
                            select new
                            {
                                MemberId = m.MemberId,
                                MemberNm = m.MemberNm,
                                AppId = m.AppId,
                                RegDt = m.RegDt,
                                PhoneNumber = x.PhoneNumber
                            }).ToList();

                //List<CustomMemberT> facebooks = new JavaScriptSerializer().Deserialize<List<CustomMemberT>>(data);

                //members = dbContext.MemberT
                //    .Join(dbContext.DetailT, m => m.MemberId, d => d.MemberId, (m, d) => new { m, d })
                //    .Where(e => e.m.MemberId == e.d.MemberId).Take(10)
                //    .Select(s => new MemberT
                //    {
                //        MemberId = s.m.MemberId,
                //        MemberNm = s.m.MemberNm,
                //        AppId = s.m.AppId,
                //        RegDt = s.m.RegDt,
                //        PhoneNumber = s.d.PhoneNumber
                //    }).ToList();

                //string query = "SELECT m.Id as MemberId, m.Member_Nm as MemberNm, m.App_Id as AppId, m.Reg_Dt as RegDt, d.Phone_Number as PhoneNumber "
                //    + "FROM Member m, Detail d with(nolock) "
                //    + "WHERE m.Id = d.Member_Id "
                //    + "ORDER BY m.Reg_Dt";
                //IEnumerable<MemberT> data = dbContext.Database.SqlQuery<MemberT>(query);

                //members = data.ToList();
            }

            return members;
        }

        /// <summary>
        /// select single data
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        internal MemberT SelectMemberByMemberId(int memberId)
        {
            MemberT member = null;

            using (dbContext = new StoreContext())
            {
                member = dbContext.MemberT.Where(m => m.MemberId == memberId).FirstOrDefault();
            }

            return member;
        }

        /// <summary>
        /// insert data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int InsertMember(MemberT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");
            int ret = 0;
            using (dbContext = new StoreContext())
            {

            }
            return ret;
        }

        /// <summary>
        /// update data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int UpdateMemberByMemberId(MemberT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");

            int ret = 0;
            using (dbContext = new StoreContext())
            {
                var originalData = dbContext.MemberT.SingleOrDefault(m => m.MemberId == data.MemberId);
                if (originalData != null)
                {
                    try
                    {
                        originalData.MemberNm = data.MemberNm;
                        originalData.AppId = data.AppId;

                        dbContext.SaveChangesAsync();
                        ret = data.MemberId;
                    }
                    catch (Exception)
                    {
                        ret = -1;
                    }
                }
                else
                {
                    ret = -2;
                    throw new NullReferenceException("The expected original Segment data is not here.");
                }
            }
            return ret;
        }
    }
}
