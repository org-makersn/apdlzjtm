using Net.Framework.Helper;
using Net.Framework.StoreModel;
using System.Collections.Generic;
using System.Linq;

namespace Net.Framwork.BizDac
{
    public class CommonCodeDac
    {
        private ISimpleRepository<CommonCodeT> _repository = new SimpleRepository<CommonCodeT>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IList<CommonCodeT> GetCommonCode(string group, string type)
        {
            //IList<CommonCodeT> codes = new List<CommonCodeT>();
            return _repository.Get(m => m.CODE_GROUP == group && m.CODE_TYPE == type).OrderBy(m => m.NO).ToList();
        }


    }
}
