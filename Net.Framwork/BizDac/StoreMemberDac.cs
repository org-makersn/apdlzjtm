using Net.Framework;
using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StoreMemberDac
    {

        private static StoreContext dbContext;
        
        /// <summary>
        /// select multi data
        /// </summary>
        /// <returns></returns>
        internal List<StoreMemberT> SelectAllStoreMember()
        { 
            
            List<StoreMemberT> printers = null;
            using (dbContext = new StoreContext())
            {
                printers = dbContext.StoreMemberT.ToList();
            }
            return printers;
        }

        /// <summary>
        /// select one StoreMember By Id
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        internal StoreMemberT SelectStoreMemberTById(int no)
        {
            StoreMemberT printer = null;

            using (dbContext = new StoreContext())
            {
                printer = dbContext.StoreMemberT.Where(m => m.No == no).FirstOrDefault();
            }

            return printer;
        }

        /// <summary>
        /// Insert StoreMember
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int InsertStoreMember(StoreMemberT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");
            int ret = 0;
            using (dbContext = new StoreContext())
            {
                dbContext.StoreMemberT.Add(data);
                dbContext.SaveChangesAsync();
            }
            return ret;
        }

        /// <summary>
        /// Update StoreMember
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int UpdateStoreMember(StoreMemberT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");

            int ret = 0;
            using (dbContext = new StoreContext())
            {
                
                if (dbContext.StoreMemberT.SingleOrDefault(m => m.No == data.No) != null)
                {
                    try
                    {
                        dbContext.StoreMemberT.Attach(data);
                        dbContext.Entry<StoreMemberT>(data).State = System.Data.Entity.EntityState.Modified;
                        dbContext.SaveChangesAsync();
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
