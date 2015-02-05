using NZBlog.Entity;
using NZBlog.Factory;
using NZBlog.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NZBlog.Provider
{
    public class ZInfoMationProvider
    {
        private readonly IZInfoMation _zInfoMation = new IFactory().CreateZInfoMation();

        public List<ZInfoMation> GetPageList(ZInfoMationParam param, int pageIndex, int pageSize, out int total)
        {
            return _zInfoMation.GetList(param, pageIndex, pageSize, out total);
        }

        public ZInfoMation GetZInfoMation(int id)
        {
            return _zInfoMation.GetModel(id);
        }

        public int AddZInfoMation(ZInfoMation model)
        {
            return _zInfoMation.Insert(model);
        }

        public bool UpdateZInfoMation(ZInfoMation model)
        {
            return _zInfoMation.Update(model);
        }

        public bool DeleteZInfoMation(int zid)
        {
            return _zInfoMation.Delete(zid);
        }
    }
}
