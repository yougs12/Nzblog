using NZBlog.Entity;
using NZBlog.Factory;
using NZBlog.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NZBlog.Provider
{
    public class LableProvider
    {
        private ILables _Lables = new IFactory().CreateLables();

        public List<Lables> GetPageList(LablesParam param, int pageIndex, int pageSize, out int total)
        {
            return _Lables.GetList(param, pageIndex, pageSize, out total);
        }

        public Lables GetLables(int lableId)
        {
            return _Lables.GetModel(lableId);
        }

        public List<Lables> GetNameList(int[] blogIds)
        {
            return _Lables.GetNameList(blogIds);
        }

        public int AddLables(Lables model)
        {
            return _Lables.Insert(model);
        }

        public bool UpdateLables(Lables model)
        {
            return _Lables.Update(model);
        }

        public bool DeleteLables(int id)
        {
            return _Lables.Delete(id);
        }
        public void AddLables(Lables[] lables)
        {
            _Lables.AddLables(lables);
        }
    }
}
