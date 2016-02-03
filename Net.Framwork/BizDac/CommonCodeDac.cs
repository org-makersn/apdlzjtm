using Net.Framework.Helper;
using Net.Framework.StoreModel;
using System.Collections.Generic;
using System.Linq;

namespace Net.Framwork.BizDac
{
    public class CommonCodeDac
    {
        static IRepository<CommonCodeT> commoncode_ins = new Repository<CommonCodeT>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IList<CommonCodeT> GetCommonCode(string group, string type)
        {
            //IList<CommonCodeT> codes = new List<CommonCodeT>();
            return commoncode_ins.Get(m => m.CODE_GROUP == group && m.CODE_TYPE == type).OrderBy(m => m.NO).ToList();
        }


    }
}
