using Net.Framework.Helper;
using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Framework.BizDac
{
    public class StoreItemFileDac
    {
        private ISimpleRepository<StoreItemFileT> _itemFileRepo = new SimpleRepository<StoreItemFileT>();

        /// <summary>
        /// by temp
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        public IList<StoreItemFileT> GetItemFilesByTemp(string temp)
        {
            return _itemFileRepo.Get(m => m.Temp == temp && m.UseYn == "Y").ToList(); ;
        }

        /// <summary>
        /// by itemNo
        /// </summary>
        /// <param name="itemNo"></param>
        /// <returns></returns>
        public IList<StoreItemFileT> GetItemFileByItemNo(long itemNo)
        {
            return _itemFileRepo.Get(m => m.StoreItemNo == itemNo).ToList();
        }

        /// <summary>
        /// single by ItemNo
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public StoreItemFileT GetItemFileByNo(long no)
        {
            return _itemFileRepo.First(m => m.No == no);
        }

        /// <summary>
        /// insert
        /// </summary>
        /// <param name="storeItem"></param>
        /// <param name="paramDelNo"></param>
        /// <returns></returns>
        public long AddItemFile(StoreItemFileT data)
        {
            long identity = 0;
            bool ret = _itemFileRepo.Insert(data);

            if (ret)
            {
                identity = data.No;
            }
            return identity;
        }

        /// <summary>
        /// update
        /// </summary>
        /// <param name="storeItem"></param>
        /// <returns></returns>
        public bool UpdateItemFile(StoreItemFileT data)
        {
            return _itemFileRepo.Update(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strNo"></param>
        public void DeleteItemFiles(string strNo)
        {
            if (!string.IsNullOrEmpty(strNo))
            {
                string[] strArr = strNo.Split(',');
                IList<StoreItemFileT> storeFileList = new List<StoreItemFileT>();
                foreach (var str in strArr)
                {
                    _itemFileRepo.Delete(m => m.No == Convert.ToInt64(str));
                }
            }
        }
    }
}
