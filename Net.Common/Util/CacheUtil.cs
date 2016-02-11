using System;
using System.Collections;
using System.Web;

namespace Net.Common.Util
{
    public class CacheUtil
    {
        /// <summary>  
        /// 
        /// </summary>  
        /// <param name="CacheKey"></param>  
        /// <returns></returns>y  
        public static object GetCache(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[CacheKey];
        }

        /// <summary>  
        /// 
        /// </summary>  
        /// <param name="CacheKey"></param>  
        /// <param name="objObject"></param>  
        public static void SetCache(string CacheKey, object objObject)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject);
        }


        /// <summary>  
        /// 
        /// </summary>  
        /// <param name="CacheKey"></param>  
        /// <param name="objObject"></param>  
        public static void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, null, absoluteExpiration, slidingExpiration);
        }

        /// <summary>  
        ///  
        /// </summary>  
        /// <param name="key"></param>  
        public static void RemoveKeyCache(string CacheKey)
        {
            try
            {
                System.Web.Caching.Cache objCache = HttpRuntime.Cache;
                objCache.Remove(CacheKey);
            }
            catch { }
        }

        /// <summary>  
        ///  
        /// </summary>  
        public static void RemoveAllCache()
        {
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            if (_cache.Count > 0)
            {
                ArrayList arr = new ArrayList();
                while (CacheEnum.MoveNext())
                {
                    arr.Add(CacheEnum.Key);
                }
                foreach (string key in arr)
                {
                    _cache.Remove(key);
                }
            }
        }

        /// <summary>  
        ///   
        /// </summary>  
        /// <returns></returns>   
        public static ArrayList ShowAllCache()
        {
            ArrayList arr = new ArrayList();
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            if (_cache.Count > 0)
            {
                IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
                while (CacheEnum.MoveNext())
                {
                    arr.Add(CacheEnum.Key);
                }
            }
            return arr;
        }
    }
}