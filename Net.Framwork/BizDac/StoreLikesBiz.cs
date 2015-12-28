using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StoreLikesBiz
    {
        public List<StoreLikesT> getAllStorePrinter() {
            return new StoreLikesDac().SelectAllStoreLikes();
        }
        public StoreLikesT getStoreLikesById (int memberNo){
					return new StoreLikesDac().SelectStoreLikesTById(memberNo);
        }
        public int add(StoreLikesT StoreLikes)
        {
            return new StoreLikesDac().InsertStoreLikes(StoreLikes);
        }
        public int upd(StoreLikesT StoreLikes)
        {
            return new StoreLikesDac().UpdateStoreLikes(StoreLikes);
        }

				public int del(StoreLikesT StoreLikes)
				{
					return new StoreLikesDac().DeleteStoreLikes(StoreLikes);
				}

				public int set(StoreLikesT StoreLikes)
				{
					StoreLikesT exist = new StoreLikesDac().SelectLikesByProductNoAndMemberNo(StoreLikes.ProductNo, StoreLikes.MemberNo);
					if (exist != null)
					{
						return new StoreLikesDac().DeleteStoreLikes(StoreLikes);
					}
					else
					{
						return new StoreLikesDac().InsertStoreLikes(StoreLikes);
					}
					
				}

				public int countLikesByProductNo(int productNo)
				{
					return new StoreLikesDac().SelectStoreLikesTByProductNo(productNo);
				}


				public List<StoreLikesT> getLikedProductsByMemberNo(int memberNo)
				{
					return new StoreLikesDac().SelectLikesByProductNoAndMemberNo(memberNo);
				}
		}
}
