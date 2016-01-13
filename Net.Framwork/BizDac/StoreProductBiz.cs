using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StoreProductBiz
    {
        /// <summary>
        /// 조회
        /// </summary>
        /// <returns></returns>
        public IList<StoreProductT> getAllStoreProduct() {
            return new StoreProductDac().SelectAllStoreProduct();
        }

        /// <summary>
        /// 조회
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public StoreProductT getStoreProductById (int no){
            return new StoreProductDac().SelectStoreProductTById(no);
        }

        /// <summary>
        /// 등록
        /// </summary>
        /// <param name="StoreProduct"></param>
        /// <returns></returns>
        public long addStoreProduct(StoreProductT StoreProduct)
        {
            return new StoreProductDac().InsertStoreProduct(StoreProduct);
        }

        /// <summary>
        /// 수정
        /// </summary>
        /// <param name="StoreProduct"></param>
        /// <returns></returns>
        public int setStoreProduct(StoreProductT StoreProduct)
        {
            return new StoreProductDac().UpdateStoreProduct(StoreProduct);
        }

        /// <summary>
        /// 조회
        /// </summary>
        /// <param name="certificateStatus"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<StoreProductT> searchProductWithCertification(int certificateStatus , string query)
        {
            return new StoreProductDac().SelectProductWithCertification(certificateStatus, query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberNo"></param>
        /// <param name="codeNo"></param>
        /// <returns></returns>
        public int getTotalCountByOption(int memberNo, int codeNo)
        {
            return new StoreProductDac().SelectTotalCountByOption(memberNo, codeNo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberNo"></param>
        /// <param name="codeNo"></param>
        /// <returns></returns>
        public IList<StoreProductT> getProductsByOption(int memberNo, int codeNo)
        {
            return new StoreProductDac().SelectProductsByOption(memberNo, codeNo);
        }
    }
}
