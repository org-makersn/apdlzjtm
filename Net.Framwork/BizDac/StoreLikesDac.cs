using Net.Framework;
using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StoreLikesDac
    {

        private static StoreContext dbContext;

        /// <summary>
        /// select multi data
        /// </summary>
        /// <returns></returns>
        internal List<StoreLikesT> SelectAllStoreLikes()
        {

            List<StoreLikesT> printers = null;
            using (dbContext = new StoreContext())
            {
                printers = dbContext.StoreLikesT.ToList();
            }
            return printers;
        }

        /// <summary>
        /// select one likes By Id
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        internal StoreLikesT SelectStoreLikesTById(int no)
        {
            StoreLikesT printer = null;

            using (dbContext = new StoreContext())
            {
                printer = dbContext.StoreLikesT.Where(m => m.NO == no).FirstOrDefault();
            }

            return printer;
        }

        /// <summary>
        /// Insert likes
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int InsertStoreLikes(StoreLikesT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");
            int ret = 0;
            using (dbContext = new StoreContext())
            {
                dbContext.StoreLikesT.Add(data);
                //dbContext.SaveChanges();
                ret = dbContext.SaveChanges();
            }
            return ret;
        }

        /// <summary>
        /// Update likes
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int UpdateStoreLikes(StoreLikesT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");

            int ret = 0;
            using (dbContext = new StoreContext())
            {
                StoreLikesT originData = dbContext.StoreLikesT.SingleOrDefault(s => s.NO == data.NO);
                if (originData != null)
                {
                    try
                    {
                        originData.PRODUCT_NO = data.PRODUCT_NO;
                        dbContext.StoreLikesT.Attach(originData);
                        dbContext.Entry<StoreLikesT>(originData).State = System.Data.Entity.EntityState.Modified;
                        dbContext.SaveChanges();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int DeleteStoreLikes(StoreLikesT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");

            int ret = 0;
            using (dbContext = new StoreContext())
            {
                //StoreLikesT originData = dbContext.StoreLikesT.Where(s => s.No == data.No).SingleOrDefault();
                StoreLikesT originData = dbContext.StoreLikesT.SingleOrDefault(s => s.PRODUCT_NO == data.PRODUCT_NO && s.MEMBER_NO == data.MEMBER_NO);
                if (originData != null)
                {
                    try
                    {
                        originData.PRODUCT_NO = data.PRODUCT_NO;
                        dbContext.StoreLikesT.Remove(originData);
                        ret = dbContext.SaveChanges();
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

        /// <summary>
        /// 상품번호로 좋아요 갯수 출력
        /// </summary>
        /// <param name="productNo">상품번호</param>
        /// <returns>갯수</returns>
        internal int SelectStoreLikesTByProductNo(int productNo)
        {
            int countLikes = 0;

            using (dbContext = new StoreContext())
            {
                countLikes = dbContext.StoreLikesT.Where(m => m.PRODUCT_NO == productNo).Count();
            }

            return countLikes;
        }

        /// <summary>
        /// 상품번호, 회원번호로 좋아요 유무 체크
        /// </summary>
        /// <param name="productNo">상품번호</param>
        /// <param name="memberNo">회원번호</param>
        /// <returns></returns>
        internal StoreLikesT SelectLikesByProductNoAndMemberNo(int productNo, int memberNo)
        {
            StoreLikesT printer = null;

            using (dbContext = new StoreContext())
            {
                printer = dbContext.StoreLikesT.Where(m => m.PRODUCT_NO == productNo && m.MEMBER_NO == memberNo).FirstOrDefault();
            }

            return printer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        internal List<StoreLikesT> SelectLikesByProductNoAndMemberNo(int memberNo)
        {
            List<StoreLikesT> storedLikesList = null;

            using (dbContext = new StoreContext())
            {
                storedLikesList = dbContext.StoreLikesT.Where(m => m.MEMBER_NO == memberNo).ToList();
            }

            return storedLikesList;
        }
    }
}
