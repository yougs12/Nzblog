using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NZBlog.Core.Cache
{
    public interface ICache
    {
        void SetCache(string key, object obj);

        T GetCache<T>(string key);

        void Remove(string key);

        void RemoveAll();
    }
}
