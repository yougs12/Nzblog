using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NZBlog.Core.Cache
{
    public class NZCache : ICache
    {
        private static Dictionary<string, object> cacheDic = new Dictionary<string, object>();
        private static object lockObj = new object();
        public void SetCache(string key, object obj)
        {
            if (cacheDic[key] == null)
            {
                cacheDic[key] = obj;
            }
        }

        public T GetCache<T>(string key)
        {
            return (T)cacheDic[key];
        }

        public void Remove(string key)
        {
            if (cacheDic[key] != null)
            {
                cacheDic.Remove(key);
            }
        }

        public void RemoveAll()
        {
            cacheDic = new Dictionary<string, object>();
        }
    }
}
