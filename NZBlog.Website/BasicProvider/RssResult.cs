using NZBlog.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NZBlog.Website.BasicProvider
{
    public class RssResult : ActionResult
    {
        public Encoding ContentEncoding { get; set; }

        public RssEntity Data { get; set; }

        public RssResult()
        {
        }

        public RssResult(Encoding encode)
        {
            ContentEncoding = encode;
        }

        public RssResult(RssEntity data, Encoding encode)
        {
            Data = data;
            ContentEncoding = encode;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "text/xml";
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data != null)
            {
                response.Write(Data.ToXmlString());
            }
        }
    }
}