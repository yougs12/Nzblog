using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NZBlog.Website.Models
{
    public class RssEntity
    {
        public string Title { get; set; }

        public string Link { get; set; }

        public string Description { get; set; }

        public string Language { get; set; }

        public string Copyright { get; set; }

        public string WebMaster { get; set; }

        public string Generator { get; set; }

        public RssImage Image { get; set; }

        public IList<RssItem> Items { get; set; }

        public RssEntity()
        {
            Title = string.Empty;
            Link = string.Empty;
            Description = string.Empty;
            Language = "zh-cn";
            Copyright = string.Empty;
            WebMaster = string.Empty;
            Generator = string.Empty;
            Image = null;
            Items = new List<RssItem>();
        }
    }

    public class RssImage
    {
        public string Title { get; set; }

        public string Url { get; set; }

        public string Link { get; set; }

        public string Description { get; set; }

        public RssImage()
        {
            Title = string.Empty;
            Url = string.Empty;
            Link = string.Empty;
            Description = string.Empty;
        }
    }

    public class RssItem
    {
        public string Link { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Category { get; set; }

        public DateTime PubData { get; set; }

        public string Guid { get; set; }

        public string Description { get; set; }

        public RssItem()
        {
            Link = string.Empty;
            Title = string.Empty;
            Author = string.Empty;
            Category = string.Empty;
            PubData = DateTime.Now;
            Guid = string.Empty;
            Description = string.Empty;
        }
    }
}