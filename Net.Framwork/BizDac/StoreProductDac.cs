using Net.Framework;
using Net.Framework.StoreModel;
using Net.Framework.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Framwork.BizDac
{
    public class StoreProductDac : DacBase
    {
        private ISimpleRepository<StoreProductT> _repository = new SimpleRepository<StoreProductT>();
        
        /// <summary>
        /// select multi data
        /// </summary>
        /// <returns></returns>
        internal IList<StoreProductT> SelectAllStoreProduct()
        {
            return _repository.GetAll().ToList();
        }

        /// <summary>
        /// select one StoreProduct By Id
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        internal StoreProductT SelectStoreProductTById(long no)
        {
            return _repository.First(m => m.NO == no);
        }

        /// <summary>
        /// Insert StoreProduct
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public long InsertStoreProduct(StoreProductT data)
        {
            long identity = 0;
            bool ret = _repository.Insert(data);
            if (ret)
            {
                identity = _repository.First(m => m.VAR_NO == data.VAR_NO).NO;
            }
            return identity;
        }

        /// <summary>
        /// Update StoreProduct
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal bool UpdateStoreProduct(StoreProductT data)
        {
return _repository.Update(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="certificateStatus"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        internal List<StoreProductT> SelectProductWithCertification(int certificateStatus,string query)
        {
            return _repository.Get(m => (m.CERTIFICATE_YN == certificateStatus) && m.NAME.Contains(query)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberNo"></param>
        /// <param name="codeNo"></param>
        /// <returns></returns>
        internal int SelectTotalCountByOption(int memberNo, int codeNo)
        {
            return _repository.QueryCount(m => m.CATEGORY_NO == codeNo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberNo"></param>
        /// <param name="codeNo"></param>
        /// <returns></returns>
        internal IList<StoreProductT> SelectProductsByOption(int memberNo, int codeNo, int fromIndex, int toIndex)
        {
            //페이징
            return _repository.Get(p => p.CATEGORY_NO == codeNo).ToList();
        }

        /// <summary>
        /// test
        /// </summary>
        /// <returns></returns>
        public IList<StoreProductExT> SelectProductTest()
        {
            string query = DacHelper.GetSqlCommand("StoreProduct.SelectProductList_S");

            IList<StoreProductExT> states = dbHelper.ExecuteMultiple<StoreProductExT>(query).ToList();

            using (var dbContext = new StoreContext())
            {
                IList<StoreProductExT> list = dbContext.Database.SqlQuery<StoreProductExT>(query).ToList();

                return list;
            }
        }
    }
}
