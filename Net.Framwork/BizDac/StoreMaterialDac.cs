using Net.Framework;
using Net.Framework.StoreModel;
using Net.Framework.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Framwork.BizDac
{
    public class StoreMaterialDac
    {
        ISimpleRepository<StoreMaterialT> _repository = new SimpleRepository<StoreMaterialT>();
        
        /// <summary>
        /// select multi data
        /// </summary>
        /// <returns></returns>
        public List<StoreMaterialT> GetAllStoreMaterial()
        {
            return _repository.GetAll().ToList();
        }

        /// <summary>
        /// select one StoreMaterial By Id
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public StoreMaterialT GetStoreMaterialById(int no)
        {
            if (no <= 0) throw new ArgumentNullException("The expected Segment NO.");

            return _repository.First(m => m.NO == no);
        }

        /// <summary>
        /// Insert StoreMaterial
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool AddStoreMaterial(StoreMaterialT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");

            return _repository.Insert(data);
        }

        /// <summary>
        /// Update StoreMaterial
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool UpdateStoreMaterial(StoreMaterialT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");

            return _repository.Update(data);
        }
    }
}
